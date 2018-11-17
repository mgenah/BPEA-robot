package il.ac.bgu.cs.bp.bpjsrobot.features;

public enum RobocodeFeatureName {
	DistanceFromEnemy("");
	
	private String value;

	RobocodeFeatureName(String stringValue){
		this.value = stringValue;
	}
	
	public String getStringValue(){
		return value;
	}
}
