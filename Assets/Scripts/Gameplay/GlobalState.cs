using UnityEngine;
using System.Collections;

public class GlobalState : MonoBehaviour {

	public bool gameWin = false;
	public string endText;

	// Use this for initialization
	void Start () {
		Object.DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
