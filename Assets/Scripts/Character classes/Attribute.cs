public class Attribute : BaseStat {
	new public const int START_EXP_COST = 50;
	
	private string _name;
	
	public Attribute() {
		ExpToLevel = START_EXP_COST;
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