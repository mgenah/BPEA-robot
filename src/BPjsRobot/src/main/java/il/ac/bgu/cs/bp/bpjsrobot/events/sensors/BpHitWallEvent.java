package il.ac.bgu.cs.bp.bpjsrobot.events.sensors;

import robocode.HitWallEvent;

@SuppressWarnings("serial")
public class BpHitWallEvent extends RobotSensorEvent {
	private HitWallEvent event;

	public BpHitWallEvent(HitWallEvent event) {
		super("BpHitWallEvent");
		this.event = event;
	}

	@Override
	public String toString() {
		return "BpHitWallEvent [event=" + event + "]";
	}

	public HitWallEvent getData() {
		return event;
	}
}
