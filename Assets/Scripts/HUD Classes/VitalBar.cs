/// <summary>
/// VitalBar.cs
/// Oct 9, 2012
/// Liu Yang
/// 
/// This class is responsible for displaying a vital for the player character or a mob
/// </summary>
/// 
using UnityEngine;
using System.Collections;

public class VitalBar : MonoBehaviour {
	public bool _isPlayerHealthBar;			//this boolean value tells us if this is the player health bar or mob health bar
	
	private int _maxBarLength;					//this is how large the vital bar can be if the target is at 100% status
	private int _curBarLength;					//this is the current length of the vital bar
	
	private GUITexture _display;
	
	void Awake() {
		_display = gameObject.GetComponent<GUITexture>();
	}
	
	// Use this for initialization
	void Start () {
		//_isPlayerHealthBar = true;
		
		_maxBarLength = (int)_display.pixelInset.width;
	
		OnEnable();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// This method is called when the gameobject is enabled
	public void OnEnable() {
		if (_isPlayerHealthBar) {
			Messenger<int, int>.AddListener("player health update", OnChangeHealthBarSize);
		} else {
			ToggleDisplay(false);
			Messenger<int, int>.AddListener("mob health update", OnChangeHealthBarSize);
			Messenger<bool>.AddListener("show mob vitalbars", ToggleDisplay);
		}
	}
	
	// This method is called when the gameobject is disabled
	public void OnDisable() {
		if (_isPlayerHealthBar) 
			Messenger<int, int>.RemoveListener("player health update", OnChangeHealthBarSize);
		else {
			Messenger<int, int>.RemoveListener("mob health update", OnChangeHealthBarSize);
			Messenger<bool>.RemoveListener("show mob vitalbars", ToggleDisplay);
		}
	}
	
	// this method will calculate the total size of the health bar in relation to the % of health the target has left
	public void OnChangeHealthBarSize(int curHealth, int maxHealth) {
		//Debug.Log("We heard an event");
		_curBarLength = (int) ((curHealth / (float)maxHealth) * _maxBarLength);		//this calculates the current var length based on the player's health %
		//_display.pixelInset = new Rect(_display.pixelInset.x, _display.pixelInset.y, _curBarLength, _display.pixelInset.height);
		_display.pixelInset = CalculatePosition();
	}
	
	// Setting the health bar to the player or mob
	public void SetPlayerHealth(bool b) {
		_isPlayerHealthBar = b;
	}
	
	private Rect CalculatePosition() {
		float yPos = _display.pixelInset.y / 2 - 10;
		
		if (!_isPlayerHealthBar) {
			float xPos = (_maxBarLength - _curBarLength) - (_maxBarLength / 4 + 10);
			return new Rect(xPos, yPos, _curBarLength, _display.pixelInset.height);
		}
		
		return new Rect(_display.pixelInset.x, yPos, _curBarLength, _display.pixelInset.height);
	}
	
	private void ToggleDisplay(bool show) {
		_display.enabled = show;
	}
}
