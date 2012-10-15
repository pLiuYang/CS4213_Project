using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		if (!CheckForMobPrefabs())
			return;
		if (!CheckForSpawnPoints())
			return;
		
		state = MobGenerator.State.Setup;
	}
	
	private void Setup() {
		state = MobGenerator.State.SpawnMob;
	}
	
	private void SpawnMob() {
		GameObject[] gos = AvailableSpawnPoints();
		
		for (int cnt = 0; cnt < gos.Length; cnt ++) {
			GameObject go = Instantiate(mobPrefabs[Random.Range(0, mobPrefabs.Length)],
										gos[cnt].transform.position,
										Quaternion.identity
										) as GameObject;
			go.transform.parent = gos[cnt].transform;
		}
		
		state = MobGenerator.State.Idle;
	}
	
	//check to see that we have at least one mob prefab to spawn
	private bool CheckForMobPrefabs() {
		if (mobPrefabs.Length > 0)
			return true;
		else 
			return false;
	}
	
	//check to see if we have at least one spawn point to spawn mobs
	private bool CheckForSpawnPoints() {
		if (spawnPoints.Length > 0)
			return true;
		else
			return false;
	}
	
	//generate a list of available spawn points that do not have any mobs childed to it
	private GameObject[] AvailableSpawnPoints() {
		List<GameObject> gos = new List<GameObject>();
		
		for (int cnt = 0; cnt < spawnPoints.Length; cnt++) {
			if (spawnPoints[cnt].transform.childCount == 0) {
				gos.Add(spawnPoints[cnt]);
			}
		}
		
		return gos.ToArray();
	}
}
