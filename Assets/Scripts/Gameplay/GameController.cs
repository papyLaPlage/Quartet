using UnityEngine;
using System.Collections;

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

	public void StartGame() {
		this.currentSituationIndex = 0;
	}

	public void EndGame() {

	}

	public void ShowMinisterSituation(Models.Ministers minister) {
		
	}

	#endregion

	#region SITUATIONS
	public void LoadGameSitutations() {
		// TODO : coder en dur situations ou importer depuis XML
	}

	public void LoadSituation() {
		
		if (this.currentSituationIndex >= this.situations.Length) {
			this.EndGame ();
		}



		this.currentSituationIndex++;
	}

	#endregion




	public void Awake() {
		
	}

	public void Start () {
	
	}

	public void Update () {
	
	}
}
