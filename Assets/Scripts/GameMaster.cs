using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {
	public GameObject playerCharacter;
	public GameObject gameSettings;
	public Camera mainCamera;
	
	public float zOffset;
	public float yOffset;
	public float xRotOffset;
	
	private GameObject _pc;
//	private PlayerCharacter _pcScript;
	
	public Vector3 _playerSpawnPointPos;		//this is the place in 3d place where I want my player to spawm
	
	// Use this for initialization
	void Start () {
		//_playerSpawnPointPos = new Vector3(990, 91, 990);			//default pos for our player spawn point
		
		GameObject go = GameObject.Find(GameSettings.PLAYER_SPAWN_POINT);
		
		if (go == null) {
			Debug.LogWarning("Cannot find Player Spawn point");
			
			go = new GameObject(GameSettings.PLAYER_SPAWN_POINT);
			Debug.Log("Created Player Spawn point");
			
			go.transform.position = _playerSpawnPointPos;
			Debug.Log("Moved Player Spawn point");
		}	
			
		_pc = Instantiate(playerCharacter, go.transform.position, Quaternion.identity) as GameObject;
		_pc.name = "pc";
		//_pcScript = _pc.GetComponent<PlayerCharacter>();
		
		zOffset = -5.2f;
		yOffset = 3.7f;
		xRotOffset = 10.0f;
		
		mainCamera.transform.position = new Vector3(_pc.transform.position.x, _pc.transform.position.y + yOffset, _pc.transform.position.z + zOffset);
		mainCamera.transform.Rotate(xRotOffset, 0, 0);
		
		LoadCharacter();
	}
	
	public void LoadCharacter() {
		GameObject gs = GameObject.Find("__GameSettings");
		
		if (gs == null) {
			GameObject gs1 = Instantiate(gameSettings, Vector3.zero, Quaternion.identity) as GameObject;
			gs1.name = "__GameSettings";
		}
		
//		GameSettings gsScript = GameObject.Find("__GameSettings").GetComponent<GameSettings>();
			
		//load the character date
//		gsScript.LoadCharacterData();
			
	}
}
