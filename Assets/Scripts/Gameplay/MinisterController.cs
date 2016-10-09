using UnityEngine;
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

    public void SubmitConfidenceChanges()
    {
        if (isLocalPlayer)
            CmdUpdateConfidence(paramConfidenceInfluence);
        paramConfidenceInfluence = 0f;
    }

    [Command]
    void CmdUpdateConfidence(float modifConfidence)
    {
        _gameController.paramConfidence = Mathf.Clamp(_gameController.paramConfidence + modifConfidence, 1, 50);
        if(modifConfidence < 0)
        {
            if (_gameController.paramConfidenceLoss == 0)
                _gameController.paramConfidenceLoss -= _gameController.factorInstability1;
            else if (_gameController.paramConfidenceLoss == _gameController.factorInstability1)
                _gameController.paramConfidenceLoss -= _gameController.factorInstability2;
            else
                _gameController.paramConfidenceLoss -= _gameController.factorInstability3;
        }
        _gameController.paramConfidenceLoss += modifConfidence < 0 ? modifConfidence : 0;
    }

    public void SubmitParamsChanges()
    {
        if (isLocalPlayer)
            CmdUpdateParameters(paramMinister1Influence, paramMinister2Influence, paramMinister3Influence, paramMinister4Influence);
        paramMinister1Influence = paramMinister2Influence = paramMinister3Influence = paramMinister4Influence = 0;
    }

    [Command]
    void CmdUpdateParameters(int modif1, int modif2, int modif3, int modif4)
    {
        _gameController.paramMinister1 = Mathf.Clamp(_gameController.paramMinister1 + (int)(modif1 * (modif1 < 0 ? _gameController.paramConfidenceLoss : 1)), 0, 100);
        _gameController.paramMinister2 = Mathf.Clamp(_gameController.paramMinister2 + (int)(modif2 * (modif2 < 0 ? _gameController.paramConfidenceLoss : 1)), 0, 100);
        _gameController.paramMinister3 = Mathf.Clamp(_gameController.paramMinister3 + (int)(modif3 * (modif3 < 0 ? _gameController.paramConfidenceLoss : 1)), 0, 100);
        _gameController.paramMinister4 = Mathf.Clamp(_gameController.paramMinister4 + (int)(modif4 * (modif4 < 0 ? _gameController.paramConfidenceLoss : 1)), 0, 100);
        /*_gameController.paramMinister1Public = Mathf.Clamp(_gameController.paramMinister1Public + modif1, 0, 100);
        _gameController.paramMinister2Public = Mathf.Clamp(_gameController.paramMinister2Public + modif2, 0, 100);
        _gameController.paramMinister3Public = Mathf.Clamp(_gameController.paramMinister3Public + modif3, 0, 100);
        _gameController.paramMinister4Public = Mathf.Clamp(_gameController.paramMinister4Public + modif4, 0, 100); */
    }

    [Command]
    public void CmdCheatParameter(int gaugeIndex, int value)
    {
        switch (gaugeIndex)
        {
            case 0:
                _gameController.paramMinister1Public = value;
                break;
            case 1:
                _gameController.paramMinister2Public = value;
                break;
            case 2:
                _gameController.paramMinister3Public = value;
                break;
            case 3:
                _gameController.paramMinister4Public = value;
                break;
        }
    }

    #endregion
}
