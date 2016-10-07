using UnityEngine;
using System.Collections;
using Models;

public class GameController : MonoBehaviour {

	// Chaque Ministre a un paramètre interne et un paramètre "public" qu'il peut modifier
	public int paramMinister1;
	public int paramMinister1Public;
	public int paramMinister2;
	public int paramMinister2Public;
	public int paramMinister3;
	public int paramMinister3Public;
	public int paramMinister4;
	public int paramMinister4Public;

	public int paramCoop;
	public int factorConfidence;

	// 
	public MinisterController[] players;

	public int currentSituationIndex;
	public SituationController situationController;
	public Models.Situation[] situations;

	#region GAME 

	public void startGame() {
		this.currentSituationIndex = 0;
	}

	public void endGame() {

	}

	public void showMinisterSituation(Models.ministers minister) {
		
	}

	#endregion

	#region SITUATIONS
	public void loadGameSitutations() {
		// TODO : coder en dur situations ou importer depuis XML
	}

	public void loadSituation() {
		
		if (this.currentSituationIndex >= this.situations.Length) {
			return this.endGame ();
		}



		this.currentSituationIndex++;
	}

	#endregion




	public void Awake() {
		this.situationController = new SituationController (this);
	}

	public void Start () {
	
	}

	public void Update () {
	
	}
}
