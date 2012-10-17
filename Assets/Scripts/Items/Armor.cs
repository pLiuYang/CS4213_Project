using UnityEngine;

public class Armor : Clothing {
	private int _armorLevel;
	
	public Armor() {
		_armorLevel = 0;
	}
	
	public Armor(int al) {
		_armorLevel = al;
	}
	
	public int ArmorLevel {
		get {return _armorLevel;}
		set {_armorLevel = value;}
	}
}
