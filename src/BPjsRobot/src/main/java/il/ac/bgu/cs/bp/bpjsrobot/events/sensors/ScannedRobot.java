package il.ac.bgu.cs.bp.bpjsrobot.events.sensors;

import robocode.ScannedRobotEvent;
import robocode.StatusEvent;

@SuppressWarnings("serial")
public class ScannedRobot extends RobotSensorEvent {
	private ScannedRobotEvent event;
	private StatusEvent statusEvent;

	public ScannedRobot(ScannedRobotEvent event, StatusEvent statusEvent) {
		super("ScannedRobot");
		this.event = event;
		this.statusEvent = statusEvent;
	}

	@Override
	public String toString() {
		return "ScannedRobot [event=" + event + "]";
	}

	public ScannedRobotEvent getData() {
		return event;
	}
	
	public StatusEvent getStatus(){
		return statusEvent;
	}
}
