using System;
using PathCreation;
using UnityEngine;

namespace UnityTemplateProjects.IA
{
    public class IAPathWalk : MonoBehaviour
    {
        public PathCreator path;

        public float movementSpeed;
        float dstTravelled;
        
        public EndOfPathInstruction endAction;

        private void Update()
        {
            dstTravelled += movementSpeed * Time.deltaTime;
            transform.position = path.path.GetPointAtDistance(dstTravelled, endAction);
            transform.rotation = path.path.GetRotationAtDistance(dstTravelled, endAction);
        }
    }
}