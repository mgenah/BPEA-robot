package il.ac.bgu.cs.bp.bpjsrobot.events.sensors;

import robocode.RobotDeathEvent;

@SuppressWarnings("serial")
public class BpRobotDeathEvent extends RobotSensorEvent {
	private RobotDeathEvent event;

	public BpRobotDeathEvent(RobotDeathEvent event) {
		super("BpRobotDeathEvent");
		this.event = event;
	}

	@Override
	public String toString() {
		return "BpRobotDeathEvent [event=" + event + "]";
	}

	public RobotDeathEvent getData() {
		return event;
	}
}