using UnityEngine;
using System.Collections.Generic;

public class PlayerCharacter : BaseCharacter {
	public static GameObject[] _weaponMesh;
	public static GameObject wM;
	public static GameObject sM;
	public static GameObject mM;
	
	private static List<Item> _inventory = new List<Item>();
	public static List<Item> Inventory {
		get { return _inventory; }
	}
	
	private static Item _equipedWeapon;
	public static Item EquipedWeapon {
		get { return _equipedWeapon; }
		set { 
			_equipedWeapon = value;
			
//			HideWeaponMeshes();
			
			if (_equipedWeapon == null)
				return;
			
			//Debug.Log("Showing: "+_equipedWeapon.Name);
			
			string temp = GameSetting2.MELEE_WEAPON_MESH_PATH;
			int index = 0;
			
			switch(_equipedWeapon.Name) {
			case "Sword":
				temp += "Sword";
				index = 1;
				//_weaponMesh[1].active = true;
				break;
			case "Axe":
				temp += "Axe";
				index = 0;
				//_weaponMesh[0].active = true;
				break;
			case "Mace":
				temp += "Mace";
				index = 2;
				//_weaponMesh[2].active = true;
				break;
			default:
				break;
			}
			
			//Debug.Log(temp);
			Transform tempTrans;
			if (index == 0)
				tempTrans = wM.transform;
			else if (index == 1) 
				tempTrans = sM.transform;
			else
				tempTrans = mM.transform;
			
			if (wM.transform.childCount > 0)
				Destroy(wM.transform.GetChild(0).gameObject);
			
			if (sM.transform.childCount > 0)
				Destroy(sM.transform.GetChild(0).gameObject);
			
			if (mM.transform.childCount > 0)
				Destroy(mM.transform.GetChild(0).gameObject);
			
			GameObject mesh = Instantiate(Resources.Load(temp), tempTrans.position, tempTrans.rotation) as GameObject;
			
			mesh.transform.parent = tempTrans;
		}
	}
	
	void Update() {
		//UnityEngine.Debug.Log(UnityEngine.Application.loadedLevelName.Length == 6);
		if ((UnityEngine.Application.loadedLevelName.Length == 6)) 
			Messenger<int, int>.Broadcast("player health update", 80, 100);
	}
	
	void Start() {
		wM = weaponMount;
		sM = swordMount;
		mM = maceMount;
	}
	
	//private static void HideWeaponMeshes() {
	//	for (int cnt = 0; cnt < _weaponMesh.Length; cnt++) {
	//		_weaponMesh[cnt].active = false;
			//Debug.Log("Hiding: "+_weaponMesh[cnt].name);
	//	}
	//}
}
