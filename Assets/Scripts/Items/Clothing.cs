using UnityEngine;

public class Clothing : BuffItem {
	private EquipmentSlot _slot;
//	private ArmorSlot _slot;
	
	public Clothing() {
//		_slot = ArmorSlot.Head;
		_slot = EquipmentSlot.Head;
	}
	
	public Clothing(EquipmentSlot slot) {
		_slot = slot;
	}
	
	public EquipmentSlot Slot {
		get {return _slot;}
		set {_slot = value;}
	}
}

/*
public enum ArmorSlot {
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
}*/
