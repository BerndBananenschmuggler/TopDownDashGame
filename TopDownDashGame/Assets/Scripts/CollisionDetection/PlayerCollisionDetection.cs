using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.GameManager;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.CollisionDetection
{
    public class PlayerCollisionDetection : MonoBehaviour
    {
        [SerializeField] private bool m_isDashing = false;
        [SerializeField] private float m_dashDamage = 1f;

        private void Awake()
        {
            GetComponent<MovementBehaviour>().OnDashed += HandlePlayerDashed;
            GetComponent<MovementBehaviour>().OnDashStopped += HandlePlayerDashStopped;
        }
        private void OnTriggerEnter(Collider other)
        {
            HandleTriggerCollision(other);
        }

        private void OnTriggerExit(Collider other)
        {
            var enemyBehaviour = other.GetComponent<EnemyBehaviour>();
            if (enemyBehaviour != null)
            {
                enemyBehaviour.CollisionTriggered = false;
            }
        }        

        private void HandleTriggerCollision(Collider other)
        {
            if (m_isDashing)
            {
                var enemyBehaviour = other.GetComponent<EnemyBehaviour>();

                if (enemyBehaviour != null)
                {
                    if (enemyBehaviour.CollisionTriggered == false)
                    {
                        enemyBehaviour.CollisionTriggered = true;
                        enemyBehaviour.Damage(m_dashDamage);

                        // Don't add Boss hits to Score
                        if (SceneManager.GetActiveScene().name == "NormalLevel") 
                            GameManager.GameManager.Instance.IncreaseScore();
                    }
                }
                else
                {
                    Debug.LogWarning("Can't Apply Damage because Enemy Components are missing");
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

}
