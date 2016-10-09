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

    public Button roleAcceptedButton;
    public Button situationAcceptedButton;
    public Text testoText;

    public delegate void IntEvent(int value);
    public IntEvent OnSlideInt;

    public delegate void BoolEvent(bool value);
    public BoolEvent OnClickBool;

    // Use this for initialization
    void Start () {
	
	}

    public void ExecuteOnSlideInt(int value)
    {
        if (OnSlideInt != null)
        {
            OnSlideInt(value);
        }
    }

    public void ExecuteOnClickBool(bool value)
    {
        if (OnClickBool != null)
        {
            OnClickBool(value);
        }
    }
}
