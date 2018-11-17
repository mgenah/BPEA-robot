package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class SetAdjustRadarForRobotTurn extends RobotActionEvent {
	boolean independent;

	public SetAdjustRadarForRobotTurn(boolean independent) {
		super("SetAdjustRadarForRobotTurn");
		this.independent = independent;
	}

	@Override
	public void act(BPjsRobot robot) {
		robot.setAdjustRadarForRobotTurn(independent);
	}

	@Override
	public String toString() {
		return "SetAdjustRadarForRobotTurn [independent=" + independent + "]";
	}

}