using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public bool isMulti = false;

	// Chaque Ministre a un paramètre interne et un paramètre "publique" qu'il peut modifier

	public int paramMinister1;
	public int paramMinister1Public;
	public int paramMinister2;
	public int paramMinister2Public;
	public int paramMinister3;
	public int paramMinister3Public;
	public int paramMinister4;
	public int paramMinister4Public;

	public float factorCoop;
	public float factorInstability1;
	public float factorInstability2;
	public float paramConfidence;
	public MinisterController[] players;

	// Situations
	public Queue<Models.Situation> situations;	

	// Current Situtation
	public int currentSituationIndex;
	public Queue<Models.Ministers>  ministers;
	public Models.Answer[] situationAnswers;

	#region GAME 

	public void StartGame() {
		this.currentSituationIndex = 0;
		this.LoadGameSitutations ();
		this.PlayNextSituation ();
	}

	public void EndGame() {
		// TODO : End Game Code 
	}

	#endregion

	#region SITUATIONS

	public void LoadGameSitutations() {
		Debug.Log ("Load Game Situations");
		foreach (Models.Situation situation in GetComponent<Parser>().Load7Situations()) {
			situations.Enqueue(situation);
		}
	}


	public void PlayNextSituation() {

		if (this.situations.Count > 0 ) {
			Debug.Log ("Play Situation");
			this.ProcessSituation (this.situations.Dequeue ());
		} else {
			this.EndGame ();
		}
	}

	// When all decisions have been played, we process the whole situation
	public void EndSituation() {
		// TODO : Process all answers
		this.PlayNextSituation ();
	}

	public void ProcessSituation(Models.Situation situation) {
		if (this.isMulti) {
			this.ProcessSituationMulti (situation);
		} else {
			this.ProcessSituationSolo (situation);
		}
	}

	public void ProcessSituationSolo(Models.Situation situation) {
		// If decisions to play are remaining in the queue, we display one of them
		if (situation.decisions.Count > 0) {
			Models.Decision decision = situation.decisions.Dequeue();
			this.GetComponentInParent<DecisionController>().UpdateDecisionScreen (situation, decision);
		} else {
			this.EndSituation ();
		}
	}

	public void ProcessSituationAnswer(Models.Answer a) {
		
	}  

	public void ProcessSituationMulti(Models.Situation situation) {
		
	}


	public void ProcessAnswer() {

	}


	public void ProcessAnswerSolo() {
		
	}

	public void ProcessAnswerMulti() {
		
	}

	public bool HasAllAnswers() {
		// TODO : implement this
		return false;
	}

	#endregion

	#region UNITY

	public void Awake() {
		this.situations = new Queue<Models.Situation> ();
		this.paramMinister1Public = 50;
		this.paramMinister2Public = 50;
		this.paramMinister3Public = 50;
		this.paramMinister4Public = 50;

		this.ministers = new Queue<Models.Ministers> ();
		this.ministers.Enqueue (Models.Ministers.Communication);
		this.ministers.Enqueue (Models.Ministers.Foreign);
		this.ministers.Enqueue (Models.Ministers.Security);
		this.ministers.Enqueue (Models.Ministers.Financial);

		this.situations = new Queue<Models.Situation> ();
	}

	public void Start () {
		this.StartGame ();
	}

	public void Update () {
	
	}

	#endregion
}
