using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameController : NetworkBehaviour
{
	// Chaque Ministre a un paramètre interne et un paramètre "publique" qu'il peut modifier

    [SyncVar]
	public int paramMinister1;
    [SyncVar]
	public int paramMinister1Public;
    [SyncVar]
    public int paramMinister2;
    [SyncVar]
    public int paramMinister2Public;
    [SyncVar]
    public int paramMinister3;
    [SyncVar]
    public int paramMinister3Public;
    [SyncVar]
    public int paramMinister4;
    [SyncVar]
    public int paramMinister4Public;

    public int GetParamMinisterByEnum(Models.Ministers minister, bool trueParam)
    {
        switch (minister)
        {
            case Models.Ministers.Communication:
                return trueParam ? paramMinister1 : paramMinister1Public;
            case Models.Ministers.Security:
                return trueParam ? paramMinister2 : paramMinister2Public;
            case Models.Ministers.Foreign:
                return trueParam ? paramMinister3 : paramMinister3Public;
            case Models.Ministers.Financial:
                return trueParam ? paramMinister4 : paramMinister4Public;
        }
        return 0;
    }

	public float factorCoop;
    public float factorInstability;
    [SyncVar]
    public float paramConfidence;
	public MinisterController[] players;

    // Situations
    [SyncVar]
    public int daysLeft;
    public Queue<Models.Situation> situations;
    public List<Models.Situation> situationsDebug;

    /*public Queue<Models.Ministers>  ministers;*/
    public Models.Answer[] situationAnswers;

    private Models.Situation currentSituation;

	#region GAME 

	public void StartGame() {
        FindObjectOfType<GameUIManager>().testoText.text += " Start server ";
        RpcPlayNextSituation ();
	}

	public void EndGame() {
		// TODO : End Game Code 
	}

	#endregion

	#region SITUATIONS

	public void LoadGameSitutations() {
		Debug.Log ("Load Game Situations");
        this.situations = new Queue<Models.Situation>();
        foreach (Models.Situation situation in GetComponent<Parser>().Load7Situations()) {
			situations.Enqueue(situation);
            situationsDebug.Add(situation);
        }
	}

    [Command]
    public void CmdPlayNextSituation()
    {
        RpcPlayNextSituation();
    }
    [ClientRpc]
	private void RpcPlayNextSituation() {
        PlayNextSituation();
    }
    private void PlayNextSituation()
    {
        FindObjectOfType<GameUIManager>().testoText.text += " RpcPlayNextSituation ";
        if (this.situations.Count > 0)
        {
            Debug.Log("Play Situation");
            ProcessSituation(situations.Dequeue());
        }
        else
        {
            EndGame();
        }
    }

    // When all decisions have been played, we process the whole situation
	public void EndSituation() {
        // TODO : Process all answers

        foreach (MinisterController player in FindObjectsOfType<MinisterController>())
            player.SubmitChanges();

        PlayNextSituation();
	}

    [ClientRpc]
    public void RpcProcessSituation()
    {
        ProcessSituation();
    }

    private void ProcessSituation(Models.Situation situation) {
        currentSituation = situation;
        ProcessSituation();
    }
    private void ProcessSituation()
    {
        // If decisions to play are remaining in the queue, we display one of them
        if (currentSituation.decisions.Count > 0)
        {
            Models.Decision decision = currentSituation.decisions.Dequeue();
            GetComponentInParent<DecisionController>().UpdateDecisionScreen(currentSituation, decision);
        }
        else
        {
            EndSituation();
        }
    }

	#endregion

	#region UNITY

	public void Awake() {
        LoadGameSitutations();
    }

    void Start () {
        if (isServer)
        {
            daysLeft = 7;

            this.paramMinister1 = this.paramMinister1Public = 50;
            this.paramMinister2 = this.paramMinister2Public = 50;
            this.paramMinister3 = this.paramMinister3Public = 50;
            this.paramMinister4 = this.paramMinister4Public = 50;
            paramConfidence = 1;
        }
    }

    #endregion
}
