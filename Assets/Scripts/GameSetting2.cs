using UnityEngine;
using System.Collections;
using System;

public static class GameSetting2 {
	public const string VERSION_KEY_NAME = "ver";
	public const float VERSION_NUMBER = 0.250f;
	
	//base values for different attacks
	public const float BASE_MELEE_ATTACK_TIMER = 2.0f;
	public const float BASE_MELEE_ATTACK_SPEED = 2.0f;
	public const float BASE_MELEE_RANGE = 3.5f;

	public const float BASE_RANGED_RANGE = 10f;

	public const float BASE_MAGIC_RANGE = 7.5f;

	#region PlayerPrefs Constants
	private const string PLAYER_POSITION = "Player Position";
	private const string CHARACTER_MODEL_INDEX = "Model Index";
	private const string PLAYER_HEAD_INDEX = "Head Index";
	private const string SKIN_COLOR = "Skin Color";
	private const string HAIR_COLOR = "Hair Color";
	private const string HAIR_MESH = "Hair Mesh";
	private const string NAME = "Player Name";
	private const string BASE_VALUE = " - BASE VALUE";
	private const string EXP_TO_LEVEL = " - EXP TO LEVEL";
	private const string CUR_VALUE = " - Cur Value";
	private const string CHARACTER_WIDTH = "Character Width";
	private const string CHARACTER_HEIGHT = "Character Height";
	#endregion
	
	#region Resource Paths
	public const string MALE_MODEL_PATH = "CharacterModel/";
	public const string FEMALE_MODEL_PATH = "Character/Meshes/Female/";

	public const string HEAD_TEXTURE_PATH = "Character/Faces/Human/Male/Textures/";
	
	public const string MELEE_WEAPON_ICON_PATH = "icons/";
	public const string MELEE_WEAPON_MESH_PATH = "Weapon/";

	public const string SHIELD_ICON_PATH = "icons/";
	public const string SHIELD_MESH_PATH = "Armor/";

	public const string HAT_ICON_PATH = "icons/";
//	public const string HAT_MESH_PATH = "Item/Mesh/Armor/Heads/";

	public const string HUMAN_MALE_HAIR_MESH_PATH = "Character/Hair/Human/Male/Prefab/";
	public const string HUMAN_MALE_HAIR_COLOR_PATH = "Character/Hair/Human/Male/Texture/";
		
	#endregion
	
	public static Vector3 startingPos = new Vector3( 1150,77,852 );
	
	public static string[] maleModels = { "Player Character Prefab", "muscular" };

//	public static PlayerCharacter pc;

	//index 0 = mainmenu
	//index 1 = character creation screen
	//index 2 = character customization screen
	//index 3 = tutorial level
	public static string[] levelNames = {
		"MainMenu",
		"CharacterGenerator",
		"CharacterCustomization",
		"Level1"
	};
	
	/// <summary>
	/// Default Constructor
	/// </summary>
	static GameSetting2() {
	}
	
	public static void SaveCharacterWidth( float width ) {
		PlayerPrefs.SetFloat( CHARACTER_WIDTH , width );
	}
	
	public static void SaveCharacterHeight( float height ) {
		PlayerPrefs.SetFloat( CHARACTER_HEIGHT , height );
	}
	
	public static void SaveCharacterScale( float width, float height ) {
		SaveCharacterWidth( width );
		SaveCharacterHeight( height );
	}
	
	public static float LoadCharacterWidth() {
		return PlayerPrefs.GetFloat( CHARACTER_WIDTH, 1 );
	}

	public static float LoadCharacterHeight() {
		return PlayerPrefs.GetFloat( CHARACTER_HEIGHT, 1 );
	}
	
	public static Vector2 LoadCharacterScale() {
		return new Vector2( PlayerPrefs.GetFloat( CHARACTER_WIDTH, 1 ), PlayerPrefs.GetFloat( CHARACTER_HEIGHT, 1 ) );
	}
	
	
	public static void SaveCharacterColor(int face, int torse, int hand, int leg, int feet) {
		PlayerPrefs.SetInt( "Face_Material", face);
		PlayerPrefs.SetInt( "Torso_Material", torse);
		PlayerPrefs.SetInt( "Hand_Material", hand);
		PlayerPrefs.SetInt( "Leg_Material", leg);
		PlayerPrefs.SetInt( "Feet_Material", feet);
	}
	
