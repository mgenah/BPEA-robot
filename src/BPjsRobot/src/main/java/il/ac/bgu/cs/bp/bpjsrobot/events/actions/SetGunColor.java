package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import java.awt.Color;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class SetGunColor extends RobotActionEvent {
	private Color color;

	public SetGunColor(Color color) {
		super("SetGunColor");
		this.color = color;
	}

	@Override
	public void act(BPjsRobot robot) {
		robot.setGunColor(color);
	}

	@Override
	public String toString() {
		return "SetGunColor [color=" + color + "]";
	}
}