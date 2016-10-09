﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class MinisterController : NetworkBehaviour {

    private GameController _gameController;

    public MinisterController[] players;

    IEnumerator Start()
    {
        while (true)
        {
            _gameController = FindObjectOfType<GameController>();
            if (_gameController != null)
                break;
            yield return new WaitForSeconds(0.5f);
        }
        while (true)
        {
            players = FindObjectsOfType<MinisterController>();
            if (players.Length >= MyNetworkLobbyManager.singleton.numPlayers)
                break;
            yield return new WaitForSeconds(0.5f);
        }
        if (isLocalPlayer && isServer)
        {
            RpcAssignRoles();
            _gameController.StartGame();
        }
    }

    #region INFLUENCE

    public List<Models.Ministers> roles = new List<Models.Ministers>();

    [ClientRpc]
    void RpcAssignRoles()
    {
        FindObjectOfType<GameUIManager>().testoText.text += " " + players.Length + " ";
        short i = 0, j = 0;
        while (i < 4)
        {
            AssignRoleToPlayer(players[j].gameObject, i);
            i++;
            j++;
            if (j >= players.Length)
                j = 0;
        }
    }

    void AssignRoleToPlayer(GameObject targetPlayer, int roleID)
    {
        targetPlayer.GetComponent<MinisterController>().roles.Add(Models.IntToMinister(roleID));
    }

    #endregion


    public void ResumeDay()
    {
        CmdResumeDay();
    }
    [Command]
    private void CmdResumeDay()
    {
        _gameController.RpcProcessSituation();
    }


    #region INFLUENCE

    public int paramMinister1Influence;
    public int paramMinister2Influence;
    public int paramMinister3Influence;
    public int paramMinister4Influence;

    public float paramConfidenceInfluence;

    public void SubmitChanges()
    {
        if (isLocalPlayer)
            CmdUpdateParameters(paramMinister1Influence, paramMinister2Influence, paramMinister3Influence, paramMinister4Influence, paramConfidenceInfluence);
        paramMinister1Influence = paramMinister2Influence = paramMinister3Influence = paramMinister4Influence = (int)(paramConfidenceInfluence = 0f);
    }

    [Command]
    void CmdUpdateParameters(int modif1, int modif2, int modif3, int modif4, float modifConfidence)
    {
        _gameController = FindObjectOfType<GameController>();
        _gameController.paramMinister1 = Mathf.Clamp(_gameController.paramMinister1 + modif1, 0, 100);
        _gameController.paramMinister2 = Mathf.Clamp(_gameController.paramMinister2 + modif2, 0, 100);
        _gameController.paramMinister3 = Mathf.Clamp(_gameController.paramMinister3 + modif3, 0, 100);
        _gameController.paramMinister4 = Mathf.Clamp(_gameController.paramMinister4 + modif4, 0, 100);
        _gameController.paramMinister1Public = Mathf.Clamp(_gameController.paramMinister1Public + modif1, 0, 100);
        _gameController.paramMinister2Public = Mathf.Clamp(_gameController.paramMinister2Public + modif2, 0, 100);
        _gameController.paramMinister3Public = Mathf.Clamp(_gameController.paramMinister3Public + modif3, 0, 100);
        _gameController.paramMinister4Public = Mathf.Clamp(_gameController.paramMinister4Public + modif4, 0, 100);

        _gameController.paramConfidence = Mathf.Clamp(_gameController.paramConfidence + modifConfidence, 1, 50);
    }

    #endregion
}
