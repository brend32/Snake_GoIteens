
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
	public void Show()
	{
		gameObject.SetActive(true);
	}

	public void PlayAgain()
	{
		SceneManager.LoadScene(gameObject.scene.buildIndex);
	}
}