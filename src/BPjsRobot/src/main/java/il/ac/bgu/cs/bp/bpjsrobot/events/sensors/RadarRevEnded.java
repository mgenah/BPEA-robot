package il.ac.bgu.cs.bp.bpjsrobot.events.sensors;

import robocode.StatusEvent;

@SuppressWarnings("serial")
public class RadarRevEnded extends RobotSensorEvent {

	public static RadarRevEnded event  = new RadarRevEnded(null);
	

	public RadarRevEnded(StatusEvent e) {
		super("RadarRevEnded");
	}

	@Override
	public String toString() {
		return "RadarRevEnded";
	}

}
