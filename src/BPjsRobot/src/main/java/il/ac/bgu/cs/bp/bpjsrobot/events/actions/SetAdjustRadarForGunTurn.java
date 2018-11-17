package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class SetAdjustRadarForGunTurn extends RobotActionEvent {
	boolean independent;

	public SetAdjustRadarForGunTurn(boolean independent) {
		super("SetAdjustRadarForGunTurn");
		this.independent = independent;
	}

	@Override
	public void act(BPjsRobot robot) {
		robot.setAdjustRadarForGunTurn(independent);
	}

	@Override
	public String toString() {
		return "SetAdjustRadarForGunTurn [independent=" + independent + "]";
	}

}