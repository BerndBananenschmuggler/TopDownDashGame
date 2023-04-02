using Assets.Scripts.GameManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(DestroyAction))]
[RequireComponent(typeof(Health))]
public class EnemyBehaviour : MonoBehaviour
{
    public bool CollisionTriggered = false;
    
    [SerializeField] private GameObject m_target;    

    private NavMeshAgent m_navAgent;
    private GameObject m_playerObject;
    private MovementBehaviour m_playerMovement;

    private Collider m_collider;
    private DestroyAction m_destroyAction;
    private Health m_health;
    

    private void Awake()
    {
        m_navAgent = GetComponent<NavMeshAgent>();
        m_destroyAction = GetComponent<DestroyAction>();
        m_collider = GetComponent<Collider>();
        m_health = GetComponent<Health>();
    }

    private void Start()
    {
        m_playerObject = GameManager.PlayerSpawnerInstance.GetActivePlayer();
        if(m_playerObject != null)
        {
            m_playerMovement = m_playerObject.GetComponent<MovementBehaviour>();
            m_playerMovement.OnDashed += HandleDash;
            m_playerMovement.OnDashStopped += HandleDashStopped;
        }        

        GameManager.PlayerSpawnerInstance.OnPlayerSpawned += HandlePlayerSpawned;
        GameManager.PlayerSpawnerInstance.OnPlayerDespawned += HandlePlayerDespawned;
    }
        
    void Update()
    {
        if (m_playerObject != null)
        {
            if (m_playerObject != m_target)
                m_target = m_playerObject;
            m_navAgent.SetDestination(m_target.transform.position);
        }
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

    public void Damage(float damage)
    {
        m_health.ApplyDamage(damage);
    }

    public void Heal(float heal)
    {
        m_health.ApplyHeal(heal);
    }

    public void Destroy()
    {
        // Contains -> Boss01 Copy registered aswell, just in case
        if (gameObject.name.Contains("Boss"))
            GameManager.Instance.BossKilled();

        m_destroyAction.Trigger();
    }

    private void HandlePlayerSpawned()
    {
        m_playerObject = GameManager.PlayerSpawnerInstance.GetActivePlayer();
        m_playerMovement = m_playerObject.GetComponent <MovementBehaviour>();
        m_playerMovement.OnDashed += HandleDash;
        m_playerMovement.OnDashStopped += HandleDashStopped;
    }

    private void HandlePlayerDespawned()
    {
        m_playerObject = null;
        m_playerMovement = null;
    }

    private void HandleDash()
    {
        m_collider.isTrigger = true;
    }

    private void HandleDashStopped()
    {
        m_collider.isTrigger = false;
    }   
}
