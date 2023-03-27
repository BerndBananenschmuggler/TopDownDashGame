using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MovementBehaviour : MonoBehaviour
{
    public UnityEvent OnMove = null;

    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private float m_movementSpeed = 1.0f;
    [SerializeField] private float m_rotationSpeed = 360f;      // 360Grad pro Sekunde
    [SerializeField] private Camera m_camera;

    private Vector2 m_movementValue = new();
    private Movement m_movement;
    private PlayerInputActions m_playerInputActions;
    private InputAction m_playerMovementAction;
    private Transform m_rotatingTransform;

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
        m_playerInputActions.Enable();
    }

    private void OnDisable() => m_playerInputActions.Disable();

    private void Update()
    {
        GetInput();
        InjectInputIntoMovement();
        if ((Mathf.Abs(m_movementValue.x) != 0 || Mathf.Abs(m_movementValue.y) != 0))
            OnMove?.Invoke();
    }

    private void GetInput()
    {
        m_movementValue = m_playerMovementAction.ReadValue<Vector2>();
    }

    private void InjectInputIntoMovement()
    {
        Vector3 movementValue3D = new Vector3(m_movementValue.x, 0, m_movementValue.y);

        m_movement.MoveRigidbody(movementValue3D, m_movementSpeed);
        m_movement.RotateTransformTowardsForward(m_rotatingTransform, movementValue3D, m_rotationSpeed);
    }


}
