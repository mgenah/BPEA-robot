package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import java.awt.Color;

import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public class SetAllColors extends RobotActionEvent {
	private Color color;

	public SetAllColors(Color color) {
		super("SetAllColors");
		this.color = color;
	}

	@Override
	public void act(BPjsRobot robot) {
		robot.setAllColors(color);
	}

	@Override
	public String toString() {
		return "SetAllColors [color=" + color + "]";
	}
}