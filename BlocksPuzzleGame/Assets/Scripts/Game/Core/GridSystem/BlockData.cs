using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GridSystem
{
    [System.Serializable]
    public class BlockData 
    {
        public List<BlockPartData> parts = new List<BlockPartData>();
        public List<BlockPartDataSerializable> partsSerializable = new List<BlockPartDataSerializable>();
        public Color blockColor;
    }
}

