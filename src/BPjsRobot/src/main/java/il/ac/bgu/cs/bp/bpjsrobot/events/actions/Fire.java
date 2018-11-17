package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class Fire extends RobotActionEvent {
	double power;

	public Fire(double power) {
		super("Fire");
		this.power = power;
	}

	@Override
	public void act(BPjsRobot robot) {
		robot.setFire(power);
	}

	@Override
	public String toString() {
		return "Fire [power=" + power + "]";
	}

}