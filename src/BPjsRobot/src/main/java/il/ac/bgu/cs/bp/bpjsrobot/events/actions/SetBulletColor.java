package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import java.awt.Color;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class SetBulletColor extends RobotActionEvent {
	private Color color;

	public SetBulletColor(Color color) {
		super("SetBulletColor");
		this.color = color;
	}

	@Override
	public void act(BPjsRobot robot) {
		robot.setBulletColor(color);
	}

	@Override
	public String toString() {
		return "SetBulletColor [color=" + color + "]";
	}
}