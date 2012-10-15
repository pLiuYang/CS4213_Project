public class Attribute : BaseStat {
	new public const int START_EXP_COST = 50;
	
	public Attribute() {
		ExpToLevel = START_EXP_COST;
		LevelModifier = 1.05f;
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