using UnityEngine;
using System.Collections.Generic;

public class PlayerCharacter : BaseCharacter {
	public static GameObject[] _weaponMesh;
	
	private static List<Item> _inventory = new List<Item>();
	public static List<Item> Inventory {
		get { return _inventory; }
	}
	
	private static Item _equipedWeapon;
	public static Item EquipedWeapon {
		get { return _equipedWeapon; }
		set { 
			_equipedWeapon = value;
			
			HideWeaponMeshes();
			
			if (_equipedWeapon == null)
				return;
			
			//Debug.Log("Showing: "+_equipedWeapon.Name);
			
			switch(_equipedWeapon.Name) {
			case "Sword":
				_weaponMesh[1].active = true;
				break;
			case "Axe":
				_weaponMesh[0].active = true;
				break;
			case "Mace":
				_weaponMesh[2].active = true;
				break;
			default:
				break;
			}
		}
	}
	
	void Awake() {
		base.Awake ();
		
		Transform weaponMount = transform.Find("Armature/hand_R_001/RH_Weapon_Mount");
		int count = weaponMount.GetChildCount();
		
		_weaponMesh = new GameObject[count];
		
		for (int cnt = 0; cnt < count; cnt++) {
			_weaponMesh[cnt] = weaponMount.GetChild(cnt).gameObject;
		}
		HideWeaponMeshes();
	}
	
	void Update() {
		//UnityEngine.Debug.Log(UnityEngine.Application.loadedLevelName.Length == 6);
		if ((UnityEngine.Application.loadedLevelName.Length == 6)) 
			Messenger<int, int>.Broadcast("player health update", 80, 100);
	}
	
	private static void HideWeaponMeshes() {
		for (int cnt = 0; cnt < _weaponMesh.Length; cnt++) {
			_weaponMesh[cnt].active = false;
			//Debug.Log("Hiding: "+_weaponMesh[cnt].name);
		}
	}
}
