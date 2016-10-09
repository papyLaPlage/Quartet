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

    public delegate void UIEvent(int value);
    public UIEvent OnClick;
    public UIEvent OnSlide;

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

    public void ExecuteOnSlide(int value)
    {
        if (OnSlide != null)
        {
            OnSlide(value);
        }
    }
}