	public static int[] LoadCharacterColor() {
		int[] temp = {
			PlayerPrefs.GetInt( "Face_Material", 0),
			PlayerPrefs.GetInt( "Torso_Material", 0),
			PlayerPrefs.GetInt( "Hand_Material", 0),
			PlayerPrefs.GetInt( "Leg_Material", 0),
			PlayerPrefs.GetInt( "Feet_Material", 0)
		};
		
		Debug.Log("In GameSetting2: face-"+ temp[0]+"  torso-"+temp[1]+" Hand-"+temp[2]+" Leg-"+temp[3]+" feet-"+temp[4]);
		
		return temp;
	}
	
	public static void SavePlayerPosition( Vector3 pos ) {
		PlayerPrefs.SetFloat( PLAYER_POSITION + "x", pos.x );
		PlayerPrefs.SetFloat( PLAYER_POSITION + "y", pos.y );
		PlayerPrefs.SetFloat( PLAYER_POSITION + "z", pos.z );
	}

	public static Vector3 LoadPlayerPosition() {
		Vector3 temp = new Vector3(
		                           PlayerPrefs.GetFloat( PLAYER_POSITION + "x", startingPos.x ),
		                           PlayerPrefs.GetFloat( PLAYER_POSITION + "y", startingPos.y ),
		                           PlayerPrefs.GetFloat( PLAYER_POSITION + "z", startingPos.z )
		                           );
		return temp;
	}


	public static void SaveHeadIndex( int index ) {
		PlayerPrefs.SetInt( PLAYER_HEAD_INDEX, index );
	}

	public static int LoadHeadIndex() {
		return PlayerPrefs.GetInt( PLAYER_HEAD_INDEX, 1 );
	}

	public static void SaveCharacterModelIndex( int index ) {
		PlayerPrefs.SetInt( CHARACTER_MODEL_INDEX, index );
	}

	public static int LoadCharacterModelIndex() {
		return PlayerPrefs.GetInt( CHARACTER_MODEL_INDEX, 1 );
	}

	public static void SaveSkinColor( int index ) {
		PlayerPrefs.SetInt( SKIN_COLOR , index );
	}

	public static int LoadSkinColor() {
		return PlayerPrefs.GetInt( SKIN_COLOR, 1 );
	}

	
	/// <summary>
	/// Store the index of the hair color as an int
	/// </summary>
	/// <param name="index">
	/// A <see cref="System.Int32"/>
	/// </param>
	public static void SaveHairColor( int index ) {
		PlayerPrefs.SetInt( HAIR_COLOR , index );
	}
	

	/// <summary>
	/// Load the players selected index for the hair color they have selected
	/// </summary>
	/// <returns>
	/// A <see cref="System.Int32"/>
	/// </returns>
	public static int LoadHairColor() {
		return PlayerPrefs.GetInt( HAIR_COLOR, 0 );
	}
	
	
	/// <summary>
	/// Save the selected index for the hair mesh as an int
	/// </summary>
	/// <param name="index">
	/// A <see cref="System.Int32"/>
	/// </param>
	public static void SaveHairMesh( int index ) {
		PlayerPrefs.SetInt( HAIR_MESH , index );
//		Debug.Log( HAIR_MESH + " : " + index  );
	}
	

	/// <summary>
	/// Load both the hair color and the hair mesh the player as selected, both as an int
	/// </summary>
	/// <param name="index">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <returns>
	/// A <see cref="System.Int32"/>
	/// </returns>
	public static int LoadHairMesh() {
		return PlayerPrefs.GetInt( HAIR_MESH, 0 );
	}
	
	
	/// <summary>
	/// Save both the hair color and the hair mesh the player as selected from the playerprefs
	/// </summary>
	/// <param name="mesh">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <param name="color">
	/// A <see cref="System.Int32"/>
	/// </param>
	public static void SaveHair( int mesh, int color ) {
		SaveHairColor( color );
		SaveHairMesh( mesh );
	}
	

	public static void SaveName( string name ) {
		PlayerPrefs.SetString( NAME, name );
	}
	

	public static string LoadName() {
		return PlayerPrefs.GetString(NAME, "Anon");
	}
	

	public static void SaveAttribute( AttributeName name, Attribute attribute ) {
		PlayerPrefs.SetInt(((AttributeName)name).ToString() + BASE_VALUE, attribute.BaseValue );
		PlayerPrefs.SetInt(((AttributeName)name).ToString() + EXP_TO_LEVEL, attribute.ExpToLevel );
	}
	

