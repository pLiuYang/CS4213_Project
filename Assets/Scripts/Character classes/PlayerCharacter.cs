public class PlayerCharacter : BaseCharacter {
	void Update() {
		Messenger<int, int>.Broadcast("player health update", 80, 100);
	}
}
