using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DecisionController : MonoBehaviour {

	public Image image;
	public Text situtationText;
	public Text decisionText;
	public Text[] answerButtonsText;
	public Slider[] sliders;
	public RawImage[] gauges;

	public GameController gameController;

	public Models.Situation situation;
	public Models.Decision decision;


	#region UI

	public void UpdateDecisionScreen(Models.Situation situation, Models.Decision decision) {

		// Update controller Situation & Decision values
		this.situation = situation;
		this.decision = decision;

		// Get the minister from Decision object
		Models.Ministers minister = decision.minister;

		this.situtationText.text = situation.description;
		this.decisionText.text = decision.description;
		this.answerButtonsText[0].text = decision.answers[0].text;
		this.answerButtonsText[1].text = decision.answers[1].text;

		// Update Sliders
		sliders[0].value = this.gameController.paramMinister1Public;
		sliders[1].value = this.gameController.paramMinister2Public;
		sliders[2].value = this.gameController.paramMinister3Public;
		sliders[3].value = this.gameController.paramMinister4Public;

		// Update Gauges 
		Vector2 v = new Vector2 (gauges [0].GetComponent<RectTransform> ().sizeDelta.x, this.gameController.paramMinister1Public);
		gauges[0].GetComponent<RectTransform>().sizeDelta = v;
		v.y = this.gameController.paramMinister2Public;
		gauges[1].GetComponent<RectTransform>().sizeDelta = v;
		v.y = this.gameController.paramMinister3Public;
		gauges[2].GetComponent<RectTransform>().sizeDelta = v;
		v.y = this.gameController.paramMinister4Public;
		gauges[3].GetComponent<RectTransform>().sizeDelta = v;

		this.enableUIElements (minister);
	}

	public void enableUIElements(Models.Ministers minister) {

		// Set current player Slider interactable
		sliders [(int)minister].interactable = true;

		// Enable answers buttons
		for (int b = 0; b < this.answerButtonsText.Length; b++ ) {
			this.answerButtonsText [b].GetComponentInParent<Button> ().interactable = true;
		}
	}

	public void disableUIElements() {
		// Disable all sliders. Overkill but ... meh
		for (int s = 0; s < this.sliders.Length; s++) {
			this.sliders [s].interactable = false;
		}

		// Disable answers buttons
		for (int b = 0; b < this.answerButtonsText.Length; b++ ) {
			this.answerButtonsText [b].GetComponentInParent<Button> ().interactable = false;
		}
	}

	#endregion


	#region ACTIONS

	public void OnAnswerClick(int answerIndex) {
		Debug.Log ("Player chose answer index " + answerIndex);
		this.disableUIElements ();

		// We store the selected answer in the situation object;
		this.situation.answerByMinister [(int) this.decision.minister] = this.decision.answers [answerIndex];
		Debug.Log (this.situation.answerByMinister [(int)this.decision.minister].text);
		this.gameController.ProcessSituation (situation);
	}

	#endregion

	#region UNITY
	void Awake() {
		this.gameController = this.GetComponentInParent<GameController> ();
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	#endregion
}
