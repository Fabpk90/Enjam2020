using System;
using UnityEditor;
using UnityEngine;

namespace Utils
{
    [CustomEditor(typeof(PathCreator))]
    public class PathEditor : Editor
    {
        private PathCreator creator;
        private Path path;

        private void OnEnable()
        {
            creator = (PathCreator) target;

            if (creator.path == null)
            {
                creator.CreatePath();
            }
        }
    }
}