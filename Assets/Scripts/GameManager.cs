using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static int level = 0;
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}


	public static void NextLevel()
	{
		string currentSceneName = SceneManager.GetActiveScene().name;
		level = int.Parse(currentSceneName);
		int nextLevel = ++level;
		string nextLevelName = string.Format("{0:000}", nextLevel);
		Debug.Log("nextLevelName " + nextLevelName);
		SceneManager.LoadScene(nextLevelName);
	}

	public static void RestartLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
