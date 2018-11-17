package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class Resume extends RobotActionEvent {
	public Resume() {
		super("Resume");
	}

	@Override
	public void act(BPjsRobot robot) {
		robot.setResume();
	}

	@Override
	public String toString() {
		return "Resume []";
	}
}