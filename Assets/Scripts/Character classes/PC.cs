using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Hack And Slash Tutorial/Player/PC Stats")]
public class PC : BaseCharacter {
	private List<Item> inventory = new List<Item>();
	public List<Item> Inventory {
		get { return inventory; }
		set { inventory = value; }
	}
	
	public bool initialized = false;

	private static PC instance = null;
	public static PC Instance {
		get {
			if ( instance == null ) {
				//Debug.Log( "***PC - Instance***" );
				GameObject go = Instantiate( Resources.Load(
				                             GameSetting2.MALE_MODEL_PATH + "Player Character Prefab" ),
				                           	 //new Vector3(994,92,1000),//
											 GameSetting2.LoadPlayerPosition(),
				                             Quaternion.identity ) as GameObject;
				
				PC temp = go.GetComponent<PC>();
				
				if( temp == null )
					Debug.LogError( "Player Prefab does not contain an PC script. Please add and configure." );
				
				instance = go.GetComponent<PC>();
				
				go.name = "PC";
				go.tag = "Player";
			}
			
			return instance;
		}
	}
	
	public void Initialize() {
		//Debug.Log( "***PC - Initialize***" );
		if( !initialized )
			LoadCharacter();
	}
	
	#region Unity functions
	public new void Awake() {
		//Debug.Log( "***PC - Awake***" );

		base.Awake();

		instance = this;
		
		//Initialize();
	}

	//we do not want to be sending messages out each frame. We will be moving this out when we get back in to combat
	void Update() {
		Messenger<int, int>.Broadcast("player health update", 80, 100, MessengerMode.DONT_REQUIRE_LISTENER);
	
		if (Input.GetKeyDown(KeyCode.Tab)) {
			TargetEnemy();
		}
		
		if (Input.GetKeyDown(KeyCode.F)) {
			//animation.CrossFade("AxeSlash");
			SendMessage("PlayMeleeAttack");
			Debug.Log("Attacking");
		}
	}
	#endregion


	public Item EquipedWeapon {
		get { return _equipment[(int)EquipmentSlot.MainHand]; }
		set {			
			_equipment[(int)EquipmentSlot.MainHand] = value;
			
			if (swordMount.transform.childCount > 0)
					Destroy(swordMount.transform.GetChild(0).gameObject);
			
				if (weaponMount.transform.childCount > 0)
					Destroy(weaponMount.transform.GetChild(0).gameObject);
			
				if (maceMount.transform.childCount > 0)
					Destroy(maceMount.transform.GetChild(0).gameObject);
			   
			if( _equipment[ (int)EquipmentSlot.MainHand ] != null) {
				Transform temp;
			
				switch(_equipment[(int)EquipmentSlot.MainHand].Name) {
				case "Sword":
					temp = swordMount.transform;
					break;
				case "Axe":
					temp = weaponMount.transform;
					break;
				case "Mace":
					temp = maceMount.transform;
					break;
				default:
					temp = transform;
					break;
				}
			
				GameObject mesh = Instantiate( Resources.Load( GameSetting2.MELEE_WEAPON_MESH_PATH + _equipment[(int)EquipmentSlot.MainHand].Name ), temp.position, temp.rotation ) as GameObject;
				mesh.transform.parent = temp;
			}
		}
	}
	

	public Item EquipedShield {
		get { return _equipment[(int)EquipmentSlot.OffHand]; }
		set {
			_equipment[(int)EquipmentSlot.OffHand] = value;
			
			if( offHandMount.transform.childCount > 0 )
				Destroy( offHandMount.transform.GetChild( 0 ).gameObject );
				      
			if( _equipment[(int)EquipmentSlot.OffHand] != null) {
				GameObject mesh = Instantiate( Resources.Load( GameSetting2.SHIELD_MESH_PATH + "Shield_FBX" ), offHandMount.transform.position, offHandMount.transform.rotation ) as GameObject;
				mesh.transform.parent = offHandMount.transform;
			}
		}
	}
	
