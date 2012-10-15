public class PlayerCharacter : BaseCharacter {
	void Update() {
		//UnityEngine.Debug.Log(UnityEngine.Application.loadedLevelName.Length == 6);
		if ((UnityEngine.Application.loadedLevelName.Length == 6)) 
			Messenger<int, int>.Broadcast("player health update", 80, 100);
	}
}
