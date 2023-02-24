using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.GridSystem;

namespace Game.Managers
{
    public class BlocksVisualizer : MonoBehaviour, IProvidable
    {
        [SerializeField] private GameObject blockPrefab;
        private void Awake()
        {
            ServiceProvider.Register(this);
        }
        public List<GameObject> Visualize(BlockData[] blocks, bool isPLayable)
        {
            int childCount = transform.childCount;
            List<GameObject> spawnedBlocks = new List<GameObject>();
            for (int i = 0; i < childCount; i++)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            for (int i = 0; i < blocks.Length; i++)
            {
                GameObject spawnedBlock = Instantiate(blockPrefab,transform.position,Quaternion.identity,transform);
                BlockObject blockObject = spawnedBlock.GetComponent<BlockObject>();
                blockObject.blockData = blocks[i];
                blockObject.Setup(isPLayable);
                spawnedBlocks.Add(spawnedBlock);
            }
            return spawnedBlocks;
        }
    }
}