	public Item EquipedHeadGear {
		get { return _equipment[(int)EquipmentSlot.Head]; }
		set {
			_equipment[(int)EquipmentSlot.Head] = value;
			
			//if( offHandMount.transform.childCount > 0 )
				//Destroy( offHandMount.transform.GetChild( 0 ).gameObject );
				      
			//if( _equipment[(int)EquipmentSlot.Head] != null) {
				//GameObject mesh = Instantiate( Resources.Load( GameSetting2.SHIELD_MESH_PATH + "Shield_FBX" ), offHandMount.transform.position, offHandMount.transform.rotation ) as GameObject;
				//mesh.transform.parent = offHandMount.transform;
			//}
		}
	}

	
	//public Item EquipedHeadGear {
	//	get { return _equipment[(int)EquipmentSlot.Head]; }
	//	set {
	//		_equipment[(int)EquipmentSlot.Head] = value;
			
			//if( helmetMount.transform.childCount > 0 )
			//	Destroy( helmetMount.transform.GetChild( 0 ).gameObject );
				      
			//if( _equipment[(int)EquipmentSlot.Head] != null) {
			//	GameObject mesh = Instantiate( Resources.Load( GameSetting2.HAT_MESH_PATH + _equipment[(int)EquipmentSlot.Head].Name ), helmetMount.transform.position, helmetMount.transform.rotation ) as GameObject;
			//	mesh.transform.parent = helmetMount.transform;
				
				//scale
			//	mesh.transform.localScale = hairMount.transform.GetChild(0).localScale;
				
				//hide player hair
			//	hairMount.transform.GetChild(0).gameObject.active = false;
	//		}
	//	}
	//}
	
	
	public void LoadCharacter() {
		GameSetting2.LoadAttributes();
		ClearModifiers();
		GameSetting2.LoadVitals();
		GameSetting2.LoadSkills();
		GameSetting2.LoadPlayerPosition();
		SetMaterial(GameSetting2.LoadCharacterColor());
		
//		LoadHair();
//		LoadSkinColor();
		
		LoadScale();

		initialized = true;
	}
	
	public void SetMaterial(int[] temp) {
		Material[] mats = PC.Instance.characterMaterialMesh.renderer.materials;
		GameObject cc = GameObject.FindWithTag("CharColor");
		if (cc != null) {
		CharacterAsset ca = cc.GetComponent<CharacterAsset>();
		if (ca != null) {
			Debug.Log("Find Asset Manager");
			mats[0] = ca.feetMaterial[temp[0]];
			mats[1] = ca.legMaterial[temp[1]];
			mats[2] = ca.handsMaterial[temp[2]];
			mats[3] = ca.torsoMaterial[temp[3]];
			mats[4] = ca.faceMaterial[temp[4]];
			
			PC.Instance.characterMaterialMesh.renderer.materials = mats;
		} else {
			Debug.Log ("No Asset Manager");
		}
		}
	}
	
	public void LoadScale() {
		Vector2 scale = GameSetting2.LoadCharacterScale();
		
		transform.localScale = new Vector3(
		                                   transform.localScale.x * scale.x,
		                                   transform.localScale.y * scale.y,
		                                   transform.localScale.z * scale.x
		                                   );
	}
	
	//public void LoadHair() {
	//	LoadHairMesh();
	//	LoadHairColor();
	//}
	
	public void LoadSkinColor() {
		characterMaterialMesh.renderer.materials[ (int)CharacterMaterialIndex.Face ].mainTexture = Resources.Load( GameSetting2.HEAD_TEXTURE_PATH + "head_" + GameSetting2.LoadHeadIndex() + "_" + GameSetting2.LoadSkinColor() + ".human") as Texture;
	}

	//public void LoadHairMesh() {
	//	if( hairMount.transform.childCount > 0 )
	//		Object.Destroy( hairMount.transform.GetChild(0).gameObject );

