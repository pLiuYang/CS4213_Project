using UnityEngine;
using System.Collections;
using System;					//added to access the enum class

public class BaseCharacter : MonoBehaviour {
	public GameObject weaponMount;
	public GameObject offHandMount;
	public GameObject swordMount;
	public GameObject maceMount;
	public GameObject characterMaterialMesh;
	public string name;
	
//	private string _name;
	private int _level;
	private uint _freeExp;
	
	public Attribute[] primaryAttribute;
	public Vital[] vital;
	public Skill[] skill;
	
	public float meleeAttackTimer = GameSetting2.BASE_MELEE_ATTACK_TIMER;
	public float meleeAttackSpeed = GameSetting2.BASE_MELEE_ATTACK_SPEED;
	public float meleeResetTimer = 0f;
	
	public bool inCombat;
	protected Item[] _equipment = new Item[ (int)EquipmentSlot.COUNT ];
		
	
	
	/*
	 * The Awake method is called before any Start() methods. We are going to use this to make sure our
	 * variables have a default value, as well makng sure that everything that needs a reference to 
	 * something else, has it
	 */
	public virtual void Awake() {
//		_name = string.Empty;
		_level = 0;
		_freeExp = 0;
		inCombat = false;
		
		primaryAttribute = new Attribute[Enum.GetValues(typeof(AttributeName)).Length];
		vital = new Vital[Enum.GetValues(typeof(VitalName)).Length];
		skill = new Skill[Enum.GetValues(typeof(SkillName)).Length];
		
		SetupPrimaryAttributes();
		SetupVitals();
		SetupSkills();
	}
	
	#region Setters and Getters
//	public string Name {
//		get{ return _name; }
//		set { _name = value; }
//	}
	
	public int Level {
		get{ return _level; }
		set{ _level = value; }
	}
	
	public uint FreeExp {
		get{ return _freeExp; }
		set{ _freeExp = value; }
	}
	
	#endregion

	
	/*
	 * Add a certain amount of exp to our characters free exp pool. We are using an uint as the value
	 * for _freeExp can never be negative and this allows us to get a larger number then using its
	 * signed version
	 */
	public void AddExp(uint exp) {
		_freeExp += exp;
		
		CalculateLevel();
	}

	
	//take avg of all of the players skills and assign that as the player level
	public void CalculateLevel() {
	}
	

	/*
	 * iterate though all of the characters primary attributes and set them up for use
	 */
	private void SetupPrimaryAttributes() {
		for(int cnt = 0; cnt < primaryAttribute.Length; cnt++) {
			primaryAttribute[cnt] = new Attribute();
			primaryAttribute[cnt].Name = ((AttributeName)cnt).ToString();
		}
	}
	
	/*
	 * iterate though all of the characters vitals and set them up for use
	 */
	private void SetupVitals() {
		for(int cnt = 0; cnt < vital.Length; cnt++)
			vital[cnt] = new Vital();
		
		SetupVitalModifiers();
	}

	
	/*
	 * iterate though all of the characters skills and make sure they are set up for use
	 */
	private void SetupSkills() {
		for(int cnt = 0; cnt < skill.Length; cnt++)
			skill[cnt] = new Skill();
		
		SetupSkillModifiers();
	}

	
	/*
	 * return a certain primary attribute in our array given the index
	 * TODO:
	 * make sure the index we are passed is within range of the size of the array
	 * instead of passing back a null on error, pass back an empty attribute (no values)
	 */
	public Attribute GetPrimaryAttribute(int index) {
		return primaryAttribute[index];
	}

	
	/*
	 * return a vital given the index
	 * TODO:
	 * make sure the index we are passed is within range of the size of the array
	 * instead of passing back a null on error, pass back an empty vital (no values)
	 */
	public Vital GetVital(int index) {
		return vital[index];
	}
	
	
	/*
	 * return a skill given the index
	 * TODO:
	 * make sure the index we are passed is within range of the size of the array
	 * instead of passing back a null on error, pass back an empty skill (no values)
	 */
	public Skill GetSkill(int index) {
		return skill[index];
	}

	public void ClearModifiers() {
		for( int cnt = 0; cnt < vital.Length; cnt++ )
			vital[cnt].ClearModifiers();
		
		for( int cnt = 0; cnt < skill.Length; cnt++ )
			skill[cnt].ClearModifiers();

		SetupVitalModifiers();
		SetupSkillModifiers();
	}
	
	/*
	 * Setup the moodifiers that our vitals will have based on the primary attributes
	 */
	private void SetupVitalModifiers() {
		GetVital((int)VitalName.Health).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Constitution), .5f));	//health
		GetVital((int)VitalName.Energy).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Constitution), 1));	//energy
		GetVital((int)VitalName.Mana).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.WillPower), 1));		//mana

	}
	

	/*
	 * Setup the modifiers that our skills will have based n the promary attributes
	 */
	private void SetupSkillModifiers() {
//		Debug.Log("BaseCharacter - SetupSkillModifiers***");
		//melee offence
		GetSkill((int)SkillName.Melee_Offence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Might), .33f));
		GetSkill((int)SkillName.Melee_Offence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Nimbleness), .33f));
		//melee defence
		GetSkill((int)SkillName.Melee_Defence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Speed), .33f));
		GetSkill((int)SkillName.Melee_Defence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Constitution), .33f));
		//magic offence
		GetSkill((int)SkillName.Magic_Offence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Concentration), .33f));
		GetSkill((int)SkillName.Magic_Offence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.WillPower), .33f));
		//magic defence
		GetSkill((int)SkillName.Magic_Defence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Concentration), .33f));
		GetSkill((int)SkillName.Magic_Defence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.WillPower), .33f));
		//ranged offence
		GetSkill((int)SkillName.Ranged_Offence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Concentration), .33f));
		GetSkill((int)SkillName.Ranged_Offence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Speed), .33f));
		//ranged defence
		GetSkill((int)SkillName.Ranged_Defence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Speed), .33f));
		GetSkill((int)SkillName.Ranged_Defence).AddModifier(new ModifyingAttribute(GetPrimaryAttribute((int)AttributeName.Nimbleness), .33f));
	}
	

	/*
	 * A wrapper method that is used to call all of the updates on all of your skills and vitals
	 */
	public void StatUpdate() {
//		Debug.Log( "***BaseCharacter - StatUpdate***" );
		for(int cnt = 0; cnt < vital.Length; cnt++)
			vital[cnt].Update();
		
		for(int cnt = 0; cnt < skill.Length; cnt++)
			skill[cnt].Update();
	}



	public Item EquipedWeapon {
		get { return _equipment[(int)EquipmentSlot.MainHand]; }
		set {
			_equipment[(int)EquipmentSlot.MainHand] = value;
			
/*			if( weaponMount.transform.childCount > 0 )
				Destroy( weaponMount.transform.GetChild( 0 ).gameObject );
				      
			if( _equipment[ (int)EquipmentSlot.MainHand ] != null) {
				GameObject mesh = Instantiate( Resources.Load( GameSetting2.MELEE_WEAPON_MESH_PATH + _equipment[(int)EquipmentSlot.MainHand].Name ), weaponMount.transform.position, weaponMount.transform.rotation ) as GameObject;
				mesh.transform.parent = weaponMount.transform;
			} */
		}
	}
	
	public void CalculateMeleeAttackSpeed() {
		//todo
	}
	
	public bool InCombat {
		get{ return inCombat; }
		set{ inCombat = value; }
	}
}



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
}