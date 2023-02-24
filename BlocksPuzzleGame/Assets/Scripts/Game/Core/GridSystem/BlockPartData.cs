using Game.Core.GridSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GridSystem
{
    public class BlockPartData
    {
        public BlockPartType type;
        public BlockData blockData;
        public BlockPartDataSerializable serializableData;
        public Cell cell;
        public int gridX;
        public int gridY;
    }

    [Serializable]
    public class BlockPartDataSerializable
    {
        public BlockPartType type;
        public int gridX;
        public int gridY;
    }
}
