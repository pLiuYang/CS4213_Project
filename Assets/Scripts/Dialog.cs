using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(BoxCollider))]
[RequireComponent (typeof(AudioSource))]
public class Dialog : MonoBehaviour {
	public enum State {
		open,
		close,
		inbetween
	}
	
	public AudioClip openSound;
	public AudioClip closeSound;
	
	public GameObject[] parts;
	private Color[] _defaultColors;
	
	public State state;
	
	public float maxDistance = 4;
	
	private GameObject _player;
	private Transform _myTransform;
	private bool _used = false;
	
	public string sentence;
	
	// Use this for initialization
	void Start () {
		_myTransform = transform;
		
		state = Dialog.State.close;
		
		_defaultColors = new Color[parts.Length];
		
		if (parts.Length > 0)
			for (int i = 0; i < _defaultColors.Length; i++)
				_defaultColors[i] = parts[i].renderer.material.GetColor("_Color");
	}
	
	// Update is called once per frame
	void Update () {
		
		if (_player == null)
			return;
		
		if (Vector3.Distance(transform.position, _player.transform.position) > maxDistance)// && !inUse)
			myGUI.dialog.ForceClose();
			//Messenger.Broadcast("CloseChest");
	}
	
	public void OnMouseEnter() {
		//Debug.Log("Enter");
		HightLight(true);
	}
	
	public void OnMouseExit() {
		//Debug.Log("exit");
		HightLight(false);
	}
	
	public void OnMouseUp() {
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		
		if (go == null)
			return;
		
		if (Vector3.Distance(_myTransform.position, go.transform.position) > maxDistance)
			return;
		
		switch (state) {
		case State.open:
			state = Dialog.State.inbetween;
			//StartCoroutine("Close");
			ForceClose();
			break;
		case State.close:
			if (myGUI.dialog != null) 
				myGUI.dialog.ForceClose();
			
			state = Dialog.State.inbetween;
			StartCoroutine("Open");
			break;
		}
	}
	
	private IEnumerator Open() {
		myGUI.dialog = this;
		
		_player = GameObject.FindGameObjectWithTag("Player");
		
		Messenger.Broadcast("DisplayDialog");
		//animation.Play ("open");
		
		audio.PlayOneShot(openSound);
		
		yield return new WaitForSeconds(animation["open"].length);
		
		state = Dialog.State.open;
		//Messenger<int>.Broadcast("PopulateChest", 5, MessengerMode.DONT_REQUIRE_LISTENER);
		
	}
	
	private IEnumerator Close() {
		_player = null;
		
		//animation.Play("close");
		
		audio.PlayOneShot(closeSound);
		
		state = Dialog.State.close;
		
		float tempTimer = animation["close"].length;
		
		if (closeSound.length > tempTimer)
			tempTimer = closeSound.length;
		
		yield return new WaitForSeconds(animation["close"].length);
		
		
	}
	
	public void ForceClose() {
		Messenger.Broadcast("CloseChest");
		
		StopCoroutine("Open");
		StartCoroutine("Close");
	}
	
	private void HightLight(bool glow) {
		if (glow) {
			if (parts.Length > 0)
				for (int i = 0; i < _defaultColors.Length; i++)
					parts[i].renderer.material.SetColor("_Color", Color.yellow);
		} else {
			if (parts.Length > 0)
				for (int i = 0; i < _defaultColors.Length; i++)
					parts[i].renderer.material.SetColor("_Color", _defaultColors[i]);
		}
	}
}
