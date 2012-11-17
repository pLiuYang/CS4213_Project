using UnityEngine;
using System.Collections;

public enum CharacterMeshMaterial {
	Feet,
	Legs,
	Torso,
	Hands,
	Face,
	COUNT
}

public class ChangingRoom : MonoBehaviour {
	private int _charModelIndex = 0;
	
	private CharacterAsset ca;
//	private PlayerCharacter PC.Instance;
	private int _weaponIndex = 0;
	private string _charModelName = "Muscular";
	
	private GameObject _characterMesh;
	
	public int _faceMaterialIndex = 0;
	public int _torsoMaterialIndex = 0;
	public int _handMaterialIndex = 0;
	public int _legMaterialIndex = 0;
	public int _feetMaterialIndex = 0;
	
	// Use this for initialization
	void Start () {
		ca = GameObject.Find("Character Asset Manager").GetComponent<CharacterAsset>();
		
		//InstantiateCharacterModel();
		//PC.Instance = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
		
//		InstantiateWeaponModel();
		
		
	}
	
	void OnGUI() {
		
		if (Application.loadedLevelName == "CharacterCustomization") {
		//ChangeCharacterMesh();
		ChangeFaceMaterialGUI();
		ChangeTorsoMaterialGUI();
		ChangeHandMaterialGUI();
		ChangeLegsMaterialGUI();
		ChangeFeetMaterialGUI();
		//ChangeWeaponMesh();
		//RotateCharacterModel();
		}
	}
	
	public void SaveCharacterColor() {
		GameSetting2.SaveCharacterColor(_faceMaterialIndex, _torsoMaterialIndex, _handMaterialIndex, _legMaterialIndex, _faceMaterialIndex);
		Debug.Log("In ChangingRoom: " + _legMaterialIndex);
	}
	
	private void ChangeFaceMaterialGUI() {
		if(GUI.Button(new Rect(Screen.width * .5f - 395, Screen.height*.5f - 175, 30, 30), "<"))
		{
			_faceMaterialIndex--;
			ChangeMeshMaterial(CharacterMeshMaterial.Face);
		}
		
		GUI.Box(new Rect(Screen.width / 2 - 360, Screen.height*.5f - 175, 120, 30), "Hands "+_faceMaterialIndex.ToString());
		
		if(GUI.Button(new Rect(Screen.width * .5f -235, Screen.height*.5f - 175, 30, 30), ">"))
		{
			_faceMaterialIndex++;
			ChangeMeshMaterial(CharacterMeshMaterial.Face);
		}
	}

	private void ChangeTorsoMaterialGUI() {
		if(GUI.Button(new Rect(Screen.width * .5f - 395, Screen.height*.5f - 140, 30, 30), "<"))
		{
			_torsoMaterialIndex--;
			ChangeMeshMaterial(CharacterMeshMaterial.Torso);
		}
		
		GUI.Box(new Rect(Screen.width / 2 - 360, Screen.height*.5f - 140, 120, 30), "Skin "+_torsoMaterialIndex.ToString());
		
		if(GUI.Button(new Rect(Screen.width * .5f -235, Screen.height*.5f - 140, 30, 30), ">"))
		{
			_torsoMaterialIndex++;
			ChangeMeshMaterial(CharacterMeshMaterial.Torso);
		}
	}

	private void ChangeHandMaterialGUI() {
		if(GUI.Button(new Rect(Screen.width * .5f - 395, Screen.height*.5f - 105, 30, 30), "<"))
		{
			_handMaterialIndex--;
			ChangeMeshMaterial(CharacterMeshMaterial.Hands);
		}
		
		GUI.Box(new Rect(Screen.width / 2 - 360, Screen.height*.5f - 105, 120, 30), "Cloth "+_handMaterialIndex.ToString());
		
		if(GUI.Button(new Rect(Screen.width * .5f -235, Screen.height*.5f - 105, 30, 30), ">"))
		{
			_handMaterialIndex++;
			ChangeMeshMaterial(CharacterMeshMaterial.Hands);
		}
	}

