using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SituationController : MonoBehaviour {

	public Image image;
	public Text situtationText;
	public Text decisionText;
	public Text[] answerButtonsText;

	// update the situation canvas with a given situation for a given minister
	public void ShowSituation(Models.Situation situation, Models.Ministers minister) {
		// Situation image and description
		this.situtationText.text = situation.description;
		//this.situationViewController.image = situation.image;

		// Minister Decision description and answers
		Models.Decision ministerDecision = situation.decisions [(int) minister];
		this.decisionText.text = ministerDecision.description;
		this.answerButtonsText[0].text = ministerDecision.answers[0].text;
		this.answerButtonsText[1].text = ministerDecision.answers[1].text;
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
