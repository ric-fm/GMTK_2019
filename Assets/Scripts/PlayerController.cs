using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//[ExecuteInEditMode]
public class PlayerController : MonoBehaviour
{
	public enum MoveDirection
	{
		None = 0,
		Down = 1,
		Left = 2,
		Right = 3,
		Up = 4
	}
	bool onTile = false;

	Vector2 velocity = Vector2.zero;

	public float speed = 10f;
	public float checkDistance = 1.0f;

	public LayerMask HitMask;

	public ParticleSystem Particles;

	public bool GodMode = false;

	bool isAlive = true;

	struct CollisionInfo
	{
		public Wall Up;
		public Wall Right;
		public Wall Down;
		public Wall Left;
		public MyTile Center;

		public void Clear()
		{
			Up = Right = Down = Left = null;
			Center = null;
		}
	}

	CollisionInfo lastCollisions;
	CollisionInfo collisions;

	// Start is called before the first frame update
	void Start()
	{

	}

	bool CanGo(MoveDirection direction)
	{
		bool result = false;

		switch (direction)
		{
			case MoveDirection.Up:
				result = !collisions.Up && (collisions.Down || collisions.Left || collisions.Right) ||
					(collisions.Center && collisions.Center.ControlledByPlayer && collisions.Center.CanGo(MyTile.TileDirection.Up));
				break;
			case MoveDirection.Right:
				result = !collisions.Right && (collisions.Left || collisions.Up || collisions.Down) ||
					(collisions.Center && collisions.Center.ControlledByPlayer && collisions.Center.CanGo(MyTile.TileDirection.Right));
				break;
			case MoveDirection.Down:
				result = !collisions.Down && (collisions.Up || collisions.Left || collisions.Right) ||
					(collisions.Center && collisions.Center.ControlledByPlayer && collisions.Center.CanGo(MyTile.TileDirection.Down));
				break;
			case MoveDirection.Left:
				result = !collisions.Left && (collisions.Right || collisions.Up || collisions.Down) ||
					(collisions.Center && collisions.Center.ControlledByPlayer && collisions.Center.CanGo(MyTile.TileDirection.Left));
				break;
		}

		return result;
	}

	bool CanGoOnWall(MoveDirection direction)
	{
		bool result = false;

		switch (direction)
		{
			case MoveDirection.Up:
				result = !collisions.Up && (collisions.Down/* || collisions.Left || collisions.Right*/);
				break;
			case MoveDirection.Right:
				result = !collisions.Right && (collisions.Left/* || collisions.Up || collisions.Down*/);
				break;
			case MoveDirection.Down:
				result = !collisions.Down && (collisions.Up/* || collisions.Left || collisions.Right*/);
				break;
			case MoveDirection.Left:
				result = !collisions.Left && (collisions.Right/* || collisions.Up || collisions.Down*/);
				break;
		}

		return result;
	}

	bool CanGoOnTile(MoveDirection direction)
	{
		bool result = false;

		switch (direction)
		{
			case MoveDirection.Up:
				result = (collisions.Center && collisions.Center.ControlledByPlayer && collisions.Center.CanGo(MyTile.TileDirection.Up));
				break;
			case MoveDirection.Right:
				result = (collisions.Center && collisions.Center.ControlledByPlayer && collisions.Center.CanGo(MyTile.TileDirection.Right));
				break;
			case MoveDirection.Down:
				result = (collisions.Center && collisions.Center.ControlledByPlayer && collisions.Center.CanGo(MyTile.TileDirection.Down));
				break;
			case MoveDirection.Left:
				result = (collisions.Center && collisions.Center.ControlledByPlayer && collisions.Center.CanGo(MyTile.TileDirection.Left));
				break;
		}

		return result;
	}

	void Move(MoveDirection direction)
	{
		switch(direction)
		{
			case MoveDirection.Up:
				velocity.x = 0;
				velocity.y = speed;
				break;
			case MoveDirection.Right:
				velocity.x = speed;
				velocity.y = 0;
				break;
			case MoveDirection.Down:
				velocity.x = 0;
				velocity.y = -speed;
				break;
			case MoveDirection.Left:
				velocity.x = -speed;
				velocity.y = 0;
				break;
		}
	}

	void TryMoveOn(MoveDirection direction)
	{
		if (CanGoOnTile(direction))
		{
			transform.position = collisions.Center.transform.position;
			Move(direction);
		}
		else if (CanGoOnWall(direction))
		{
			SnapTransform();
			Move(direction);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.R))
		{
			Restart();
		}

		if(!isAlive)
		{
			return;
		}

		collisions = GetCollisions();
		DebugCollisions(collisions);

