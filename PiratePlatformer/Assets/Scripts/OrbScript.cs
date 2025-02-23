using UnityEngine;
using UnityEngine.SceneManagement;

public class OrbScript : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			if (SceneManager.GetActiveScene().buildIndex == 1)
			{
				MenuController.toSecondLevel();
			}
			else if (SceneManager.GetActiveScene().buildIndex == 2)
			{
				MenuController.toThirdLevel();
			}
			else if (SceneManager.GetActiveScene().buildIndex == 3)
			{
				MenuController.toFinal();
			}
		}
	}
}
