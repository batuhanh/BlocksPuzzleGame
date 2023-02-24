using Game.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GridSystem
{
    public class BlockObject : MonoBehaviour
    {
        public BlockData blockData;
        [SerializeField] private GameObject blockpartPrefab;
        private List<BlockPartObject> partsObjects = new List<BlockPartObject>();
        public static event Action blockSnappedEvent;
        public void Setup(bool isPlayable)
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            if (isPlayable)
            {
                if (blockData.partsSerializable[0] != null)
                    transform.position = ServiceProvider.GetGridObject.GetWorldPos(blockData.partsSerializable[0].gridX, blockData.partsSerializable[0].gridY);
                for (int i = 0; i < blockData.partsSerializable.Count; i++)
                {
                    Vector3 spawnPos = ServiceProvider.GetGridObject.GetWorldPos(blockData.partsSerializable[i].gridX, blockData.partsSerializable[i].gridY);
                    GameObject spawnedBlockPart = Instantiate(blockpartPrefab, spawnPos, Quaternion.identity, transform);
                    BlockPartObject blockPartObject = spawnedBlockPart.GetComponent<BlockPartObject>();
                    BlockPartData bpd = new BlockPartData();
                    bpd.gridX = blockData.partsSerializable[i].gridX;
                    bpd.gridY = blockData.partsSerializable[i].gridY;
                    bpd.type = blockData.partsSerializable[i].type;
                    bpd.blockData = blockData;
                    bpd.cell = null;
                    blockPartObject.blockPartData = bpd;
                    blockPartObject.myBlockObject = this;
                    partsObjects.Add(blockPartObject);
                    blockPartObject.Setup();
                }
            }
            else
            {
                if (blockData.parts[0] != null)
                    transform.position = blockData.parts[0].cell.transform.position;
                for (int i = 0; i < blockData.parts.Count; i++)
                {
                    Vector3 spawnPos = blockData.parts[i].cell.transform.position;
                    GameObject spawnedBlockPart = Instantiate(blockpartPrefab, spawnPos, Quaternion.identity, transform);
                    BlockPartObject blockPartObject = spawnedBlockPart.GetComponent<BlockPartObject>();
                    blockPartObject.myBlockObject = this;
                    blockPartObject.myCell = blockData.parts[i].cell;
                    blockPartObject.blockPartData = blockData.parts[i];
                    blockPartObject.Setup();
                }
            }
        }
        public void OnHolded(int clickCount)
        {
            foreach (BlockPartObject bpo in partsObjects)
            {
                bpo.Unsnap();
                bpo.SetLayerOrder(15 + clickCount);//15 is a value that any blocks can not be bigger than that
                if (bpo.blockPartData.type != BlockPartType.Cube)
                    bpo.SetPlacerIconStatus(true);
            }
        }
        public void OnReleased()
        {
            if (IsSnappable())
            {
                Snap();
            }
            foreach (BlockPartObject bpo in partsObjects)
            {
                bpo.SetPlacerIconStatus(false);
            }
        }
        public void SetLayerOrder(int newOrder)
        {
            foreach (BlockPartObject bpo in partsObjects)
            {
                bpo.SetLayerOrder(newOrder);
            }
        }
        public bool IsSnappable()
        {
            foreach (BlockPartObject bpo in partsObjects)
            {
                if (!bpo.IsSnappable())
                {
                    return false;
                }
            }
            return true;
        }
        public void Snap()
        {
            Vector3 moveAmount = partsObjects[0].closestCell.transform.position - partsObjects[0].transform.position;
            transform.position += moveAmount;
            foreach (BlockPartObject bpo in partsObjects)
            {
                bpo.Snap();
            }
            blockSnappedEvent?.Invoke();
        }
    }
}

