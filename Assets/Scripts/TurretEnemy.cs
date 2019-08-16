using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class TurretEnemy : MonoBehaviour
{
	public float TargetDistance = 5;
	public float Cooldown = 3;
	public float CurrentCooldown = 0;
	bool CanShoot = true;

	public Projectile ProjectilePrefab;
	public Transform ProjectileSpawnPoint;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		RaycastHit2D hitInfo;

		hitInfo = Physics2D.Raycast(transform.position, Vector2.up, TargetDistance, 1 << LayerMask.NameToLayer("Player"));

		Debug.DrawRay(transform.position, Vector3.up * TargetDistance, hitInfo.collider ? Color.red : Color.blue);

		if(hitInfo.collider != null && CanShoot)
		{
			Shoot();
			StartCoroutine(PlayCoolDown());
		}

	}

	void Shoot()
	{
		Debug.Log("shoot");

		Projectile projectile = Instantiate<Projectile>(ProjectilePrefab);
		projectile.transform.position = ProjectileSpawnPoint.position;
		projectile.Direction = transform.up;
		//bullet.ve
	}

	IEnumerator PlayCoolDown()
	{
		CanShoot = false;
		yield return new WaitForSeconds(Cooldown);
		CanShoot = true;
		yield return null;
	}
}
