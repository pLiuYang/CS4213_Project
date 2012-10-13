using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	private const float VERSION = .1f;
	
	// Use this for initialization
	void Start () {
		if (PlayerPrefs.HasKey("ver"))
			if (PlayerPrefs.GetFloat("ver") == VERSION) 
				if (PlayerPrefs.HasKey("Player Name"))
			{}
			
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
