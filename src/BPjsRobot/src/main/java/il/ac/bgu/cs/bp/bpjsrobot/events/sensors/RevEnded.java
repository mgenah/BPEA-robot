package il.ac.bgu.cs.bp.bpjsrobot.events.sensors;

import robocode.StatusEvent;

@SuppressWarnings("serial")
public class RevEnded extends RobotSensorEvent {

	public static RevEnded event  = new RevEnded(null);
	

	public RevEnded(StatusEvent e) {
		super("RevEnded");
	}

	@Override
	public String toString() {
		return "RevEnded";
	}

}
