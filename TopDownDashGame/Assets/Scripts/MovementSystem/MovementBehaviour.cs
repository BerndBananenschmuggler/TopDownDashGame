using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MovementBehaviour : MonoBehaviour
{
    public UnityEvent OnMove = null;
    public UnityEvent OnDashed = null;

    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private Camera m_camera;
    [SerializeField] private float m_movementSpeed = 1.0f;
    [SerializeField] private float m_rotationSpeed = 360f;      // 360Grad pro Sekunde
    private Vector2 m_movementInputValue = new();
    private Movement m_movement;
    private PlayerInputActions m_playerInputActions;
    private InputAction m_playerMovementAction;
    private InputAction m_playerDashAction;
    private Transform m_rotatingTransform;

    [SerializeField] private float m_dashSpeedMultiplicator = 3f;
    [SerializeField] private float m_dashCooldown = 5f;         // Sekunden
    [SerializeField] private float m_dashDuration = 1.5f;       // Sekundem
    private bool m_canDash = true;
    private bool m_isDashing = false;
    private float m_dashInputValue = 0;   

    private void Awake()
    {
        if (m_rigidbody == null)
            m_rigidbody = GetComponent<Rigidbody>();
        //m_playerInputActions = new PlayerInputActions();
        m_movement = new Movement(m_rigidbody);

        if (m_camera == null)
            m_camera = Camera.main;

        m_playerInputActions = new();

        m_rotatingTransform = transform.GetChild(0).transform;
    }

    private void OnEnable()
    {
        m_playerMovementAction = m_playerInputActions.Player.Move;
        m_playerDashAction = m_playerInputActions.Player.Dash;
        m_playerInputActions.Enable();
    }

    private void OnDisable() => m_playerInputActions.Disable();

    private void Update()
    {
        GetInput();
        InjectInputIntoMovement();
        
        if (m_movementInputValue.x != 0 || m_movementInputValue.y != 0)
            OnMove?.Invoke();
    }

    private IEnumerator Dash()
    {
        m_rigidbody.useGravity = false;        
        m_canDash = false;
        m_isDashing = true;

        m_movement.StartDashRigidbody(m_movementSpeed * m_dashSpeedMultiplicator);
        OnDashed?.Invoke();

        yield return new WaitForSeconds(m_dashDuration);

        m_movement.StopDashRigidbody();
        m_isDashing = false;
        m_rigidbody.useGravity = true;

        yield return new WaitForSeconds(m_dashCooldown);

        m_canDash = true;

        StopCoroutine(Dash());
    }

    private void GetInput()
    {
        m_movementInputValue = m_playerMovementAction.ReadValue<Vector2>();
        m_dashInputValue = m_playerDashAction.ReadValue<float>();
    }

    private void InjectInputIntoMovement()
    {
        if (m_canDash && m_dashInputValue > 0 && m_movementInputValue.magnitude > 0) 
        {
            StartCoroutine(Dash());
        }

        if (m_isDashing == true)
            return;

        Vector3 movementValue3D = new Vector3(m_movementInputValue.x, 0, m_movementInputValue.y);        

        m_movement.MoveRigidbody(movementValue3D, m_movementSpeed);
        m_movement.RotateTransformTowardsForward(m_rotatingTransform, movementValue3D, m_rotationSpeed);

        
    }


}
