using UnityEngine;
using System.Collections;
using Models;

public class SituationController : MonoBehaviour {

	GameController gameController;
	SituationViewController situationViewController;

	public SituationController(GameController gameController, SituationViewController svc) {
		this.gameController = gameController;
		this.situationViewController = svc;
	}

	// update the situation canvas with a given situation for a given minister
	public void showSituation(Models.Situation situation, Models.ministers minister) {
		// Situation image and description
		this.situationViewController.situtationText = situation.description;
		//this.situationViewController.image = situation.image;

		// Minister Decision description and answers
		Models.Decision ministerDecision = situation.decisions [minister];
		this.situationViewController.decisionText = ministerDecision.description;
		this.situationViewController.answerButtons[0] = ministerDecision.answers[0].text;
		this.situationViewController.answerButtons[1] = ministerDecision.answers[1].text;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
