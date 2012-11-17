using UnityEngine;
using System.Collections;
using System;					//added to access the enum

[AddComponentMenu("Hack And Slash Tutorial/Mob/All Mob Scripts")]
[RequireComponent (typeof(CharacterController))]
[RequireComponent (typeof(SphereCollider))]
[RequireComponent (typeof(AI))]
[RequireComponent (typeof(AdvancedMovement))]

public class Mob : BaseCharacter {
	static public GameObject camera;
	
	private Transform displayName;
	
	
	new void Awake() {
		base.Awake();
		
		Spawn();
	}
	
	// Use this for initialization
	void Start () {
		//find the player camera
		camera = GameObject.Find( "Main Camera" );

		//find the Name component
		displayName = transform.FindChild("Name");
		
		//if it does not exist, warn and return
		if(displayName == null) {
			Debug.LogWarning("Please Add a 3DText to the mob.");
			return;
		}
		
		//change the displayed name to what we want
		displayName.GetComponent<TextMesh>().text = name;
		displayName.GetComponent<MeshRenderer>().enabled = false;
	}
	
	
	public void DisplayHealth() {
//		Messenger<int, int>.Broadcast("mob health update", curHealth, maxHealth);
	}
	
	
	private void Spawn() {
		//setup attributes and skills
		SetupStats();
		//setup gear
		SetupGear();
	}
	

	private void SetupStats() {
		for( int cnt = 0; cnt < Enum.GetValues(typeof(AttributeName)).Length; cnt++ )
			GetPrimaryAttribute(cnt).BaseValue = UnityEngine.Random.Range( 50, 101 );
		
		StatUpdate();

		for( int cnt = 0; cnt < Enum.GetValues(typeof( VitalName )).Length; cnt++ )
			GetVital(cnt).CurValue = GetVital(cnt).AdjustedBaseValue;
	}
	
	void Update() {
		//if it does not exist, warn and return
		if(displayName == null) {
			Debug.LogWarning("Please Add a 3DText to the mob.");
			return;
		}
		
		if( camera == null ) {
			Debug.LogWarning("Can not find the player Camera.");
			return;
		}
		
		displayName.LookAt( camera.transform );
		displayName.Rotate( new Vector3( 0, 180, 0 ) );
			
	}

	/*
	//display the mob stats only if we have it targettted and the DEBUGGER is defined
	void OnGUI() {
			int lh = 20;
			
			for( int cnt = 0; cnt < Enum.GetValues( typeof( AttributeName ) ).Length; cnt++ )
				GUI.Label( new Rect( 10, 10 + (cnt * lh), 140, lh ), ((AttributeName)cnt).ToString() + ": " + GetPrimaryAttribute(cnt).BaseValue );

			for( int cnt = 0; cnt < Enum.GetValues(typeof( VitalName )).Length; cnt++ )
				GUI.Label( new Rect( 10, 10 + (cnt * lh) + ( Enum.GetValues(typeof(AttributeName)).Length * lh), 140, lh ), ((VitalName)cnt).ToString() + ": " + GetVital(cnt).CurValue + " / " + GetVital(cnt).AdjustedBaseValue );

			for( int cnt = 0; cnt < Enum.GetValues( typeof( SkillName ) ).Length; cnt++ )
				GUI.Label( new Rect( 150, 10 + (cnt * lh), 140, lh ), ((SkillName)cnt).ToString() + ": " + GetSkill(cnt).AdjustedBaseValue );
		
	} */

	
	
	private void SetupGear() {
		//use the itemgenerator script to create a melee weapon
		EquipedWeapon = ItemGenerator.CreateItem( ItemType.MeleeWeapon );

		//use the itemgenerator script to create a ranged weapon
		
		Debug.Log( "Name: " + _equipment[(int)EquipmentSlot.MainHand].Name );
		Debug.Log( "Name: " + ((Weapon)_equipment[(int)EquipmentSlot.MainHand]).MaxDamage );
		
		
	}
}
