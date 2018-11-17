package il.ac.bgu.cs.bp.bpjsrobot.features;

//@JsonTypeInfo(use = JsonTypeInfo.Id.NAME, include = JsonTypeInfo.As.PROPERTY, property = "_type")
//@JsonSubTypes({
//@JsonSubTypes.Type(value = RobocodeFeature.class, name = "RobocodeFeature")
//})
public class NumberFeature extends Feature {
	private double value;

	public NumberFeature(String name, double value) {
		super(name);
		this.value = value;
	}

	public double getValue() {
		return value;
	}
}
