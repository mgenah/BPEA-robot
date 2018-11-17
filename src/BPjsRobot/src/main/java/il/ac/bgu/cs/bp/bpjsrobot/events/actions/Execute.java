package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class Execute extends RobotActionEvent {

	public Execute() {
		super("Execute");
	}

	@Override
	public void act(BPjsRobot robot) {
		robot.execute();
	}

	@Override
	public String toString() {
		return "Execute []";
	}
}