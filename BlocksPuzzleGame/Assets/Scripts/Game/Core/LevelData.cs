using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.GridSystem;

namespace Game.Core
{
    [System.Serializable]
    public class LevelData
    {
        public int blockCount;
        public int gridSize;
        public BlockData[] blockDatas;
        public string difficulty;
        public LevelData(int blockCount, int gridSize, BlockData[] blockDatas, string difficulty)
        {
            this.blockDatas = blockDatas;
            this.blockCount = blockCount; 
            this.gridSize = gridSize;
            this.difficulty = difficulty;
        }
    }
}
