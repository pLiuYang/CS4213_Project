using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	//private const float GameSetting2.VERSION_NUMBER = .1f;
	public bool clearPrefs = false;
	
	private string _levelToLoad = "";
	private string _CharacterGeneration = GameSetting2.levelNames[1];
	private string _firstLevel = GameSetting2.levelNames[3];
	
	private bool _hasCharacter = false;
	
	private bool _exit = false;
	
	// Use this for initialization
	void Start () {
		if (clearPrefs)
			PlayerPrefs.DeleteAll();
		
		if (PlayerPrefs.HasKey(GameSetting2.VERSION_KEY_NAME)) {
			if (GameSetting2.LoadGameVersion() != GameSetting2.VERSION_NUMBER) {
				/* Upgrade playerprefs here */
				_levelToLoad = _CharacterGeneration;
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
					GameSetting2.SaveGameVersion();
					//PlayerPrefs.SetFloat("ver", GameSetting2.VERSION_NUMBER);
					_levelToLoad = _CharacterGeneration;
				}
			}
		}
		else {
			Debug.Log("no ver key");
			PlayerPrefs.DeleteAll();
			GameSetting2.SaveGameVersion();
			_levelToLoad = _CharacterGeneration;
		}
			
	}
	
	// Update is called once per frame
	void Update () {
		if (_exit) {
			Application.Quit();
			//Debug.Log("Cannot exit!!!" + _exit.ToString());
		}
		
		if (_levelToLoad == "")
			return;
		Application.LoadLevel(_levelToLoad);
	}
	
	void OnGUI() {
		if (_hasCharacter) {
			if (GUI.Button(new Rect(Screen.width*0.5f - 105, Screen.height-60, 110, 25), "Load Character")) {
				_levelToLoad = _firstLevel;
			}
			
			if (GUI.Button(new Rect(Screen.width*0.5f - 255, Screen.height-60, 110, 25), "New Game")) {
				PlayerPrefs.DeleteAll();
				GameSetting2.SaveGameVersion();
				_levelToLoad = _CharacterGeneration;
			}
		}
		
		if (GUI.Button(new Rect(Screen.width*0.5f + 45, Screen.height-60, 110, 25), "Exit")) {
			Application.Quit();
			_exit = true;
			Debug.Log("exit");
		}
	}
}
