using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.GridSystem;
using Game.Managers;

namespace Utils
{
    public class LevelCreatorObject : MonoBehaviour
    {
        [SerializeField] private GridObject gridObject;
        LevelData curLevelData;
        public void OnCreateLevelButtonClicked(int blockCount, int gridSize, string difficulty, bool isTriangular, Color[] blockColors)
        {
            gridObject.InitData(gridSize);
            BlockData[] blockDatas = CreateBlocks(blockCount, gridSize, isTriangular, blockColors);
            curLevelData = new LevelData(blockCount, gridSize, blockDatas, difficulty);
            DisplayLevel();
        }
        public void OnSaveLevelDataButtonClicked()
        {
            ServiceProvider.GetDataHandler.SaveLevelData(curLevelData);
        }
        private BlockData[] CreateBlocks(int BlockCount, int gridSize, bool isTriangular, Color[] blockColors)
        {
            BlockData[] blockdatas = new BlockData[BlockCount];

            //Step 1 assigning first blockpart of each block
            for (int i = 0; i < blockdatas.Length; i++)
            {
                blockdatas[i] = CreateNewBlock(gridSize, blockColors[i]);
            }

            //Step 2 add new blockparts to blocks. 
            while (!gridObject.IsFull()) //Step 3 repeat if any null cell exist
            {
                for (int i = 0; i < blockdatas.Length; i++)
                {
                    int maxBlockPartCount = UnityEngine.Random.Range(0, (gridSize * gridSize) / blockdatas.Length);
                    for (int j = 0; j < blockdatas[i].parts.Count; j++)
                    {
                        Cell randomPartCell = blockdatas[i].parts[j].cell;
                        int x = randomPartCell.x;
                        int y = randomPartCell.y;
                        if (x < gridSize - 1)
                        {
                            if (gridObject.cells[x + 1, y].connectedData == null)
                            {
                                AddNewPartToBlock(blockdatas[i], x + 1, y, BlockPartType.Cube);
                                maxBlockPartCount--;
                                if (maxBlockPartCount <= 0)
                                    break;
                            }
                        }
                        if (x > 0)
                        {
                            if (gridObject.cells[x - 1, y].connectedData == null)
                            {
                                AddNewPartToBlock(blockdatas[i], x - 1, y,BlockPartType.Cube);
                                maxBlockPartCount--;
                                if (maxBlockPartCount <= 0)
                                    break;
                            }
                        }
                        if (y > 0)
                        {
                            if (gridObject.cells[x, y - 1].connectedData == null)
                            {
                                AddNewPartToBlock(blockdatas[i], x, y - 1, BlockPartType.Cube);
                                maxBlockPartCount--;
                                if (maxBlockPartCount <= 0)
                                    break;
                            }
                        }
                        if (y < gridSize - 1)
                        {
                            if (gridObject.cells[x, y + 1].connectedData == null)
                            {
                                AddNewPartToBlock(blockdatas[i], x, y + 1, BlockPartType.Cube);
                                maxBlockPartCount--;
                                if (maxBlockPartCount <= 0)
                                    break;
                            }
                        }
                    }
                }
            }

            //Step 4: if it is triangular. Add traingle to random points
            if (isTriangular)
            {
                int triangleCount = UnityEngine.Random.Range(1, gridSize + 1);
                for (int i = 0; i < triangleCount; i++)
                {
                    Cell cell = gridObject.cells[UnityEngine.Random.Range(0, gridSize),
                UnityEngine.Random.Range(0, gridSize)];
                    List<TriangleDirection> possibleTriangles = DetectTriangles(cell, gridSize);
                    if (possibleTriangles.Count > 0)
                    {
                        TriangleDirection selectedDirection = possibleTriangles[UnityEngine.Random.Range(0, possibleTriangles.Count)];
                        CreateTriangle(selectedDirection, cell);
                    }
                }
            }
            return blockdatas;
        }
        private void CreateTriangle(TriangleDirection direction, Cell cell)
        {
            BlockPartData newBlock;
            switch (direction)
            {
                case TriangleDirection.None:
                    break;
                case TriangleDirection.Up:
                    cell.connectedData[0].type = BlockPartType.DownOutward;
                    cell.connectedData[0].serializableData.type = BlockPartType.DownOutward;
                    newBlock = AddNewPartToBlock(gridObject.cells[cell.x, cell.y + 1].connectedData[0].blockData, cell.x, cell.y, BlockPartType.UpInward);
                    break;
                case TriangleDirection.Down:
                    cell.connectedData[0].type = BlockPartType.UpOutward;
                    cell.connectedData[0].serializableData.type = BlockPartType.UpOutward;
                    newBlock = AddNewPartToBlock(gridObject.cells[cell.x, cell.y - 1].connectedData[0].blockData, cell.x, cell.y, BlockPartType.DownInward);
                    break;
                case TriangleDirection.Right:
                    cell.connectedData[0].type = BlockPartType.RightInward;
                    cell.connectedData[0].serializableData.type = BlockPartType.RightInward;
                    newBlock = AddNewPartToBlock(gridObject.cells[cell.x + 1, cell.y].connectedData[0].blockData, cell.x, cell.y, BlockPartType.LeftOutward);
                    break;
                case TriangleDirection.Left:
                    cell.connectedData[0].type = BlockPartType.LeftInward;
                    cell.connectedData[0].serializableData.type = BlockPartType.LeftInward;
                    newBlock = AddNewPartToBlock(gridObject.cells[cell.x - 1, cell.y].connectedData[0].blockData, cell.x, cell.y, BlockPartType.RightOutward);
                    break;
            }

        }
        private List<TriangleDirection> DetectTriangles(Cell cell, int gridSize)
        {
            int x = cell.x;
            int y = cell.y;
            BlockData currentBlock = cell.connectedData[0].blockData;
            List<TriangleDirection> directions = new List<TriangleDirection>();

            if (x < gridSize - 1 && x > 0)
            {
                if (x < gridSize - 1 && gridObject.cells[x + 1, y].connectedData[0].blockData != currentBlock
               && gridObject.cells[x - 1, y].connectedData[0].blockData == currentBlock
               && gridObject.cells[x - 1, y].connectedData[0].type == BlockPartType.Cube
               && gridObject.cells[x + 1, y].connectedData[0].type == BlockPartType.Cube)
                {
                    directions.Add(TriangleDirection.Right);
                }
                if (x > 0 && gridObject.cells[x - 1, y].connectedData[0].blockData != currentBlock
                    && gridObject.cells[x + 1, y].connectedData[0].blockData == currentBlock
                    && gridObject.cells[x + 1, y].connectedData[0].type == BlockPartType.Cube
                     && gridObject.cells[x - 1, y].connectedData[0].type == BlockPartType.Cube)
                {
                    directions.Add(TriangleDirection.Left);
                }
            }

            if (y < gridSize - 1 && y > 0)
            {
                if (y > 0 && gridObject.cells[x, y - 1].connectedData[0].blockData != currentBlock
                    && gridObject.cells[x, y + 1].connectedData[0].blockData == currentBlock
                    && gridObject.cells[x, y + 1].connectedData[0].type == BlockPartType.Cube
                 && gridObject.cells[x, y - 1].connectedData[0].type == BlockPartType.Cube)
                {
                    directions.Add(TriangleDirection.Down);
                }
                if (y < gridSize - 1 && gridObject.cells[x, y + 1].connectedData[0].blockData != currentBlock
                    && gridObject.cells[x, y - 1].connectedData[0].blockData == currentBlock
                    && gridObject.cells[x, y - 1].connectedData[0].type == BlockPartType.Cube
                     && gridObject.cells[x, y + 1].connectedData[0].type == BlockPartType.Cube)
                {
                    directions.Add(TriangleDirection.Up);
                }
            }

            return directions;
        }
        private BlockData CreateNewBlock(int gridSize, Color blockColor)
        {
            BlockData blockdata = new BlockData();
            Cell cell = gridObject.cells[UnityEngine.Random.Range(0, gridSize),
                UnityEngine.Random.Range(0, gridSize)];
            while (cell.connectedData != null)
            {
                cell = gridObject.cells[UnityEngine.Random.Range(0, gridSize),
                UnityEngine.Random.Range(0, gridSize)];
            }
            AddNewPartToBlock(blockdata, cell.x, cell.y, BlockPartType.Cube);
            blockdata.blockColor = blockColor;
            return blockdata;
        }
        private BlockPartData AddNewPartToBlock(BlockData blockData, int x, int y,BlockPartType blockPartType)
        {
            BlockPartData newBlockPartData = new BlockPartData();
            BlockPartDataSerializable newBlockPartDataSer = new BlockPartDataSerializable();
            newBlockPartData.blockData = blockData;
            newBlockPartData.cell = gridObject.cells[x, y];
            newBlockPartData.gridX = x;
            newBlockPartData.gridY = y;
            newBlockPartData.type = blockPartType;
            newBlockPartData.serializableData =newBlockPartDataSer;
            blockData.parts.Add(newBlockPartData);
           
            if (gridObject.cells[x, y].connectedData == null)
                gridObject.cells[x, y].connectedData = new List<BlockPartData>();
            gridObject.cells[x, y].connectedData.Add(newBlockPartData);

            newBlockPartDataSer.gridX = x;
            newBlockPartDataSer.gridY = y;
            newBlockPartDataSer.type = blockPartType;
            blockData.partsSerializable.Add(newBlockPartDataSer);
            return newBlockPartData;
        }
        private void DisplayLevel()
        {
            ServiceProvider.GetBlocksVisualizer.Visualize(curLevelData.blockDatas,false);
        }


    }
}
