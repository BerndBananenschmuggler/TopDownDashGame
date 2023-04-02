using Assets.Scripts.GameManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCollisionDetection : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == GameManager.PlayerSpawnerInstance.GetActivePlayer())
        {
            GameManager.Instance.LoadBossLevel();
        }
    }
}
