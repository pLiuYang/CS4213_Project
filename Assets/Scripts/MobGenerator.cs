using UnityEngine;
using System.Collections;

public class MobGenerator : MonoBehaviour {
	public enum State {
		Idle,
		Initialize,
		Setup,
		SpawnMob
	}
	
	public GameObject[] mobPrefabs;					//an array to hold all of the prefabs of mobs we want to spawn
	public GameObject[] spawnPoints; 				//this array will hold a reference 
	
	public State state;								//this is our local var that holds our cur state
	
	void Awake() {
		state = MobGenerator.State.Initialize;
	}
	
	// Use this for initialization
	IEnumerator Start () {
		while (true) {
			switch(state) {
			case State.Initialize:
				Initialize();
				break;
			case State.Setup:
				Setup();
				break;
			case State.SpawnMob:
				SpawnMob();
				break;
			}
			
			yield return 0;
		}
	}
	
	private void Initialize() {
		state = MobGenerator.State.Setup;
	}
	
	private void Setup() {
		state = MobGenerator.State.SpawnMob;
	}
	
	private void SpawnMob() {
		state = MobGenerator.State.Idle;
	}
}
