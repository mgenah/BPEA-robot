package il.ac.bgu.cs.bp.bpjsrobot.features;

public class BooleanFeature extends NumberFeature {
	public BooleanFeature(String name, boolean value) {
		super(name, value ? 1 : 0);
	}
}
