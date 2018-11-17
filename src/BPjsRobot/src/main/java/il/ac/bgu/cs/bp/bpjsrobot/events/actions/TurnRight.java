package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class TurnRight extends RobotActionEvent {
	int angle;

	public TurnRight(int angle) {
		super("TurnRight");
		this.angle = angle;
	}

	@Override
	public void act(BPjsRobot robot) {
		robot.setTurnRight(angle);
	}

	@Override
	public String toString() {
		return "TurnRight [angle=" + angle + "]";
	}
}