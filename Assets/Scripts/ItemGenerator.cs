using UnityEngine;

public static class ItemGenerator {
	public const int BASE_MELEE_RANGE = 1;
	public const int BASE_RANGED_RANGE = 5;
	
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
		
		meleeWeapon.Name = "MW:" + Random.Range(0, 100);
		
		meleeWeapon.MaxDamage = Random.Range (5, 11);
		meleeWeapon.DamageVariance = Random.Range(.2f, .76f);
		meleeWeapon.TypeOfDamage = DamageType.Slash;
		
		meleeWeapon.MaxRange = BASE_MELEE_RANGE;
		
		return meleeWeapon;
	}
}

public enum ItemType {
	Armor,
	Weapon,
	Potion,
	Scroll
}
