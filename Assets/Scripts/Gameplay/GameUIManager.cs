using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour {

    public Image image;
    public Text situtationText;
    public Text decisionText;
    public Text[] answerButtonsText;
    public Slider[] sliders;
    public RawImage[] gauges;

    public Text testoText;

    public delegate void ClickEvent(int value);
    public ClickEvent OnClick;

    // Use this for initialization
    void Start () {
	
	}

    public void ExecuteOnClick(int value)
    {
        if (OnClick != null)
        {
            OnClick(value);
        }
    }
}
