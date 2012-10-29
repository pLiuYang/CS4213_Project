using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(BoxCollider))]
[RequireComponent (typeof(AudioSource))]
public class Chest : MonoBehaviour {
	public enum State {
		open,
		close,
		inbetween
	}
	
	public AudioClip openSound;
	public AudioClip closeSound;
	
	public GameObject particleEffect;
	
	public GameObject[] parts;
	private Color[] _defaultColors;
	
	public State state;
	
	public float maxDistance = 3;
	
	private GameObject _player;
	private Transform _myTransform;
	public bool inUse = false;
	private bool _used = false;
	
	public List<Item> loot = new List<Item>();
	
	public static float defaultLifeTimer = 120;
	private float _lifeTimer = 0;
	
	// Use this for initialization
	void Start () {
		_myTransform = transform;
		
		state = Chest.State.close;
		
		particleEffect.active = false;
		
		_defaultColors = new Color[parts.Length];
		
		if (parts.Length > 0)
			for (int i = 0; i < _defaultColors.Length; i++)
				_defaultColors[i] = parts[i].renderer.material.GetColor("_Color");
	}
	
	// Update is called once per frame
	void Update () {
		_lifeTimer += Time.deltaTime;
		
		if (_lifeTimer > defaultLifeTimer && state == Chest.State.close)
			Destroy(gameObject);
		
		if (!inUse)
			return;
		
		if (_player == null)
			return;
		
		if (Vector3.Distance(transform.position, _player.transform.position) > maxDistance)// && !inUse)
			myGUI.chest.ForceClose();
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
		
		if (Vector3.Distance(_myTransform.position, go.transform.position) > maxDistance && !inUse)
			return;
		
		switch (state) {
		case State.open:
			state = Chest.State.inbetween;
			//StartCoroutine("Close");
			ForceClose();
			break;
		case State.close:
			if (myGUI.chest != null) 
				myGUI.chest.ForceClose();
			
			state = Chest.State.inbetween;
			StartCoroutine("Open");
			break;
		}
	}
	
	private IEnumerator Open() {
		myGUI.chest = this;
		
		_player = GameObject.FindGameObjectWithTag("Player");
		inUse = true;
		
		animation.Play ("open");
		particleEffect.active = true;
		audio.PlayOneShot(openSound);
		
		if (!_used)
			PopulateChest(5);
		
		yield return new WaitForSeconds(animation["open"].length);
		
		state = Chest.State.open;
		//Messenger<int>.Broadcast("PopulateChest", 5, MessengerMode.DONT_REQUIRE_LISTENER);
		Messenger.Broadcast("DisplayLoot");
	}
	
	private IEnumerator Close() {
		_player = null;
		inUse = false;
		
		animation.Play("close");
		particleEffect.active = false;
		audio.PlayOneShot(closeSound);
		
		float tempTimer = animation["close"].length;
		
		if (closeSound.length > tempTimer)
			tempTimer = closeSound.length;
		
		yield return new WaitForSeconds(animation["close"].length);
		
		state = Chest.State.close;
		
		if (loot.Count == 0)
			Destroy(gameObject);
	}
	
	private void DestoryChest() {
		loot = null;
		Destroy(gameObject);
	}
	
	public void ForceClose() {
		Messenger.Broadcast("CloseChest");
		
		StopCoroutine("Open");
		StartCoroutine("Close");
	}
	
	private void PopulateChest(int x) {
		for (int cnt = 0; cnt < x; cnt++) {
			loot.Add(ItemGenerator.CreateItem());
		}
		
		_used = true;
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
