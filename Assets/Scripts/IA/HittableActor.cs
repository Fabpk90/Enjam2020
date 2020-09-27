using System;
using UnityEngine;

namespace IA
{
    public class HittableActor : MonoBehaviour
    {
        public int health;
        public EventHandler OnDeath;

        [FMODUnity.EventRef]
        public string hitEvent;

        private FMOD.Studio.EventInstance fmodinstance;

        public virtual void TakeDamage(int amount)
        {
            if (health - amount <= 0)
            {
                Death();
                fmodinstance = FMODUnity.RuntimeManager.CreateInstance(hitEvent);
                fmodinstance.start();
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