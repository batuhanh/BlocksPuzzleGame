using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GridSystem
{
    public class Cell : MonoBehaviour
    {
        public List<BlockPartData> connectedData;
        public List<BlockPartObject> connectedObject = new List<BlockPartObject>();
        public int x;
        public int y;

        public bool IsCompleted()
        {
            if ((connectedObject.Count == 1 && connectedObject[0].blockPartData.type == BlockPartType.Cube)
                || (connectedObject.Count == 2 && connectedObject[0].blockPartData.type != BlockPartType.Cube
                && connectedObject[1].blockPartData.type != BlockPartType.Cube))
            {
                return true;
            }
            return false;
        }
    }
}
