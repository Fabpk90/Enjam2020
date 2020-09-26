using System;
using UnityEngine;

namespace Utils
{
    public class PathCreator : MonoBehaviour
    {
        [HideInInspector]
        public Path path;

        private void Start()
        {
            path = new Path(transform.position);
        }

        public void CreatePath()
        {
            
        }
    }
}