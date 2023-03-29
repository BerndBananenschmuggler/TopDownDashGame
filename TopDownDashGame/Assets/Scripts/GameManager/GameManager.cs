using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private GameObject m_playerInstance;
    [SerializeField] private Transform m_defaultPlayerSpawnPoint;

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

        if(m_defaultPlayerSpawnPoint == null)
        {
            Debug.LogWarning("No Default SpawnPoint selected for Player.");
        }

        if (playerPrefab == null)
        {
            Debug.LogWarning("No Player Prefab selected.");
        }

        InitPlayer();
    }   

    private void Start()
    {
        if(m_playerInstance == null)
        {
            //SpawnPlayer();
        }
    }

    public void SpawnPlayer()
    {
        if (m_playerInstance != null)
            return;

        m_playerInstance = Instantiate(playerPrefab, m_defaultPlayerSpawnPoint.position, Quaternion.identity);
    }

    public void DespawnPlayer()
    {
        m_playerInstance.GetComponent<DestroyAction>().Trigger();
        m_playerInstance = null;
    }

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
