package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import java.awt.Color;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class SetBodyColor extends RobotActionEvent {
	private Color color;

	public SetBodyColor(Color color) {
		super("SetBodyColor");
		this.color = color;
	}

	@Override
	public void act(BPjsRobot robot) {
		robot.setBodyColor(color);
	}

	@Override
	public String toString() {
		return "SetBodyColor [color=" + color + "]";
	}
}