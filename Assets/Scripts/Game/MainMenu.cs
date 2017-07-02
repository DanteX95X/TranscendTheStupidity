using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game
{
	public class MainMenu : MonoBehaviour 
	{
		public void Play()
		{
			SceneManager.LoadScene("game");
		}

		public void Quit()
		{
			Application.Quit();
		}
	}
}