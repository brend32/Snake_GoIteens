using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public enum Direction
{
	Up,
	Down,
	Left,
	Right
}

public class SnakeController : MonoBehaviour
{
	public SnakeBodyCell BodyCellPrefab;
	public GameObject Mouth;
	public Room Room;
	public Vector2Int RoomPosition;
	public Direction MoveDirection;
	public float MoveDelay = 1.5f;

	private float _timer;
	private List<SnakeBodyCell> _body = new();

	public void Start()
	{
		TeleportTo(RoomPosition);
		AddInitialBodyCells();
	}

	public List<Vector2Int> GetOccupiedCells()
	{
		List<Vector2Int> occupiedCells = new List<Vector2Int>();
		occupiedCells.Add(RoomPosition);
		occupiedCells.AddRange(_body.Select(b => b.RoomPosition));

		return occupiedCells;
	}

	public void AddInitialBodyCells()
	{
		Vector2Int roomPosition = RoomPosition;
		for (int i = 0; i < 3; i++)
		{
			roomPosition.x--;
			Vector2 spawnPoint = Room.GetCellPosition(roomPosition.x, roomPosition.y);
			SnakeBodyCell cell = SpawnBodyCell();

			cell.RoomPosition = roomPosition; 
			cell.transform.localPosition = spawnPoint;
			_body.Add(cell);
		}
	} 

	private SnakeBodyCell SpawnBodyCell()
	{
		return Instantiate(BodyCellPrefab, transform.parent);
	}

	public void TeleportTo(Vector2Int roomPosition)
	{
		transform.localPosition = Room.GetCellPosition(roomPosition.x, roomPosition.y);
	}

	private void Update()
	{
		_timer += Time.deltaTime;

		ReadInput();

		if (_timer > MoveDelay)
		{
			MoveSnakeForward();
			_timer = 0;
		}
	}

	private void ReadInput()
	{
		if (MoveDirection != Direction.Down && Input.GetKeyDown(KeyCode.UpArrow))
		{
			MoveDirection = Direction.Up;
		}
		if (MoveDirection != Direction.Right && Input.GetKeyDown(KeyCode.LeftArrow))
		{
			MoveDirection = Direction.Left;
		}
		if (MoveDirection != Direction.Up && Input.GetKeyDown(KeyCode.DownArrow))
		{
			MoveDirection = Direction.Down;
		}
		if (MoveDirection != Direction.Left && Input.GetKeyDown(KeyCode.RightArrow))
		{
			MoveDirection = Direction.Right;
		}
	}

	private void MoveSnakeForward()
	{
		Vector2Int direction = GetForwardVector();

		Vector2Int oldPosition = RoomPosition;
		Vector2Int newPosition = oldPosition + direction;

		if (Room.CheckCollision(newPosition))
		{
			Debug.Log("Game over");
			return;
		}
		
		Mouth.SetActive(false);

		if (Room.TryGetFruitAtPosition(newPosition, out Fruit fruit))
		{
			Mouth.SetActive(true);	
			Grow();
			Room.RemoveAndSpawnNewFruit(fruit);
		}
		
		MoveBody(oldPosition);
		RoomPosition = newPosition;
		TeleportTo(RoomPosition);
		FaceDirection(direction);
	}

	private void Grow()
	{
		SnakeBodyCell cell = SpawnBodyCell();
		_body.Add(cell);
	}

	private void MoveBody(Vector2Int oldHeadPosition)
	{
		SnakeBodyCell lastCell = _body.Last();
		lastCell.RoomPosition = oldHeadPosition;
		lastCell.transform.localPosition = Room.GetCellPosition(oldHeadPosition.x, oldHeadPosition.y);

		_body.Remove(lastCell);
		_body.Insert(0, lastCell);
	}

	public void FaceDirection(Vector2Int direction)
	{
		transform.localRotation = Quaternion.FromToRotation(new Vector3(1, 0), new Vector3(direction.x, direction.y));
	}

	private Vector2Int GetForwardVector()
	{
		return MoveDirection switch
		{
			Direction.Down => new Vector2Int(0, -1),
			Direction.Up => new Vector2Int(0, 1),
			Direction.Right => new Vector2Int(1, 0),
			Direction.Left => new Vector2Int(-1, 0)
		};
	}
}