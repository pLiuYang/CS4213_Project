using UnityEngine;
using System.Collections;

public class PlayerModelCustomization : MonoBehaviour {
	public string[] maleModels;
	public float rotationSpeed = 100;
	
	private int _index = 0;
	private GameObject _characterMesh;
	
	public static Vector2 scale = Vector2.one;
	
	private bool _rotateMe = false;
	private bool _rotateCW = true;
	
	private const string MALE_MODEL_PATH = "CharacterModel/";
	
	void Start() {
		InstantiateCharacterModel();
	}
	
	public void OnEnable() {
		Messenger<bool>.AddListener("RotatePlayerClockwise", OnRotateClockwise);
	}
	
	public void OnDisable() {
		Messenger<bool>.RemoveListener("RotatePlayerClockwise", OnRotateClockwise);
	}
	
	void Update() {
		if (_rotateMe) {
			if (_rotateCW)
				transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
			else
				transform.Rotate(Vector3.down * Time.deltaTime * rotationSpeed);
			
			_rotateMe = false;
		}
		
		if (_characterMesh == null)
			return;
		
		_characterMesh.transform.localScale = new Vector3( scale.x, scale.y, scale.x);
	}
	
	private void InstantiateCharacterModel() {
		_characterMesh = Instantiate(Resources.Load(MALE_MODEL_PATH + maleModels[_index]), transform.position, Quaternion.identity) as GameObject;
		
		Destroy(_characterMesh.GetComponent<PlayerInput>());
		_characterMesh.transform.parent = transform;
		
		_characterMesh.animation["GoodIdle"].wrapMode = WrapMode.Loop;
		_characterMesh.animation.Play ("GoodIdle");
	}
	
	private void OnRotateClockwise( bool cw ) {
		_rotateMe = true;
		_rotateCW = cw;
	}
	
	public void OnGUI() {
		if (GUI.Button(new Rect(Screen.width-55, Screen.height-30,50,25), "Next")) {
			SaveCustomizations();
			Application.LoadLevel(GameSetting2.levelNames[3]);
		}
	}
	
	private void SaveCustomizations() {
		GameSetting2.SaveCharacterScale(scale.x, scale.y);
		GameObject cc = GameObject.FindWithTag("CharColor");
		ChangingRoom go = cc.GetComponent<ChangingRoom>();
		go.SaveCharacterColor();
		GameSetting2.SaveGameVersion();
	}
}
