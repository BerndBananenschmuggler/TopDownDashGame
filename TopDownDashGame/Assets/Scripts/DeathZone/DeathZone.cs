using Assets.Scripts.GameManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (SceneManager.GetActiveScene().name != "BossLevel")
                RespawnPlayer();
            else
                KillPlayer();

        }
    }    

    private void RespawnPlayer()
    {
        GameManager.PlayerSpawnerInstance.DespawnPlayer();
        GameManager.PlayerSpawnerInstance.SpawnPlayer();
    }

    private void KillPlayer()
    {
        // Apply Max HP as Damage
        Health playerHealth = GameManager.PlayerSpawnerInstance.GetActivePlayer().GetComponent<Health>();
        playerHealth.ApplyDamage(playerHealth.MaxHealth);
    }
}
