using System.Collections.Generic;

public class PlayerCharacter : BaseCharacter {
	private static List<Item> _inventory = new List<Item>();
	public static List<Item> Inventory {
		get { return _inventory; }
	}
	
	void Update() {
		//UnityEngine.Debug.Log(UnityEngine.Application.loadedLevelName.Length == 6);
		if ((UnityEngine.Application.loadedLevelName.Length == 6)) 
			Messenger<int, int>.Broadcast("player health update", 80, 100);
	}
}
