using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SituationController : MonoBehaviour {

	public Image image;
	public Text situtationText;
	public Text decisionText;
	public Text[] answerButtonsText;
	public Slider[] sliders;
	public RawImage[] gauges;
	public GameController gameController;

	#region UI
	// update the situation canvas with a given situation for a given minister (player)
	public void ShowSituation(Models.Situation situation, Models.Ministers minister) {
		// Situation image and description
		this.situtationText.text = situation.description;

		// Update Image
		//this.situationViewController.image = situation.image;

		// Minister Decision description and answers
		Models.Decision ministerDecision = situation.decisions [(int) minister];
		this.decisionText.text = ministerDecision.description;
		this.answerButtonsText[0].text = ministerDecision.answers[0].text;
		this.answerButtonsText[1].text = ministerDecision.answers[1].text;

		Debug.Log ("Public Value for min 1 " + this.gameController.paramMinister1Public);

		// Update Sliders
		sliders[0].value = this.gameController.paramMinister1Public;
		sliders[1].value = this.gameController.paramMinister2Public;
		sliders[2].value = this.gameController.paramMinister3Public;
		sliders[3].value = this.gameController.paramMinister4Public;

		// Set current player Slider interactable
		sliders [(int)minister].interactable = true;

		// Update Gauges 
		Vector2 v = new Vector2 (gauges [0].GetComponent<RectTransform> ().sizeDelta.x, this.gameController.paramMinister1Public);
		gauges[0].GetComponent<RectTransform>().sizeDelta = v;
		v.y = this.gameController.paramMinister2Public;
		gauges[1].GetComponent<RectTransform>().sizeDelta = v;
		v.y = this.gameController.paramMinister3Public;
		gauges[2].GetComponent<RectTransform>().sizeDelta = v;
		v.y = this.gameController.paramMinister4Public;
		gauges[3].GetComponent<RectTransform>().sizeDelta = v;

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
