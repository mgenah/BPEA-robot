package il.ac.bgu.cs.bp.bpjsrobot.events.sensors;

import robocode.StatusEvent;

@SuppressWarnings("serial")
public class MotionEnded extends RobotSensorEvent {

	public static MotionEnded event  = new MotionEnded(null);
	

	public MotionEnded(StatusEvent e) {
		super("MotionEnded");
	}

	@Override
	public String toString() {
		return "MotionEnded";
	}

}
