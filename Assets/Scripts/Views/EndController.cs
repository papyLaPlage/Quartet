using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EndController : MonoBehaviour {

    public GameObject endCanvas;

    public Text endText;
    public GameObject winText;
    public GameObject winImg;

    public GameObject looseText;
    public GameObject looseImg;

    public bool keepShowing;

    // Use this for initialization
    void Start () {
		GlobalState gs = GameObject.FindObjectOfType<GlobalState> ();

        if (gs != null && gs.showEnd)
        {
            endCanvas.SetActive(true);

            endText.text = gs.endText;
            bool win = gs.gameWin;

            looseText.SetActive(!win);
            looseImg.SetActive(!win);
            winText.SetActive(win);
            winImg.SetActive(win);

            gs.showEnd = keepShowing;
        }
        else
            endCanvas.SetActive(false);
    }

    public void OnEndButtonClicked()
    {
        keepShowing = false;
        MyNetworkLobbyManager.singleton.GetComponent<MyNetworkLobbyManager>().OnDisconnectClicked();
    }
}                                   
