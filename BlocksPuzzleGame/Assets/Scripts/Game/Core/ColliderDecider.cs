using Game.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Game.Core
{
    public class ColliderDecider : MonoBehaviour, IProvidable
    {
        [SerializeField] private Vector2[] cubePoints;
        [SerializeField] private Vector2[] rightOutwardPoints;
        [SerializeField] private Vector2[] rightInwardPoints;
        [SerializeField] private Vector2[] leftOutwardPoints;
        [SerializeField] private Vector2[] leftInwardPoints;
        [SerializeField] private Vector2[] upOutwardPoints;
        [SerializeField] private Vector2[] upInwardPoints;
        [SerializeField] private Vector2[] downOutwardPoints;
        [SerializeField] private Vector2[] downInwardPoints;
        private void Awake()
        {
            ServiceProvider.Register(this);
        }
        public void Decide(BlockPartType type, ref PolygonCollider2D collider)
        {
            switch (type)
            {
                case BlockPartType.Cube:
                    collider.SetPath(0, cubePoints);
                    break;
                case BlockPartType.RightOutward:
                    collider.SetPath(0, rightOutwardPoints);
                    break;
                case BlockPartType.RightInward:
                    collider.SetPath(0, rightInwardPoints);
                    break;
                case BlockPartType.LeftOutward:
                    collider.SetPath(0, leftOutwardPoints);
                    break;
                case BlockPartType.LeftInward:
                    collider.SetPath(0, leftInwardPoints);
                    break;
                case BlockPartType.UpOutward:
                    collider.SetPath(0, upOutwardPoints);
                    break;
                case BlockPartType.UpInward:
                    collider.SetPath(0, upInwardPoints);
                    break;
                case BlockPartType.DownOutward:
                    collider.SetPath(0, downOutwardPoints);
                    break;
                case BlockPartType.DownInward:
                    collider.SetPath(0, downInwardPoints);
                    break;
            }
        }
    }
}

