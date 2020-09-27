using System;
using PathCreation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityTemplateProjects.IA
{
    public class IAPathWalk : MonoBehaviour
    {
        public PathCreator path;

        public float movementSpeed;
        float dstTravelled;

        private bool _isAfraid;
        private CooldownTimer _timerAfraid;

        public float timerCooldown;
        
        public EndOfPathInstruction endAction;

        private Animator _animator;
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            GetComponentInChildren<SpriteRenderer>().color 
                = Random.ColorHSV(0, 1, 0, 1, .5f, 1f);
            
            _animator.SetFloat(Speed, .50f);
            
            _timerAfraid = new CooldownTimer(timerCooldown);
            _timerAfraid.TimerCompleteEvent += () =>
            {
                _isAfraid = false;
                _animator.SetFloat(Speed, .50f);
            };
            
            MakeHimAfraid();
        }

        public void MakeHimAfraid()
        {
            _timerAfraid.Start();
            _isAfraid = true;
            _animator.SetFloat(Speed, 1.0f);
        }

        private void Update()
        {
            _timerAfraid.Update(Time.deltaTime);
            dstTravelled += (_isAfraid ? movementSpeed * 2.0f : movementSpeed) * Time.deltaTime;
            var newPos = path.path.GetPointAtDistance(dstTravelled, endAction);
            var direction = (newPos - transform.position).normalized;

            transform.position = newPos;
            transform.right = direction;
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
        }
    }
}