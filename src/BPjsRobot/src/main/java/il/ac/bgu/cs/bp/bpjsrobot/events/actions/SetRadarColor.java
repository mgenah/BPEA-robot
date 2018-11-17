package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import java.awt.Color;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class SetRadarColor extends RobotActionEvent {
	private Color color;

	public SetRadarColor(Color color) {
		super("SetRadarColor");
		this.color = color;
	}

	@Override
	public void act(BPjsRobot robot) {
		robot.setRadarColor(color);
	}

	@Override
	public String toString() {
		return "SetRadarColor [color=" + color + "]";
	}
}