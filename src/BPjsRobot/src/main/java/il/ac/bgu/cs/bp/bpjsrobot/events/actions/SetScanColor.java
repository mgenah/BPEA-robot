package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import java.awt.Color;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class SetScanColor extends RobotActionEvent {
	private Color color;

	public SetScanColor(Color color) {
		super("SetScanColor");
		this.color = color;
	}

	@Override
	public void act(BPjsRobot robot) {
		robot.setScanColor(color);
	}

	@Override
	public String toString() {
		return "setScanColor [color=" + color + "]";
	}
}