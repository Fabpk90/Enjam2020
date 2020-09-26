using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    
    public class Path
    {
        [SerializeField, HideInInspector]
        public List<Vector2> points;

        public int GetNumSegments()
        {
            //we have 4 initial points
            return (points.Count - 4) / 3 + 1;
        }

        public int GetNumPoints()
        {
            return points.Count;
        }

        public Vector2 this[int i]
        {
            get
            {
                return points[i];
            }
        }
        
        public Path(Vector2 center)
        {
            points = new List<Vector2>()
            {
                center + Vector2.left,
                center + (Vector2.left + Vector2.left) * 0.5f,
                center + (Vector2.right + Vector2.down) * 0.5f,
                center + Vector2.right
            };
        }

        public void AddSegment(Vector2 anchor)
        {
            points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
            points.Add((anchor + points[points.Count - 1]) * .5f);
            points.Add(anchor);
        }

        public Vector2[] GetPointsInSegment(int i)
        {
            return new[] {points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[i * 3 + 3]};
        }

        public void MovePoint(int index, Vector2 newPos)
        {
            points[index] = newPos;
        }
    }
}