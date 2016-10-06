using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;

public class MyNetworkDiscovery : NetworkDiscovery {

    #region CALLBACKS

    NetworkLobbyManager lobby;
    string[] items;

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        Debug.Log("Client discovery received broadcast " + data + " from " + fromAddress);

        lobby = NetworkManager.singleton as NetworkLobbyManager;
        if (lobby.client == null)
        {
            items = fromAddress.Split(':');
            if (items.Length >= 4)
            {
                lobby.networkAddress = items[3];
                lobby.StartClient();
                Debug.Log("Connect please");
            }
        }
    }

    #endregion
}
