package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class Back extends RobotActionEvent {
	private final int distance;

	public Back(int distance) {
		super("Back");
		this.distance = distance;
	}

	@Override
	public void act(BPjsRobot robot) {
		robot.setBack(distance);
	}

	public int getDistance() {
		return distance;
	}

	@Override
	public String toString() {
		return "" + "Back [distance=" + distance + "]";
	}

}