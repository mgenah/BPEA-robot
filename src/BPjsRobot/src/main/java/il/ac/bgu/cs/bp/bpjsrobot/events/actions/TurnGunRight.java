package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class TurnGunRight extends RobotActionEvent {
	int angle;

	public TurnGunRight(int angle) {
		super("TurnGunRight");
		this.angle = angle;
	}

	@Override
	public void act(BPjsRobot robot) {
		robot.setTurnGunRight(angle);
	}

	@Override
	public String toString() {
		return "TurnGunRight [angle=" + angle + "]";
	}

}