package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class TurnLeft extends RobotActionEvent {
	int angle;

	public TurnLeft(int angle) {
		super("TurnLeft");
		this.angle = angle;
	}

	@Override
	public void act(BPjsRobot robot) {
		robot.setTurnLeft(angle);
	}

	@Override
	public String toString() {
		return "TurnLeft [angle=" + angle + "]";
	}

}