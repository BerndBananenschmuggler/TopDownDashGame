using Assets.Scripts.GameManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            RespawnPlayer();
        }
    }    

    private void RespawnPlayer()
    {
        GameManager.PlayerSpawnerInstance.DespawnPlayer();
        GameManager.PlayerSpawnerInstance.SpawnPlayer();
    }
}
