using UnityEngine;
using System.Collections;

public class CharacterGenerator : MonoBehaviour {
	private PlayerCharacter _toon;

	// Use this for initialization
	void Start () {
		_toon = new PlayerCharacter();
		_toon.Awake();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		GUI.Label(new Rect(10, 10, 50, 25), "Name:");
		_toon.Name = GUI.TextArea(new Rect(65, 10, 100, 25), _toon.Name);
	}
}
