using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	private const float VERSION = .1f;
	public bool clearPrefs = false;
	
	private string _levelToLoad = "";
	private string _CharacterGeneration = "CharacterGenerator";
	private string _firstLevel = "Level1";
	
	private bool _hasCharacter = false;
	
	// Use this for initialization
	void Start () {
		if (clearPrefs)
			PlayerPrefs.DeleteAll();
		
		if (PlayerPrefs.HasKey("ver")) {
			if (PlayerPrefs.GetFloat("ver") != VERSION) {
				/* Upgrade playerprefs here */
			}
			else {
				if (PlayerPrefs.HasKey("Player Name")) {
					if (PlayerPrefs.GetString("Player Name") == "") {
						PlayerPrefs.DeleteAll();
						_levelToLoad = _CharacterGeneration;
					}
					else {
						_hasCharacter = true;
						//_levelToLoad = _firstLevel;
					}
				}
				else {
					PlayerPrefs.DeleteAll();
					PlayerPrefs.SetFloat("ver", VERSION);
					_levelToLoad = _CharacterGeneration;
				}
			}
		}
		else {
			Debug.Log("no ver key");
			//PlayerPrefs.DeleteAll();
			PlayerPrefs.SetFloat("ver", VERSION);
			_levelToLoad = _CharacterGeneration;
		}
			
	}
	
	// Update is called once per frame
	void Update () {
		if (_levelToLoad == "")
			return;
		
		Application.LoadLevel(_levelToLoad);
	}
	
	void OnGUI() {
		if (_hasCharacter) {
			if (GUI.Button(new Rect(10, 10, 110, 25), "Load Character")) {
				_levelToLoad = _firstLevel;
			}
			
			if (GUI.Button(new Rect(10, 40, 110, 25), "Delete Character")) {
				PlayerPrefs.DeleteAll();
				PlayerPrefs.SetFloat("ver", VERSION);
				_levelToLoad = _CharacterGeneration;
			}
		}
	}
}
