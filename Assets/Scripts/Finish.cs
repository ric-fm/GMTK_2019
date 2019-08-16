using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
	public float NextLevelTime = 2;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("Level Completed");
		StartCoroutine(NextLebelByTime());
	}

	IEnumerator NextLebelByTime()
	{
		yield return new WaitForSeconds(NextLevelTime);
		GameManager.NextLevel();
		yield return null;
	}
}
