using Assets.Scripts.GameManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.CollisionDetection
{
    public class EnemyCollisionDetection : MonoBehaviour
    {
        [SerializeField] private float m_collisionDamage = 1f;

        private GameObject m_player;

        private void Start()
        {
            GameManager.GameManager.PlayerSpawnerInstance.OnPlayerSpawned += HandlePlayerSpawned;
            GameManager.GameManager.PlayerSpawnerInstance.OnPlayerDespawned += HandlePlayerDespawned;
        }        

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject == GameManager.GameManager.PlayerSpawnerInstance.GetActivePlayer())
            {
                var playerHealth = collision.gameObject.GetComponent<Health>();
                playerHealth.ApplyDamage(m_collisionDamage);
            }
        }

        private void HandlePlayerSpawned()
        {
            m_player = GameManager.GameManager.PlayerSpawnerInstance.GetActivePlayer();

            var playerMovement = m_player.GetComponent<MovementBehaviour>();
            playerMovement.OnDashed += HandleDashed;
            playerMovement.OnDashStopped += HandleDashStopped;
        }

        private void HandleDashStopped()
        {
            throw new NotImplementedException();
        }

        private void HandleDashed()
        {
            throw new NotImplementedException();
        }

        private void HandlePlayerDespawned()
        {            
            //var playerMovement = m_player.GetComponent<MovementBehaviour>();
            //playerMovement.OnDashed -= HandleDashed;
            //playerMovement.OnDashStopped -= HandleDashStopped;

            m_player = null;
        }
    }
}