	public static Attribute LoadAttribute( AttributeName name ) {
		Attribute att = new Attribute();
		att.BaseValue = PlayerPrefs.GetInt(((AttributeName)name).ToString() + BASE_VALUE, 0);
		att.ExpToLevel = PlayerPrefs.GetInt(((AttributeName)name).ToString() + EXP_TO_LEVEL, Attribute.START_EXP_COST);
		return att;
//		PC.Instance.GetPrimaryAttribute( (int)name ).BaseValue  = PlayerPrefs.GetInt(((AttributeName)name).ToString() + BASE_VALUE, 0);
//		PC.Instance.GetPrimaryAttribute( (int)name ).ExpToLevel = PlayerPrefs.GetInt(((AttributeName)name).ToString() + EXP_TO_LEVEL, Attribute.STARTING_EXP_COST);
	}
	

	public static void SaveAttributes( Attribute[] attribute ) {
		for( int cnt = 0; cnt < attribute.Length; cnt++ )
			SaveAttribute( (AttributeName)cnt, attribute[cnt] );
	}
	

	public static void LoadAttributes() {
		for(int cnt = 0; cnt < Enum.GetValues(typeof(AttributeName)).Length; cnt++) {
			PC.Instance.primaryAttribute[cnt] = LoadAttribute( (AttributeName)cnt );
		}
	}


	public static void SaveVital( VitalName name, Vital vital ) {
		PlayerPrefs.SetInt(((VitalName)name).ToString() + BASE_VALUE, vital.BaseValue);
		PlayerPrefs.SetInt(((VitalName)name).ToString() + EXP_TO_LEVEL, vital.ExpToLevel);
		PlayerPrefs.SetInt(((VitalName)name).ToString() + CUR_VALUE, vital.CurValue);
	}
	

	public static void LoadVital( VitalName name ) {
		PC.Instance.GetVital( (int)name ).BaseValue = PlayerPrefs.GetInt(((VitalName)name).ToString() + BASE_VALUE, 0);
		PC.Instance.GetVital( (int)name ).ExpToLevel = PlayerPrefs.GetInt(((VitalName)name).ToString() + EXP_TO_LEVEL, 0);
		
		//make sure you call this so that the AjustedBaseValue will be updated before you try to call to get the curValue
		PC.Instance.GetVital( (int)name ).Update();

		//get the stored value for the curValue for each vital
		PC.Instance.GetVital( (int)name ).CurValue = PlayerPrefs.GetInt(((VitalName)name).ToString() + CUR_VALUE, 1);
	}
	

	public static void SaveVitals( Vital[] vital ) {
		for( int cnt = 0; cnt < vital.Length; cnt++ )
			SaveVital( (VitalName)cnt, vital[cnt] );
	}
	

	public static void LoadVitals() {
		for(int cnt = 0; cnt < Enum.GetValues(typeof(VitalName)).Length; cnt++)
			LoadVital( (VitalName)cnt );
	}
	

	public static void SaveSkill( SkillName name, Skill skill ) {
		PlayerPrefs.SetInt(((SkillName)name).ToString() + BASE_VALUE, skill.BaseValue);
		PlayerPrefs.SetInt(((SkillName)name).ToString() + EXP_TO_LEVEL, skill.ExpToLevel);
	}
	

	public static void LoadSkill( SkillName name ) {
		PC.Instance.GetSkill( (int)name ).BaseValue = PlayerPrefs.GetInt(((SkillName)name).ToString() + BASE_VALUE, 0);
		PC.Instance.GetSkill( (int)name ).ExpToLevel = PlayerPrefs.GetInt(((SkillName)name).ToString() + EXP_TO_LEVEL, 0);
		PC.Instance.GetSkill( (int)name ).Update();
	}
	

	public static void SaveSkills( Skill[] skill ) {
		for( int cnt = 0; cnt < skill.Length; cnt++ )
			SaveSkill( (SkillName)cnt, skill[cnt] );
	}
	

	public static void LoadSkills() {
		for(int cnt = 0; cnt < Enum.GetValues(typeof(SkillName)).Length; cnt++) 
			LoadSkill( (SkillName)cnt );
	}
	
	public static void SaveGameVersion() {
		PlayerPrefs.SetFloat( VERSION_KEY_NAME, VERSION_NUMBER);
	}
	
	public static float LoadGameVersion() {
		return PlayerPrefs.GetFloat( VERSION_KEY_NAME, 0);
	}
}

public enum CharacterMaterialIndex {
	Feet = 0,
	Pants = 1,
	Torso = 2,
	Hands = 3,
	Face = 4
}