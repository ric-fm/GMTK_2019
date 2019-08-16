using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTile : MonoBehaviour
{
	public enum TileDirection
	{
		Up = 1 << 0,
		Right = 1 << 1,
		Down = 1 << 2,
		Left = 1 << 3,
	}


	//public enum Direction
	//{
	//	All = 0,
	//	Down = 1,
	//	Left = 2,
	//	Right = 3,
	//	Up = 4,
	//}
	public Sprite[] sprites;
	public SpriteRenderer[] spriteRenderers;
	//public Direction direction;
	public bool ControlledByPlayer;

	public Color ControlledByPlayerColor;
	public Color NotControlledByPlayerColor;

	[EnumFlag]
	public TileDirection Direction = TileDirection.Up | TileDirection.Right | TileDirection.Down | TileDirection.Down;

	void UpadteDirection(int index, bool visible, Color color)
	{
		SpriteRenderer sr = spriteRenderers[index];
		sr.enabled = visible;
		sr.color = color;
	}

	void UpdateDirection()
	{
		//SpriteRenderer sr = GetComponent<SpriteRenderer>();

		Color color = ControlledByPlayer ? ControlledByPlayerColor : NotControlledByPlayerColor;

		bool up = CanGo(TileDirection.Up);
		UpadteDirection(0, CanGo(TileDirection.Up), color);
		UpadteDirection(1, CanGo(TileDirection.Right), color);
		UpadteDirection(2, CanGo(TileDirection.Down), color);
		UpadteDirection(3, CanGo(TileDirection.Left), color);
		//if (CanGo(FEnum.Up))
		//{
		//	sr = spriteRenderers[0];
		//	sr.sprite = sprites[0];
		//	sr.color = ControlledByPlayer ? ControlledByPlayerColor : NotControlledByPlayerColor;
		//}

		//int spriteIndex = (int)TestFlags;
		//if (spriteIndex == 0)
		//{
		//	sr.sprite = null;
		//}
		//else
		//{
		//	if (spriteIndex == -1)
		//		spriteIndex = 0;
		//	Debug.Log("Sprite index: " + spriteIndex);
		//	sr.sprite = sprites[spriteIndex];
		//	sr.color = ControlledByPlayer ? ControlledByPlayerColor : NotControlledByPlayerColor;
		//}
	}

	//public bool CanGo(Direction targetDirection)
	//{
	//	return direction == Direction.All || targetDirection == direction;
	//}

	public bool CanGo(TileDirection targetDirection)
	{
		return (Direction & targetDirection) == targetDirection;
	}

	private void OnValidate()
	{
		UpdateDirection();
	}
}
