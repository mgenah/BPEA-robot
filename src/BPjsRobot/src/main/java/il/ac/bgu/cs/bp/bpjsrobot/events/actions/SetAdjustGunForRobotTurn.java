package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class SetAdjustGunForRobotTurn extends RobotActionEvent {
	boolean independent;

	public SetAdjustGunForRobotTurn(boolean independent) {
		super("SetAdjustGunForRobotTurn");
		this.independent = independent;
	}

	@Override
	public void act(BPjsRobot robot) {
		robot.setAdjustGunForRobotTurn(independent);
	}

	@Override
	public String toString() {
		return "SetAdjustGunForRobotTurn [independent=" + independent + "]";
	}

}