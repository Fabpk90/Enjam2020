using System;
using UnityEngine;

namespace IA
{
    public class HittableActor : MonoBehaviour
    {
        public int health;
        public EventHandler OnDeath;
        
        public virtual void TakeDamage(int amount)
        {
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