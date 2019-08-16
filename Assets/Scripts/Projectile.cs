using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public Vector2 Direction;
	public float Speed = 10f;
	public float LifeTime = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(DestroyByTime());
	}

    // Update is called once per frame
    void Update()
    {
		Vector2 move = Direction * Speed * Time.deltaTime;
		transform.position += new Vector3(move.x, move.y, transform.position.z);
	}

	IEnumerator DestroyByTime()
	{
		yield return new WaitForSeconds(LifeTime);
		GameObject.Destroy(gameObject);
		yield return null;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("OnCollisionEnter2D");
		StopAllCoroutines();
		PlayerController player = collision.collider.GetComponent<PlayerController>();
		if (player != null)
			player.Die();
		GameObject.Destroy(gameObject);

	}
}
