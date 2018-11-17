package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class TurnGunRightRadians extends RobotActionEvent {
	double radians;

	public TurnGunRightRadians(double radians) {
		super("TurnGunRightRadians");
		this.radians = radians;
	}

	@Override
	public void act(BPjsRobot robot) {
		robot.setTurnGunRightRadians(radians);
	}

	@Override
	public String toString() {
		return "TurnGunRightRadians [radians=" + radians + "]";
	}
}
