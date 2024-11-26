using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FruitSpawner : MonoBehaviour
{
    public GameObject Fruit;
    public Room Room;

    public void Start()
    {
        SpawnFruit();
    }

    public void SpawnFruit()
    {
        int width = Room.Width;
        int height = Room.Height;

        int x = Random.Range(0, width);
        int y = Random.Range(0, height);

        Vector2 spawnPosition = Room.GetCellPosition(x, y);

        GameObject fruit = Instantiate(Fruit, transform);
        fruit.transform.localPosition = spawnPosition;
    }
}
