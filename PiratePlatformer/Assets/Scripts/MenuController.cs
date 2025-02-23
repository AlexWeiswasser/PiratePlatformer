using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	static float overallTimer = 0;
	static bool timerStarted = false;
	static TextMeshProUGUI timerText;

	void Awake()
	{
		DontDestroyOnLoad(this);
	}

	private void Update()
	{
		if(timerStarted)
		{
			overallTimer += Time.deltaTime;
			timerText.text = "Time: " + overallTimer;
		}
		else
		{
			if(timerText != null)
			{
				timerText.text = "Your final time is: " + overallTimer;
			}
		}
	}

	public static void toFirstLevel()
	{
		SceneManager.LoadScene(1);
		GameObject.FindObjectOfType<MenuController>().StartCoroutine(levelLoading());
		timerStarted = true;
	}
	public static void toSecondLevel()
	{
		SceneManager.LoadScene(2);
		GameObject.FindObjectOfType<MenuController>().StartCoroutine(levelLoading());
		timerStarted = true;
	}
	public static void toThirdLevel()
	{
		SceneManager.LoadScene(3);
		GameObject.FindObjectOfType<MenuController>().StartCoroutine(levelLoading());
		timerStarted = true;
	}
	public static void toFinal()
	{
		SceneManager.LoadScene(4);
		GameObject.FindObjectOfType<MenuController>().StartCoroutine(levelLoading());
		timerStarted = false;
		Cursor.visible = true;
	}
	public static void restart()
	{
		SceneManager.LoadScene(0);
		timerText = null;
		overallTimer = 0;
		timerStarted = false;
	}

	private static IEnumerator levelLoading()
	{
		yield return new WaitForSeconds(.1f);
		{
			timerText = null;
			timerText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
		}
	}
}
