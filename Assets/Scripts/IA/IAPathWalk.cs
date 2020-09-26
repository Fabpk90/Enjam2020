using System;
using UnityEngine;

namespace UnityTemplateProjects.IA
{
    public class IAPathWalk : MonoBehaviour
    {
        public Transform[] points;
        private int _indexPoint;

        private Vector3 _nextPosition;
        
        public bool isMoving;
        public float movementSpeed;

        private Animator _stateMachine;
        private void Start()
        {
            _stateMachine = GetComponent<Animator>();
            _stateMachine.GetBehaviour<PointChooser>().ia = this;
            
            _indexPoint = 0;
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, _nextPosition, Time.deltaTime);
        }

        public void ChooseNextPoint()
        {
            _indexPoint = (_indexPoint + 1) % points.Length;

            _nextPosition = points[_indexPoint].position;
        }
    }
}