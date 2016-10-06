using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyNetworkLobbyManager : NetworkLobbyManager {

    private NetworkDiscovery _networkDiscovery;

    void Start()
    {
        _networkDiscovery = GetComponentInChildren<NetworkDiscovery>();
        Debug.Log(networkAddress + " - " + networkPort);
    }


    #region BUTTONS

    public void OnStartHostClicked()
    {
        StartHost();
        _networkDiscovery.Initialize();
        _networkDiscovery.StartAsServer();
    }

    public void OnDisconnectClicked()
    {
        StopClient();
        StopHost();
        _networkDiscovery.StopBroadcast();  
    }

    public void OnStartDiscoveryClicked()
    {
        _networkDiscovery.Initialize();
        _networkDiscovery.StartAsClient();
    }

    public void OnStopDiscoveryClicked()
    {
        _networkDiscovery.StopBroadcast();
        StopClient();
    }

    public void OnPlayerReadyClicked(int ID)
    {
        playerLobbyPanels[ID].readyButton.gameObject.SetActive(false);
        lobbySlots[ID].SendReadyToBeginMessage();
    }

    #endregion


    #region UI MANAGEMENT

    [Header("UI"), SerializeField]
    private GameObject connectionPanel;
    [SerializeField]
    private GameObject roomPanel;
    [SerializeField]
    private GameObject discoveryPanel;
    [SerializeField]
    private Text debugText;

    [System.Serializable]
    private struct PlayerLobbyPanel
    {
        public GameObject gameObject;
        public Text infos;
        public Button readyButton;
    }
    [SerializeField]
    private PlayerLobbyPanel[] playerLobbyPanels;

    #endregion


    #region CALLBACKS

    public override void OnStartHost()
    {
        Debug.Log(networkAddress + " - " + networkPort);
    }

    public override void OnStopHost()
    {
        MasterServer.UnregisterHost();
        Network.Disconnect();
    }

    public override void OnLobbyStartClient(NetworkClient lobbyClient)
    {
        Debug.Log("OnStartClient");
        base.OnLobbyStartClient(lobbyClient);
    }
    public override void OnLobbyStopClient()
    {
        SetLobbyGUIActive(false);
        base.OnLobbyStopClient();
    }

    public override void OnLobbyClientEnter()
    {
        Debug.Log("OnLobbyClientEnter");
        connectionPanel.SetActive(false);
        discoveryPanel.SetActive(false);
        roomPanel.SetActive(true);
        SetLobbyGUIActive(true);
    }
    public override void OnLobbyClientConnect(NetworkConnection conn)
    {
        Debug.Log("OnLobbyClientConnect"); 
    }

    public override void OnLobbyServerPlayersReady()
    {
        roomPanel.SetActive(false);
        SetLobbyGUIActive(false);
    }

    bool isLobbyActive = false;
    public void SetLobbyGUIActive(bool state)
    {
        if (!isLobbyActive && state)
            StartCoroutine(_UpdateLobbyGUI());
        else if (isLobbyActive && !state)
            StopCoroutine(_UpdateLobbyGUI());
        isLobbyActive = state;
    }

    IEnumerator _UpdateLobbyGUI()
    {
        short i;
        for (i = 0; i < lobbySlots.Length; i++)
        {
            playerLobbyPanels[i].gameObject.SetActive(false);
        }

        float timer = 0.1f;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            if(timer <= 0f)
            {
                if (IsClientConnected())
                {
                    for (i = 0; i < lobbySlots.Length; i++)
                    {
                        if (lobbySlots[i] == null)
                        {
                            playerLobbyPanels[i].gameObject.SetActive(false);
                        }
                        else
                        {
                            playerLobbyPanels[i].gameObject.SetActive(true);
                            playerLobbyPanels[i].infos.text = lobbySlots[i].readyToBegin ? "ready" : "not ready";
                            playerLobbyPanels[i].readyButton.gameObject.SetActive(lobbySlots[i].isLocalPlayer && !lobbySlots[i].readyToBegin);
                        }
                    }
                    timer = 2f;
                }
                else
                    break;
            }
            yield return false;
        }
    }

    #endregion
}
