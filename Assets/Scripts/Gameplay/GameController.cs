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
	public Situation[] situations;

	#region GAME 

	void startGame() {
		this.currentSituationIndex = 0;
	}

	void endGame() {

	}

	#endregion

	#region SITUATIONS
	void loadGameSitutations() {
		
	}

	void loadNextSituation() {
		this.currentSituationIndex++;
		if (this.currentSituationIndex >= this.situations.GetLength()) {
			this.endGame ();
		}



	}

	#endregion




	void Awake() {

	}

	void Start () {
	
	}

	void Update () {
	
	}
}
