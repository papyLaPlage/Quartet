using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DefaultTransition : MonoBehaviour {

    [Header("Normal")]
    public string transitionTarget;
    public RawImage blackScreen;
    public float displayTime;
    public float transitionTime;

    public bool canSkip;

    [Header("Bonus!")]
    public AudioSource jokeSound;
    public bool playJokeSound;

    float timer;
    float factor;

    // Use this for initialization
    void Start () {
        StartCoroutine(FadeInPhase());
	}

    IEnumerator FadeInPhase()
    {
        if (playJokeSound)
            jokeSound.Play();

        timer = transitionTime;
        factor = 1 / transitionTime;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            blackScreen.color = Color.Lerp(Color.clear, Color.black, timer * factor);
            VerifySkip();
            yield return null;
        }
        blackScreen.color = Color.clear;
        StartCoroutine(DisplayPhase());
    }

    IEnumerator DisplayPhase()
    {
        timer = displayTime;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            VerifySkip();
            yield return null;
        }
        StartCoroutine(FadeOutPhase());
    }

    IEnumerator FadeOutPhase()
    {
        timer = transitionTime;
        factor = 1 / transitionTime;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            blackScreen.color = Color.Lerp(Color.black, Color.clear, timer * factor);
            VerifySkip();
            yield return null;
        }
        blackScreen.color = Color.black;
        SceneManager.LoadScene(transitionTarget);
    }

    // Update is called once per frame
    void VerifySkip () {
        if (canSkip && Input.GetMouseButtonDown(0))
        {
            if(timer * factor > 0.5f)
                Color.Lerp(Color.black, Color.clear, 0.5f);
            SceneManager.LoadScene(transitionTarget);
        }
    }
}
