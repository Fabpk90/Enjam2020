using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IA
{
    public class HittableActor : MonoBehaviour
    {
        public int health;
        public EventHandler OnDeath;
        public GameObject poopHat;
        public Animator umbrella;
        public bool canBeHit = true;
        private static readonly int Hit = Animator.StringToHash("Hit");

        private void Start()
        {
            canBeHit = true;
            if (Random.Range(0, 4) == 0)
            {
                health = 2;
                umbrella.gameObject.SetActive(true);
            }
            else
            {
                health = 1;
            }
        }

        public virtual void TakeDamage(int amount)
        {
            if(!canBeHit) return;
            
            if(health - amount <= 0)
            {
                poopHat.SetActive(true);
                Death();
            }
            else if(health - amount == 1)
            {
                umbrella.SetTrigger(Hit);
                health -= amount;
            }
            else
            {
                health -= amount;
            }
        }

        public void Death()
        {
            OnDeath?.Invoke(this, null);
        }
    }
}