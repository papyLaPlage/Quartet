using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MyNetworkLobbyPlayer : NetworkLobbyPlayer
{

    /*public override void OnClientEnterLobby()
    {
        if (isLocalPlayer)
            FindObjectOfType<MyNetworkLobbyManager>().UpdateLobbyGUI();
    }

    public override void OnClientReady(bool readyState)
    {
        if (isLocalPlayer)
            FindObjectOfType<MyNetworkLobbyManager>().UpdateLobbyGUI();
    }

    public override void OnClientExitLobby()
    {
        if (isLocalPlayer)
            FindObjectOfType<MyNetworkLobbyManager>().UpdateLobbyGUI();
    }*/
}
