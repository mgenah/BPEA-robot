package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class TurnRightRadians extends RobotActionEvent {
	private double radians;

	public TurnRightRadians(double radians) {
		super("TurnRightRadians");
		this.radians = radians;
	}

	@Override
	public void act(BPjsRobot robot) {
		robot.setTurnRightRadians(radians);
	}

	@Override
	public String toString() {
		return "TurnRightRadians [angle=" + radians + "]";
	}
}
