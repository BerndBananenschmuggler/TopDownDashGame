using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Spawner
{
    public class Spawner
    {
        public GameObject SpawnObject(GameObject obj, Vector3 spawnPoint) => Object.Instantiate(obj, spawnPoint, Quaternion.identity);
        public GameObject SpawnObject(GameObject obj, Vector3 spawnPoint, Quaternion rotation) => Object.Instantiate(obj, spawnPoint, rotation);
    }
}
