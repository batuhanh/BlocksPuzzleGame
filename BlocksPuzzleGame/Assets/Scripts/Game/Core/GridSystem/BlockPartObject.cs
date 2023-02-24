using Game.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Game.Core.GridSystem
{
    public class BlockPartObject : MonoBehaviour
    {
        public BlockPartData blockPartData;
        public BlockObject myBlockObject;
        public Cell myCell;
        public Cell closestCell = null;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private PolygonCollider2D myCollider;
        [SerializeField] private GameObject placerIconObj;
        public void Setup()
        {
            spriteRenderer.color = myBlockObject.blockData.blockColor;
            spriteRenderer.sprite = DetectSprite();
            if(ServiceProvider.GetColliderDecider)
                ServiceProvider.GetColliderDecider.Decide(blockPartData.type, ref myCollider);
        }
        private Sprite DetectSprite()
        {
            switch (blockPartData.type)
            {
                case BlockPartType.Cube:
                    return ServiceProvider.GetImageManager.Cube;
                case BlockPartType.RightOutward:
                    return ServiceProvider.GetImageManager.RightOutward;
                case BlockPartType.RightInward:
                    return ServiceProvider.GetImageManager.RightInward;
                case BlockPartType.LeftOutward:
                    return ServiceProvider.GetImageManager.LeftOutward;
                case BlockPartType.LeftInward:
                    return ServiceProvider.GetImageManager.LeftInward;
                case BlockPartType.UpOutward:
                    return ServiceProvider.GetImageManager.UpOutward;
                case BlockPartType.UpInward:
                    return ServiceProvider.GetImageManager.UpInward;
                case BlockPartType.DownOutward:
                    return ServiceProvider.GetImageManager.DownOutward;
                case BlockPartType.DownInward:
                    return ServiceProvider.GetImageManager.DownInward;
                default:
                    return null;

            }
        }
        public void SetPlacerIconStatus(bool isActive)
        {
            placerIconObj.SetActive(isActive);
        }
        public void SetLayerOrder(int newOrder)
        {
            spriteRenderer.sortingOrder = newOrder;
        }
        public void Snap()
        {
            myCell = closestCell;
            myCell.connectedObject.Add(this);
        }
        public void Unsnap()
        {
            closestCell = null;
            if (myCell && myCell.connectedObject != null && myCell.connectedObject.Contains(this))
                myCell.connectedObject.Remove(this);
            myCell = null;
        }
        public bool IsSnappable()
        {
            return ServiceProvider.GetGridObject.IsPartCloseToAnyCell(this, ref closestCell);
        }
    }
}