	//	GameObject hairStyle;
		
	//	int hairMeshIndex = GameSetting2.LoadHairMesh();

	//	int hairSet = hairMeshIndex / 5 + 1;
	//	int hairIndex = hairMeshIndex % 5 + 1;
		
	//	hairStyle = Object.Instantiate( Resources.Load(
	//	                                               GameSetting2.HUMAN_MALE_HAIR_MESH_PATH + "Hair" + " " + hairSet + "_" + hairIndex ),
	//	                               				   hairMount.transform.position,
	//	                                               hairMount.transform.rotation
	//	                                              ) as GameObject;

	//	hairStyle.transform.parent = PC.Instance.hairMount.transform;
		
	//	LoadHairColor();		

		//MeshOffset mo = hairStyle.GetComponent<MeshOffset>();
		//if( mo == null )
		//	return;
		
		//hairStyle.transform.localPosition = mo.positionOffset;
		//hairStyle.transform.localRotation = Quaternion.Euler( mo.rotationOffset );
		//hairStyle.transform.localScale = mo.scaleOffset;
	//}
	
//	public void LoadHairColor() {
//		Texture temp = Resources.Load( GameSetting2.HUMAN_MALE_HAIR_COLOR_PATH + ((HairColorNames)GameSetting2.LoadHairColor()).ToString()) as Texture;
		
//		hairMount.transform.GetChild(0).renderer.material.mainTexture = temp;
//	}
	
	public void LoadHelmet() {
	}

	public void LoadShoulderPads() {
	}

	public void LoadTorsoArmor() {
	}
	
	public void LoadGloves() {
	}

	public void LoadLegArmor() {
	}

	public void LoadBoots() {
	}

	public void LoadBackItem() {
	}
	
	public List<Transform> targets;
	public Transform selectedTarget;
	
	private Transform myTransform;

	// Use this for initialization
	void Start () {
		targets = new List<Transform>();
		selectedTarget = null;
		myTransform = transform;
		
		AddAllEnemies();
	}
	
	public void AddAllEnemies() {
		GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");
		
		foreach (GameObject enemy in go)
			AddTarget(enemy.transform);
	}
	
	public void AddTarget(Transform enemy) {
		targets.Add(enemy);
	}
	
	private void SortTargetsByDistance() {
		targets.Sort(delegate(Transform t1, Transform t2) { 
			return (Vector3.Distance(t1.position, myTransform.position)).CompareTo(Vector3.Distance(t2.position, myTransform.position));
		});
	}
	
	private void TargetEnemy() {
		if (targets.Count == 0) {
			AddAllEnemies();
		}
		
		if (targets.Count > 0) {
			if (selectedTarget == null)
			{
				SortTargetsByDistance();
				selectedTarget = targets[0];
			}
			else
			{
				int index = targets.IndexOf(selectedTarget);
				
				if (index < targets.Count - 1)
					index++;
				else
					index = 0;
				
				DeselectTarget();
				selectedTarget = targets[index];
			}
			SelectTarget();
		}
	}
	
	private void SelectTarget() {
		Transform name = selectedTarget.Find("Name");
		
		if (name == null) {
			Debug.LogError("Could not find the Name on " + selectedTarget.name);
			return;
		}
		
		name.GetComponent<TextMesh>().text = selectedTarget.GetComponent<Mob>().name;
		name.GetComponent<MeshRenderer>().enabled = true;
		
		Messenger<bool>.Broadcast("show mob vitalbars", true);
	}
	
	private void DeselectTarget() {
		selectedTarget.FindChild("Name").GetComponent<MeshRenderer>().enabled = false;
		
		selectedTarget = null;
		Messenger<bool>.Broadcast("show mob vitalbars", false);
	}
}
/*
public enum EquipmentSlot {
	Head,
	Shoulders,
	Torso,
	Legs,
	Hands,
	Feet,
	Back,
	OffHand,
	MainHand,
	COUNT
} */