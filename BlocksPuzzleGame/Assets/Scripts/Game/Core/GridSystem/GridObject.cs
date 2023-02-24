using Game.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Core.GridSystem
{
    public class GridObject : MonoBehaviour, IProvidable
    {
        public Cell[,] cells;
        private int size;
        [SerializeField] private Transform bgOutline;
        [SerializeField] private Transform bgInside;
        [SerializeField] private Transform cellsParent;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private Animator myAnim;
        private float outlineGap = 0.2f;
        private float cellSize = 0.5f;

        public static event Action gridCompletedEvent;
        private void Awake()
        {
            ServiceProvider.Register(this);
        }

        public void InitData(LevelData levelData)
        {
            InitData(levelData.gridSize);
        }
        public void InitData(int newSize)
        {
            size = newSize;
            cells = new Cell[size, size];
            bgInside.localScale = new Vector3(cellSize * size, cellSize * size, 1);
            bgOutline.localScale = new Vector3(bgInside.localScale.x + outlineGap, bgInside.localScale.y + outlineGap, 1);
            int childCount = cellsParent.childCount;
            for (int i = 0; i < childCount; i++)
            {
                DestroyImmediate(cellsParent.GetChild(0).gameObject);
            }

            Vector3 startPos = new Vector3(transform.position.x - (((size - 1) * cellSize) / 2f),
               transform.position.y + (((size - 1) * cellSize) / 2f), transform.position.z);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Vector3 spawnPos = new Vector3(startPos.x + (i * cellSize), startPos.y - (j * cellSize), transform.position.z);
                    cells[i, j] = Instantiate(cellPrefab, spawnPos, Quaternion.identity, cellsParent).GetComponent<Cell>();
                    cells[i, j].x = i;
                    cells[i, j].y = j;
                }
            }
            Camera.main.orthographicSize = Mathf.Clamp(size,4,5);
        }
     
        public bool IsFull()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (cells[i, j].connectedData == null)
                        return false;
                }
            }
            return true;
        }
        public Vector3 GetWorldPos(int gridX, int gridY)
        {
            return cells[gridX, gridY].transform.position;
        }

        public bool IsPartCloseToAnyCell(BlockPartObject bpo, ref Cell closestCell)
        {
            Vector3 pos = bpo.gameObject.transform.position;
            float closeGap = 0.15f;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (Vector3.Distance(cells[i, j].transform.position, pos) < closeGap)
                    {
                        closestCell = cells[i, j];
                        return true;
                    }
                        
                }
            }
            closestCell = null;
            return false;
        }
        public void CheckGridComplete()
        {
            bool isCompleted = true;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (!cells[i,j].IsCompleted())
                    {
                        isCompleted= false;
                        break;
                    }
                }
            }
            if (isCompleted)
            {
                myAnim.SetTrigger("Shine");
                gridCompletedEvent?.Invoke();
            }        
        }
        private void OnEnable()
        {
            BlockObject.blockSnappedEvent += CheckGridComplete;
            LevelManager.currentLevelSettedEvent += InitData;
        }
        private void OnDisable()
        {
            BlockObject.blockSnappedEvent -= CheckGridComplete;
            LevelManager.currentLevelSettedEvent -= InitData;
        }

    }
}
