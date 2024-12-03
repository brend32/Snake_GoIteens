using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Cell Cell;
    public int Width = 10;
    public int Height = 10;
    public BoxCollider2D RoomBounds;
    public SnakeController Snake;
    public FruitSpawner FruitSpawner;

    private List<Fruit> _fruits = new();

    public void Start()
    {
        CreateCells();
        FitToBounds();
        
        FruitSpawner.SpawnFruit();
    }

    public bool TryGetFruitAtPosition(Vector2Int roomPosition, out Fruit fruit)
    {
        fruit = _fruits.FirstOrDefault(f => f.RoomPosition == roomPosition);

        return fruit != null;
    }

    public void AddFruit(Fruit fruit)
    {
        _fruits.Add(fruit);
    }

    public void RemoveAndSpawnNewFruit(Fruit fruit)
    {
        FruitSpawner.UpdatePosition(fruit);
    }

    public List<Vector2Int> GetFreeCells()
    {
        List<Vector2Int> occupiedCells = _fruits.Select(f => f.RoomPosition).ToList();
        occupiedCells.AddRange(Snake.GetOccupiedCells());

        List<Vector2Int> freeCells = new List<Vector2Int>();
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector2Int position = new Vector2Int(x, y);

                if (occupiedCells.Contains(position))
                    continue;
                
                freeCells.Add(position);
            }
        }

        return freeCells;
    }

    public void FitToBounds()
    {
        float boundsWidth = RoomBounds.bounds.size.x;
        float boundsHeight = RoomBounds.bounds.size.y;
        float aspectRatio = Width / (float)Height;
        Debug.Log(boundsWidth);

        float scale = 1;
        
        if (boundsHeight * aspectRatio < boundsWidth)
        {
            scale = boundsHeight / Height;
        }
        else
        {
            scale = boundsWidth / Width;
        }

        transform.localScale = Vector3.one * scale;
    }
    
    public Vector2 GetCellPosition(int x, int y)
    {
        return new Vector2(x * 1 - (Width - 1) / 2f, y * 1 - (Height - 1) / 2f);
    }

    public void CreateCells()
    {
        bool isOdd = false;
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Cell cell = Instantiate(Cell, transform);
                cell.transform.localPosition = GetCellPosition(x, y);
                cell.SetColor(isOdd);

                isOdd = !isOdd;
            }
            
            isOdd = !isOdd;
        } 
    }
}
