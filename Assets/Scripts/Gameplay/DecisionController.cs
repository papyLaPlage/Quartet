﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class DecisionController : NetworkBehaviour {

	public GameController gameController;

	public Models.Situation situation;
	public Models.Decision decision;

    private MinisterController ministerAnswering;

    void Awake()
    {
        this.gameController = this.GetComponent<GameController>();
        UIManager = FindObjectOfType<GameUIManager>();
    }

    void Start()
    {
        FindObjectOfType<GameUIManager>().OnClick += OnAnswerClick;
    }

    #region UI

    GameUIManager UIManager;

	public void UpdateDecisionScreen(Models.Situation situation, Models.Decision decision) {

		// Update controller Situation & Decision values
		this.situation = situation;
		this.decision = decision;

		// Get the minister from Decision object
		Models.Ministers minister = decision.minister;

        UIManager.situtationText.text = situation.description;
        UIManager.decisionText.text = decision.description;
        UIManager.answerButtonsText[0].text = decision.answers[0].text;
        UIManager.answerButtonsText[1].text = decision.answers[1].text;

        // Update Sliders
        UIManager.sliders[0].value = this.gameController.paramMinister1Public;
        UIManager.sliders[1].value = this.gameController.paramMinister2Public;
        UIManager.sliders[2].value = this.gameController.paramMinister3Public;
        UIManager.sliders[3].value = this.gameController.paramMinister4Public;

		// Update Gauges 
		Vector2 v = new Vector2 (UIManager.gauges [0].GetComponent<RectTransform> ().sizeDelta.x, this.gameController.paramMinister1Public);
        UIManager.gauges[0].GetComponent<RectTransform>().sizeDelta = v;
		v.y = this.gameController.paramMinister2Public;
        UIManager.gauges[1].GetComponent<RectTransform>().sizeDelta = v;
		v.y = this.gameController.paramMinister3Public;
        UIManager.gauges[2].GetComponent<RectTransform>().sizeDelta = v;
		v.y = this.gameController.paramMinister4Public;
        UIManager.gauges[3].GetComponent<RectTransform>().sizeDelta = v;

        foreach(MinisterController ministerController in FindObjectsOfType<MinisterController>())
        {
            if (ministerController.roles.Contains(minister) && ministerController.isLocalPlayer)
            {
                ministerAnswering = ministerController;
                EnableUIElements(minister);
                return;
            }
        }
        //else if not the right localPlayer
	}

	public void EnableUIElements(Models.Ministers minister) {

        // Set current player Slider interactable
        UIManager.sliders [(int)minister].interactable = true;

		// Enable answers buttons
		for (int b = 0; b < UIManager.answerButtonsText.Length; b++ ) {
            UIManager.answerButtonsText [b].GetComponentInParent<Button> ().interactable = true;
		}
	}

	public void disableUIElements() {
		// Disable all sliders. Overkill but ... meh
		for (int s = 0; s < UIManager.sliders.Length; s++) {
            UIManager.sliders [s].interactable = false;
		}

		// Disable answers buttons
		for (int b = 0; b < UIManager.answerButtonsText.Length; b++ ) {
            UIManager.answerButtonsText [b].GetComponentInParent<Button> ().interactable = false;
		}
	}

	#endregion


	#region ACTIONS

	public void OnAnswerClick(int answerIndex) {
		Debug.Log ("Player chose answer index " + answerIndex);
		this.disableUIElements ();

        // We store the selected answer in the situation object;

        foreach(Models.Operation operation in decision.answers[answerIndex].operations)
        {
            switch (operation.minister)
            {
                case Models.Ministers.Communication:
                    ministerAnswering.paramMinister1Influence += operation.value;
                    break;
                case Models.Ministers.Security:
                    ministerAnswering.paramMinister2Influence += operation.value;
                    break;
                case Models.Ministers.Foreign:
                    ministerAnswering.paramMinister3Influence += operation.value;
                    break;
                case Models.Ministers.Financial:
                    ministerAnswering.paramMinister4Influence += operation.value;
                    break;
            }
        }

        //verify altruism
        if(gameController.GetParamMinisterByEnum(decision.answers[answerIndex].minister, true) <= gameController.GetParamMinisterByEnum(decision.answers[answerIndex == 0 ? 0 : 1].minister, true))
        {
            ministerAnswering.paramConfidenceInfluence += gameController.factorCoop;
        }
        else
        {
            ministerAnswering.paramConfidenceInfluence -= gameController.factorInstability;
        }

		ministerAnswering.ResumeDay();
	}

	#endregion
}