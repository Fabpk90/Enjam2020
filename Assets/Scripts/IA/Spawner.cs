using System;
using PathCreation;
using UnityEngine;
using UnityTemplateProjects.IA;

namespace IA
{
    public class Spawner : MonoBehaviour
    {
        public GameObject prefabToSpawn;
        public PathCreator path;

        public float spawnCooldown;
        private CooldownTimer _timerToRespawn;

        public int amountToSpawn;

        private void Start()
        {
            _timerToRespawn = new CooldownTimer(spawnCooldown, true);
            _timerToRespawn.TimerCompleteEvent += () =>
            {
                var go = Instantiate(prefabToSpawn, transform.position, Quaternion.identity); 
                go.GetComponent<IAPathWalk>().path = path; 
                amountToSpawn--;

                if (amountToSpawn == 0)
                    _timerToRespawn.Pause();
            };
            
            _timerToRespawn.Start();
            
            GameManager.instance.spawners.Add(this);
        }

        public void NextWave(int amountToSpawn)
        {
            this.amountToSpawn = amountToSpawn;
            _timerToRespawn.Start();
        }

        private void Update()
        {
            _timerToRespawn.Update(Time.deltaTime);
        }
    }
}