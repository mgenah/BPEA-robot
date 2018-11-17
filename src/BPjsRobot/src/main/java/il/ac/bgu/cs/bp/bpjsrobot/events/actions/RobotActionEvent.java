package il.ac.bgu.cs.bp.bpjsrobot.events.actions;

import il.ac.bgu.cs.bp.bpjs.model.BEvent;
import il.ac.bgu.cs.bp.bpjsrobot.BPjsRobot;

@SuppressWarnings("serial")
public abstract class RobotActionEvent extends BEvent {
	public abstract void act(BPjsRobot robot);

	public RobotActionEvent(String aName) {
		super(aName);
	}
}