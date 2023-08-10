using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

	public void PlayGame() {
		SceneManager.LoadScene("flatWorld");
	}
	
	public void Quit() {
		Application.Quit();
	}

}