		CheckCollisions(collisions);

		if (Input.GetKeyDown(KeyCode.W))
		{
			TryMoveOn(MoveDirection.Up);
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			TryMoveOn(MoveDirection.Right);
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			TryMoveOn(MoveDirection.Down);
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			TryMoveOn(MoveDirection.Left);
		}

		Vector2 move = velocity * Time.deltaTime;
		transform.position += new Vector3(move.x, move.y, transform.position.z);


		lastCollisions = collisions;
	}

	void SnapTransform()
	{
		transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), transform.position.z);
	}

	void SnapTo(Vector2 position)
	{
		transform.position = new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), transform.position.z);
	}


	CollisionInfo GetCollisions()
	{
		CollisionInfo result = new CollisionInfo();

		RaycastHit2D hitInfo;

		hitInfo = Physics2D.Raycast(transform.position, Vector2.up, checkDistance, 1 << LayerMask.NameToLayer("Hit"));
		if (hitInfo.collider != null)
			result.Up = hitInfo.collider.transform.GetComponent<Wall>();

		hitInfo = Physics2D.Raycast(transform.position, Vector2.right, checkDistance, 1 << LayerMask.NameToLayer("Hit"));
		if (hitInfo.collider != null)
			result.Right = hitInfo.collider.transform.GetComponent<Wall>();

		hitInfo = Physics2D.Raycast(transform.position, Vector2.down, checkDistance, 1 << LayerMask.NameToLayer("Hit"));
		if (hitInfo.collider != null)
			result.Down = hitInfo.collider.transform.GetComponent<Wall>();

		hitInfo = Physics2D.Raycast(transform.position, Vector2.left, checkDistance, 1 << LayerMask.NameToLayer("Hit"));
		if (hitInfo.collider != null)
			result.Left = hitInfo.collider.transform.GetComponent<Wall>();

		Collider2D centerCollider = Physics2D.OverlapCircle(transform.position, 0.25f, 1 << LayerMask.NameToLayer("Stand"));
		if (centerCollider != null)
		{
			result.Center = centerCollider.transform.GetComponent<MyTile>();
		}

		return result;
	}

	void CheckCollisions(CollisionInfo collisions)
	{
		Wall wall = null;

		if (velocity.x > 0 && collisions.Right)
		{
			wall = collisions.Right;
		}
		else if (velocity.x < 0 && collisions.Left)
		{
			wall = collisions.Left;
		}
		else if (velocity.y > 0 && collisions.Up)
		{
			wall = collisions.Up;
		}
		else if (velocity.y < 0 && collisions.Down)
		{
			wall = collisions.Down;
		}

		if (collisions.Center && lastCollisions.Center != collisions.Center)
		{
			if (!collisions.Center.ControlledByPlayer)
			{
				switch (collisions.Center.Direction)
				{
					case MyTile.TileDirection.Up:
						transform.position = collisions.Center.transform.position;
						Move(MoveDirection.Up);
						break;
					case MyTile.TileDirection.Right:
						transform.position = collisions.Center.transform.position;
						Move(MoveDirection.Right);
						break;
					case MyTile.TileDirection.Down:
						transform.position = collisions.Center.transform.position;
						Move(MoveDirection.Down);
						break;
					case MyTile.TileDirection.Left:
						transform.position = collisions.Center.transform.position;
						Move(MoveDirection.Left);
						break;
				}
			}
		}
		else if (wall)
		{
			velocity.x = 0;
			velocity.y = 0;
			SnapTransform();
			switch (wall.type)
			{
				case Wall.Type.Normal:
					break;
				case Wall.Type.Damage:
					Die();
					break;
			}
		}
	}

	void DebugCollisions(CollisionInfo collisions)
	{
		Debug.DrawRay(transform.position, Vector3.up * checkDistance, collisions.Up ? Color.red : Color.blue);
		Debug.DrawRay(transform.position, Vector3.right * checkDistance, collisions.Right ? Color.red : Color.blue);
		Debug.DrawRay(transform.position, Vector3.down * checkDistance, collisions.Down ? Color.red : Color.blue);
		Debug.DrawRay(transform.position, Vector3.left * checkDistance, collisions.Left ? Color.red : Color.blue);

	}

	private void OnDrawGizmos()
	{
		Gizmos.color = collisions.Center ? Color.red : Color.blue;
		Gizmos.DrawWireSphere(transform.position, 0.25f);
	}

	public void Die()
	{
		Particles.Play();

		GetComponent<SpriteRenderer>().enabled = false;
		velocity.x = 0;
		velocity.y = 0;
		isAlive = false;
	}

	public void Restart()
	{
		GameManager.RestartLevel();
	}
}
