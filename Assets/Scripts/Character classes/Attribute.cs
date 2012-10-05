public class Attribute : BaseStat {
	private string _name;
	
	public Attribute() {
		ExpToLevel = 50;
		LevelModifier = 1.05f;
		_name = "";
	}
	
	public string Name {
		get { return _name; }
		set { _name = value;}
	}
}

public enum AttributeName {
	Might,
	Constitution,
	Nimbleness,
	Speed,
	Concentration,
	WillPower,
	Charisma
}