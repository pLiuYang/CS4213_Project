using UnityEngine;

public static class ItemGenerator {
	public const int BASE_MELEE_RANGE = 1;
	public const int BASE_RANGED_RANGE = 5;
	
	private const string MELEE_WEAPON_PATH = "icons/";
	
	public static Item CreateItem() {
		Item item = CreateWeapon();
		
		item.Value = Random.Range(1, 101);
		
		item.Rarity = RarityTypes.Common;
		
		item.MaxDurability = Random.Range(50, 61);
		
		item.CurDurability = item.MaxDurability;
		
		return item;
	}
	
	private static Weapon CreateWeapon() {
		Weapon weapon = CreateMeleeWeapon();
		
		return weapon;
	}
	
	private static Weapon CreateMeleeWeapon() {
		Weapon meleeWeapon = new Weapon();
		
		string[] weaponNames = new string[3];
		weaponNames[0] = "Sword";
		weaponNames[1] = "Axe";
		weaponNames[2] = "Mace";
		//Debug.Log(Random.Range(0, weaponNames.Length));
		meleeWeapon.Name = weaponNames[Random.Range(0, weaponNames.Length)];
		
		meleeWeapon.MaxDamage = Random.Range (5, 11);
		meleeWeapon.DamageVariance = Random.Range(.2f, .76f);
		meleeWeapon.TypeOfDamage = DamageType.Slash;
		
		meleeWeapon.MaxRange = BASE_MELEE_RANGE;
		
		meleeWeapon.Icon = Resources.Load(MELEE_WEAPON_PATH + meleeWeapon.Name) as Texture2D;
		
		return meleeWeapon;
	}
}

public enum ItemType {
	Armor,
	Weapon,
	Potion,
	Scroll
}
