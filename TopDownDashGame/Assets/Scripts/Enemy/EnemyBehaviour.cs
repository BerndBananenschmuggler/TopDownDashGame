using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(DestroyAction))]
public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject m_target;    

    private NavMeshAgent m_navAgent;
    private GameObject m_playerObject;
    private MovementBehaviour m_playerMovement;
    private Collider m_collider;
    private DestroyAction m_destroyAction;

    private void Awake()
    {
        m_navAgent = GetComponent<NavMeshAgent>();
        m_destroyAction = GetComponent<DestroyAction>();
        m_collider = GetComponent<Collider>();
    }

    private void Start()
    {
        if (GameManager.PlayerSpawnerInstance.GetActivePlayer() == null)
            return;

        m_playerObject = GameManager.PlayerSpawnerInstance.GetActivePlayer();
        m_playerMovement = m_playerObject.GetComponent<MovementBehaviour>();
        m_playerMovement.OnDashed += HandleDash;
        m_playerMovement.OnDashStopped += HandleDashStopped;

        GameManager.PlayerSpawnerInstance.OnPlayerSpawned += HandlePlayerSpawned;
        GameManager.PlayerSpawnerInstance.OnPlayerDespawned += HandlePlayerDespawned;

    }

    private void HandleDash()
    {
        m_collider.isTrigger = true;
    }

    private void HandleDashStopped()
    {
        m_collider.isTrigger = false;
    }

    

    private void OnDestroy()
    {
        GameManager.PlayerSpawnerInstance.OnPlayerSpawned -= HandlePlayerSpawned;
        GameManager.PlayerSpawnerInstance.OnPlayerDespawned -= HandlePlayerDespawned;

        if(m_playerMovement != null)
        {
            m_playerMovement.OnDashed -= HandleDash;
            m_playerMovement.OnDashStopped -= HandleDashStopped;
        }
    }

    private void HandlePlayerSpawned()
    {
        m_playerObject = GameManager.PlayerSpawnerInstance.GetActivePlayer();
        m_playerMovement = m_playerObject.GetComponent <MovementBehaviour>();
    }

    private void HandlePlayerDespawned()
    {
        m_playerObject = null;
        m_playerMovement = null;
    }


    // Update is called once per frame
    void Update()
    {
        if(m_playerObject != null)
        {
            m_navAgent.SetDestination(m_playerObject.transform.position);
        }
        
    }

    public void Destroy()
    {
        m_destroyAction.Trigger();
    }
}
