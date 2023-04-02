using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Spawner
{
    public class PlayerSpawner : MonoBehaviour 
    {
        public static PlayerSpawner Instance { get; private set; }

        public event Action OnPlayerSpawned;
        public event Action OnPlayerDespawned;

        [SerializeField] private GameObject m_playerPrefab;
        [SerializeField] private GameObject m_playerInstance;
        [SerializeField] private Transform m_defaultPlayerSpawnPoint;

        private Spawner m_playerSpawner;


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            else
            {
                Instance = this;
            }

            m_playerSpawner = new Spawner();
        }
                
        private void Start()
        {
            InitPlayer();
        }

        private void OnDestroy()
        {
            OnPlayerDespawned = null;
            OnPlayerDespawned = null;
        }

        public void SpawnPlayer()
        {
            if (m_playerInstance != null)
                return;

            m_playerInstance = m_playerSpawner.SpawnObject(m_playerPrefab, m_defaultPlayerSpawnPoint.position);
            OnPlayerSpawned?.Invoke();
        }

        public void DespawnPlayer()
        {
            OnPlayerDespawned?.Invoke();

            m_playerInstance.GetComponent<DestroyAction>().Trigger();
            m_playerInstance = null;
        }

        public GameObject GetActivePlayer() => m_playerInstance;

        private void InitPlayer()
        {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

            if (playerObjects.Length == 0)
            {
                SpawnPlayer();
            }
            else if (playerObjects.Length == 1)
            {
                m_playerInstance = playerObjects[0];
            }
            else if (playerObjects.Length > 1)
            {
                // Debug Value
                int originalObjectsLength = playerObjects.Length;

                // Get the first Player Object and Destroy all others
                for (int i = 0; i < playerObjects.Length; i++)
                {
                    if (i == 0)
                    {
                        m_playerInstance = playerObjects[i];
                        continue;
                    }

                    Destroy(playerObjects[i]);

                    Debug.LogWarning($"Too many Player Objects ({playerObjects.Length})! Destroyed unnecessary Objects.");
                }
            }
        }
    }
}
