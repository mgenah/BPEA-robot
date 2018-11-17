package il.ac.bgu.cs.bp.bpjsrobot.events.sensors;

import robocode.StatusEvent;

@SuppressWarnings("serial")
public class GunRevEnd extends RobotSensorEvent{
	public static GunRevEnd event  = new GunRevEnd(null);

	public GunRevEnd(StatusEvent e) {
		super("GunRevEnd");
	}

	@Override
	public String toString() {
		return "GunRevEnd";
	}
}
