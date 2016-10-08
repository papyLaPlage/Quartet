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
		this.LoadGameSitutations ();
		this.LoadSituation ();
	}

	public void EndGame() {

	}

	public void ShowMinisterSituation(Models.Ministers minister) {
		
	}

	#endregion

	#region SITUATIONS
	public void LoadGameSitutations() {
		Debug.Log ("Load Game Situations");
		// TODO : coder en dur situations ou importer depuis XML
		Models.Situation s = new Models.Situation ();
		s.description = "Attentat à Paris";
		s.decisions = new Models.Decision[4];

		Models.Decision d = new Models.Decision ();
		d.description = "Riposte Militaire";
		d.minister = Models.Ministers.Security;
		d.answers = new Models.Answer[2];

			
		Models.Answer a1 = new Models.Answer ();
		a1.text = "Intervenir sur le territoire national";
		a1.minister = Models.Ministers.Foreign;

		Models.Answer a2 = new Models.Answer ();
		a2.text = "Intervenir sur le territoire des terroristes";
		a2.minister = Models.Ministers.Communication;

		d.answers [0] = a1;
		d.answers [1] = a2;
		s.decisions [(int) Models.Ministers.Security] = d;
		this.situations [0] = s;

	}

	public void LoadSituation() {
		Debug.Log ("Load Current Situation");
		if (this.currentSituationIndex >= this.situations.Length) {
			Debug.Log ("Game Over");	
			this.EndGame ();
			return;
		}

		Debug.Log ("Show situation");
		this.GetComponentInParent<SituationController>().ShowSituation (situations [this.currentSituationIndex], Models.Ministers.Security);

		this.currentSituationIndex++;
	}

	public void ProcessSituationResults() {
		
	}

	#endregion

	#region UNITY

	public void Awake() {
		this.situations = new Models.Situation[7];
		this.paramMinister1Public = 50;
		this.paramMinister2Public = 50;
		this.paramMinister3Public = 50;
		this.paramMinister4Public = 50;
	}

	public void Start () {
		this.StartGame ();
	}

	public void Update () {
	
	}

	#endregion
}
