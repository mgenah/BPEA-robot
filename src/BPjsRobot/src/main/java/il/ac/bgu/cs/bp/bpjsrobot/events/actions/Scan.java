package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class Scan extends RobotActionEvent {
	public Scan() {
		super("Scan");
	}

	@Override
	public void act(BPjsRobot robot) {
		robot.scan();
	}

	@Override
	public String toString() {
		return "Scan []";
	}
}