	private void ChangeLegsMaterialGUI() {
		if(GUI.Button(new Rect(Screen.width * .5f - 395, Screen.height*.5f - 210, 30, 30), "<"))
		{
			_legMaterialIndex--;
			ChangeMeshMaterial(CharacterMeshMaterial.Legs);
		}
		
		GUI.Box(new Rect(Screen.width / 2 - 360, Screen.height*.5f - 210, 120, 30), "Pant "+_legMaterialIndex.ToString());
		
		if(GUI.Button(new Rect(Screen.width * .5f -235, Screen.height *.5f - 210, 30, 30), ">"))
		{
			_legMaterialIndex++;
			ChangeMeshMaterial(CharacterMeshMaterial.Legs);
		}
	}

	private void ChangeFeetMaterialGUI() {
		if(GUI.Button(new Rect(Screen.width * .5f - 395, Screen.height*.5f - 245, 30, 30), "<"))
		{
			_feetMaterialIndex--;
			ChangeMeshMaterial(CharacterMeshMaterial.Feet);
		}
		
		GUI.Box(new Rect(Screen.width / 2 - 360, Screen.height*.5f - 245, 120, 30), "Shoe "+_feetMaterialIndex.ToString());
		
		if(GUI.Button(new Rect(Screen.width * .5f -235, Screen.height *.5f- 245, 30, 30), ">"))
		{
			_feetMaterialIndex++;
			ChangeMeshMaterial(CharacterMeshMaterial.Feet);
		}
	}
	
	private void RefreshCharacterMeshMaterials() {
		for(int cnt = 0; cnt < (int)CharacterMeshMaterial.COUNT; cnt++)
			ChangeMeshMaterial((CharacterMeshMaterial)cnt);
	}
	
	public void ChangeMeshMaterial(CharacterMeshMaterial cmm) {
		Material[] mats = PC.Instance.characterMaterialMesh.renderer.materials;

		for(int cnt = 0; cnt < PC.Instance.characterMaterialMesh.renderer.materials.Length; cnt++)
			mats[cnt] = PC.Instance.characterMaterialMesh.renderer.materials[cnt];

		switch(cmm) {
		case CharacterMeshMaterial.Feet:
			if(_feetMaterialIndex > ca.feetMaterial.Length - 1)
				_feetMaterialIndex = 0;
			else if(_feetMaterialIndex < 0)
				_feetMaterialIndex = ca.feetMaterial.Length - 1;

			mats[(int)cmm] = ca.feetMaterial[_feetMaterialIndex];
			break;
		case CharacterMeshMaterial.Legs:
			if(_legMaterialIndex > ca.legMaterial.Length - 1)
				_legMaterialIndex = 0;
			else if(_legMaterialIndex < 0)
				_legMaterialIndex = ca.legMaterial.Length - 1;

			mats[(int)cmm] = ca.legMaterial[_legMaterialIndex];
			break;
		case CharacterMeshMaterial.Hands:
			if(_handMaterialIndex > ca.handsMaterial.Length - 1)
				_handMaterialIndex = 0;
			else if(_handMaterialIndex < 0)
				_handMaterialIndex = ca.handsMaterial.Length - 1;

			mats[(int)cmm] = ca.handsMaterial[_handMaterialIndex];
			break;
		case CharacterMeshMaterial.Torso:
			if(_torsoMaterialIndex > ca.torsoMaterial.Length - 1)
				_torsoMaterialIndex = 0;
			else if(_torsoMaterialIndex < 0)
				_torsoMaterialIndex = ca.torsoMaterial.Length - 1;

			mats[(int)cmm] = ca.torsoMaterial[_torsoMaterialIndex];
			break;
		case CharacterMeshMaterial.Face:
			if(_faceMaterialIndex > ca.faceMaterial.Length - 1)
				_faceMaterialIndex = 0;
			else if(_faceMaterialIndex < 0)
				_faceMaterialIndex = ca.faceMaterial.Length - 1;

			mats[(int)cmm] = ca.faceMaterial[_faceMaterialIndex];
			break;
		}
		
		DestroyImmediate(PC.Instance.characterMaterialMesh.renderer.materials[(int)cmm]);

		PC.Instance.characterMaterialMesh.renderer.materials = mats;
			
//		Debug.Log(mats.Length);
//		Debug.Log("Wearing:" +PC.Instance.characterMaterialMesh.renderer.materials[2].name + " Should be wearing: " + ca.torsoMaterial[_torsoMaterialIndex].name );
//		Resources.UnloadUnusedAssets();
	}
	
