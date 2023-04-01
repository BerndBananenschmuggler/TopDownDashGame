using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionDetection : MonoBehaviour
{
    [SerializeField] private bool m_isDashing = false;
    private GameObject m_lastCollidedObject;

    private void Awake()
    {
        GetComponent<MovementBehaviour>().OnDashed += HandlePlayerDashed;
        GetComponent<MovementBehaviour>().OnDashStopped += HandlePlayerDashStopped;
    }      

    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other);
    }

    private void OnTriggerStay(Collider other)
    {
        HandleCollision(other);
    }

    private void HandleCollision(Collider other)
    {
        if (m_isDashing)
        {
            if (m_lastCollidedObject != other.gameObject)
            {
                var enemyBehaviour = other.GetComponent<EnemyBehaviour>();
                if (enemyBehaviour != null)
                {
                    m_lastCollidedObject = other.gameObject;
                    enemyBehaviour.Destroy();
                    GameManager.Instance.IncreaseScore();
                }
            }
        }
    }

    private void HandlePlayerDashed()
    {
        m_isDashing = true;
    }

    private void HandlePlayerDashStopped()
    {
        m_isDashing = false;
    }
}
