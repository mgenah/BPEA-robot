package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class TurnRadarLeft extends RobotActionEvent {
	int angle;

	public TurnRadarLeft(int angle) {
		super("TurnRadarLeft");
		this.angle = angle;
	}

	@Override
	public void act(BPjsRobot robot) {
		System.out.println("Turning radar left");
		robot.setTurnRadarLeft(angle);
	}

	@Override
	public String toString() {
		return "TurnRadarLeft [angle=" + angle + "]";
	}

}
