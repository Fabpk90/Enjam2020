using System;
using PathCreation;
using UnityEngine;

namespace IA
{
    public class Spawner : MonoBehaviour
    {
        public GameObject prefabToSpawn;
        public PathCreator path;

        public float spawnCooldown;
        private CooldownTimer _timerToRespawn;

        private void Start()
        {
            
        }
    }
}