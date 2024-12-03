using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FruitSpawner : MonoBehaviour
{
    public Fruit Fruit;
    public Room Room;

    public void SpawnFruit()
    {
        Fruit fruit = Instantiate(Fruit, transform);
        UpdatePosition(fruit);
        
        Room.AddFruit(fruit);
    }

    public void UpdatePosition(Fruit fruit)
    {
        List<Vector2Int> freeCells = Room.GetFreeCells();

        Vector2Int position = freeCells[Random.Range(0, freeCells.Count)];
        Vector2 spawnPosition = Room.GetCellPosition(position.x, position.y);
        
        fruit.RoomPosition = position;
        fruit.transform.localPosition = spawnPosition;
    }
}
