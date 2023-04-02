using Assets.Scripts.GameManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Spawner
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject m_enemyPrefab;
        [SerializeField][Range(0,20)] private float m_spawnRate = 2f;
        [SerializeField] private int m_maxEnemySpawnCount = 15;
        private float m_enemySpawnCount = 0;
        private bool m_isSpawning = true;

        private float m_currentTime;
        private float m_startTime;

        

        private void Start()
        {
            GetComponent<GameManager.GameManager>().OnGameEnded += HandleGameEnded;
            ResetTimer();            
        }
        
        private void Update()
        {
            if (!m_isSpawning)
                return;

            if (m_enemySpawnCount < m_maxEnemySpawnCount)
            {
                m_currentTime += Time.deltaTime;
                if (m_currentTime >= m_startTime + 1 / m_spawnRate)
                {
                    SpawnEnemy();
                    ResetTimer();
                }
            }
        }

        private void ResetTimer()
        {
            m_startTime = Time.time;
            m_currentTime = m_startTime;
        }

        private void SpawnEnemy()
        {
            m_enemySpawnCount++;

            float x = UnityEngine.Random.Range(-5f, 5f); // generate a random X coordinate within the range (-5, 5)
            float z = UnityEngine.Random.Range(-5f, 5f); // generate a random Z coordinate within the range (-5, 5)
            Vector3 position = new Vector3(x, 0, z);
            GameObject enemy = Instantiate(m_enemyPrefab, position, Quaternion.identity);
            Debug.Log($"Spawned Enemy: {position.ToString()}");
        }

        private void HandleGameEnded()
        {
            m_isSpawning = false;
        }
    }
}
