using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour {

	public Button startButton;

	public void OnStartButtonClick() {
		Debug.Log ("Start Game");
		SceneManager.LoadScene ("MatchMaking", LoadSceneMode.Single);
	}

	public void OnExitButtonClick() {
		Debug.Log ("Exit Game");
		Application.Quit();
	}

	public void OnCreditsButtonClick() {
		Debug.Log ("Show Credits");

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
