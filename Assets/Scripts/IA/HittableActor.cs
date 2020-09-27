using System;
using UnityEngine;

namespace IA
{
    public class HittableActor : MonoBehaviour
    {
        public int health;
        public EventHandler OnDeath;

        public bool canBeHit;
        
        public virtual void TakeDamage(int amount)
        {
            if(!canBeHit) return;
            
            if(health - amount <= 0)
                Death();
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