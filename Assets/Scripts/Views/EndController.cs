using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EndController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GlobalState gs = GameObject.FindObjectOfType<GlobalState> ();
		Text endText = GameObject.Find ("End Text").GetComponent<Text>();

		GameObject winText = GameObject.Find ("End Title Victory");
		GameObject winImg = GameObject.Find ("End Image Victory");

		GameObject looseText = GameObject.Find ("End Title Defeat");
		GameObject looseImg = GameObject.Find ("End Title Image Defeat");

		endText.text = gs.endText;
		bool win = gs.gameWin;

		looseText.SetActive (!win);
		looseImg.SetActive (!win);
		winText.SetActive (win);
		winImg.SetActive (win);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
