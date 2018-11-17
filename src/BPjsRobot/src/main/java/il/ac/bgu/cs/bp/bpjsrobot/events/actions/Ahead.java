package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class Ahead extends RobotActionEvent {
	private final int distance;
	
	public Ahead(int distance) {
		super("Ahead");
		this.distance = distance;
	}

	@Override
	public void act(BPjsRobot robot) {
		robot.setAhead(distance);
	}
	
	public int getDistance() {
		return distance;
	}

	@Override
	public String toString() {
		return "Ahead [distance=" + distance + "]";
	}
}