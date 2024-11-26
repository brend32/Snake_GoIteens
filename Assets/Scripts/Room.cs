using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Cell Cell;
    public int CellSize;
    public int Width = 10;
    public int Height = 10;

    public void Start()
    {
        CreateCells();
    }

    public Vector2 GetCellPosition(int x, int y)
    {
        return new Vector2(x * CellSize, y * CellSize);
    }

    public void CreateCells()
    {
        bool isOdd = false;
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                float spawnX = x * CellSize;
                float spawnY = y * CellSize;

                Cell cell = Instantiate(Cell, transform);
                cell.transform.localPosition = new Vector3(spawnX, spawnY);
                cell.SetColor(isOdd);

                isOdd = !isOdd;
            }
            
            isOdd = !isOdd;
        } 
    }
}
