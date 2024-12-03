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
	public Transform BodyCellPrefab;
	public Room Room;
	public Vector2Int RoomPosition;
	public Direction MoveDirection;
	public float MoveDelay = 1.5f;

	private float _timer;
	private List<Transform> _body = new();

	public void Start()
	{
		TeleportTo(RoomPosition);
		AddInitialBodyCells();
	}

	public void AddInitialBodyCells()
	{
		Vector2Int startPosition = RoomPosition;
		for (int i = 0; i < 3; i++)
		{
			startPosition.x--;
			Vector2 spawnPoint = Room.GetCellPosition(startPosition.x, startPosition.y);
			Transform cell = SpawnBodyCell();

			cell.localPosition = spawnPoint;
			_body.Add(cell);
		}
	} 

	private Transform SpawnBodyCell()
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

		//TODO: Check input
		
		if (_timer > MoveDelay)
		{
			MoveSnakeForward();
			_timer = 0;
		}
	}

	private void MoveSnakeForward()
	{
		Vector2Int direction = GetForwardVector();

		Vector2Int oldPosition = RoomPosition;
		Vector2Int newPosition = oldPosition + direction;
		
		//TODO: Collision check
		
		Grow();
		MoveBody(oldPosition);
		RoomPosition = newPosition;
		TeleportTo(RoomPosition);
		FaceDirection(direction);
	}

	private void Grow()
	{
		Transform cell = SpawnBodyCell();
		_body.Add(cell);
	}

	private void MoveBody(Vector2Int oldHeadPosition)
	{
		Transform lastCell = _body.Last();
		lastCell.localPosition = Room.GetCellPosition(oldHeadPosition.x, oldHeadPosition.y);

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