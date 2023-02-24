using Game.Core.GridSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Managers
{
    public class BlocksManager : MonoBehaviour, IProvidable
    {
        private List<GameObject> spawnedBlocks;
        private Vector3 spawnCenterPos = new Vector3(0,-3.5f,0);
        private void Awake()
        {
            ServiceProvider.Register(this);
        }
        public void CreateBlockObjects(BlockData[] blockDatas)
        {
            spawnedBlocks = ServiceProvider.GetBlocksVisualizer.Visualize(blockDatas, true);
            int order = 1;
            foreach (GameObject block in spawnedBlocks)
            {
                float randomness = 1f;
                block.transform.position = spawnCenterPos + new Vector3(UnityEngine.Random.Range(-randomness, randomness), UnityEngine.Random.Range(-randomness, randomness));
                block.GetComponent<BlockObject>().SetLayerOrder(order++);
            }
        }
    }
}
