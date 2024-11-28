using System;
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
	public Room Room;
	public Vector2Int RoomPosition;
	public Direction MoveDirection;

	private float _timer;

	public void Start()
	{
		TeleportTo(RoomPosition);
	}

	public void TeleportTo(Vector2Int roomPosition)
	{
		transform.localPosition = Room.GetCellPosition(roomPosition.x, roomPosition.y);
	}

	private void Update()
	{
		_timer += Time.deltaTime;

		if (_timer > 2.5f)
		{
			MoveSnakeForward();
			_timer = 0;
		}
	}

	private void MoveSnakeForward()
	{
		Vector2Int direction = GetForwardVector();

		Vector2Int newPosition = RoomPosition + direction;
		
		//TODO: Collision check
		
		RoomPosition = newPosition;
		TeleportTo(RoomPosition);
		FaceDirection(direction);
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