	private void RotateCharacterModel() {
		if (GUI.RepeatButton(new Rect(Screen.width*.5f-95,Screen.height-35,30,30), "<"))
			_characterMesh.transform.Rotate(Vector3.up * Time.deltaTime * 100);
		
		if (GUI.RepeatButton(new Rect(Screen.width*.5f+65,Screen.height-35,30,30), ">"))
			_characterMesh.transform.Rotate(Vector3.down * Time.deltaTime * 100);
	}
	
	private void ChangeCharacterMesh() {
		if (GUI.Button(new Rect(Screen.width*.5f - 60, Screen.height-35, 120,30),_charModelName)) {
			_charModelIndex++;
			InstantiateCharacterModel();
		}
	}
	/*
	private void InstantiateWeaponModel() {
		if (_weaponIndex > ca.weaponMesh.Length - 1)
			_weaponIndex = 0;
		
		Transform pos;
		if (_weaponIndex == 0)
			pos = PC.Instance.weaponMount.transform;
		else if (_weaponIndex == 1)
			pos = PC.Instance.maceMount.transform;
		else 
			pos = PC.Instance.swordMount.transform;
		
		if (PC.Instance.weaponMount.transform.childCount > 0)
			for (int cnt = 0; cnt < PC.Instance.weaponMount.transform.childCount; cnt++) {
				Destroy(PC.Instance.weaponMount.transform.GetChild(cnt).gameObject);
			}
		
		if (PC.Instance.maceMount.transform.childCount > 0)
			for (int cnt = 0; cnt < PC.Instance.maceMount.transform.childCount; cnt++) {
				Destroy(PC.Instance.maceMount.transform.GetChild(cnt).gameObject);
			}
		
		if (PC.Instance.swordMount.transform.childCount > 0)
			for (int cnt = 0; cnt < PC.Instance.swordMount.transform.childCount; cnt++) {
				Destroy(PC.Instance.swordMount.transform.GetChild(cnt).gameObject);
			}
		
		GameObject w = Instantiate(ca.weaponMesh[_weaponIndex], pos.position, Quaternion.identity) as GameObject;
		w.transform.parent = pos;
		w.transform.rotation = new Quaternion(0, 0, 0, 0);
		//if (_weaponIndex == 2)
			//w.transform.rotation = new Quaternion(90, 0, 0, 1);
	} */
	
	private void ChangeWeaponMesh() {
		if (GUI.Button(new Rect(Screen.width*.5f - 60, Screen.height-70, 120,30),_weaponIndex.ToString())) {
			_weaponIndex++;
//			InstantiateWeaponModel();
		}
	}
	
	private void InstantiateCharacterModel() {
		switch(_charModelIndex) {
		case 1:
			_charModelName = "Fat";
			break;
		default:
			_charModelIndex = 0;
			_charModelName = "Muscular";
			break;
		}
		
		Quaternion oldPos = (_characterMesh == null) ? transform.rotation : _characterMesh.transform.rotation;
		
		if (transform.childCount > 0)
			for (int cnt = 0; cnt < transform.childCount; cnt++)
				Destroy(transform.GetChild(cnt).gameObject);
			
		_characterMesh = Instantiate(ca.characterMesh[_charModelIndex], transform.position, Quaternion.identity) as GameObject;
		//Debug.Log("Here!!!!!!!!!!");
		_characterMesh.transform.parent = transform;
		_characterMesh.transform.rotation = oldPos;
		
		_characterMesh.animation["GoodIdle"].wrapMode = WrapMode.Loop;
		_characterMesh.animation.Play ("GoodIdle");
	}
}
