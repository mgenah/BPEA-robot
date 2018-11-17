package il.ac.bgu.cs.bp.bpjsrobot.events.sensors;

import robocode.RoundEndedEvent;

@SuppressWarnings("serial")
public class RoundEnded extends RobotSensorEvent {

	private RoundEndedEvent e;

	public RoundEnded(RoundEndedEvent e) {
		super("RoundEnded");
		this.e = e;
	}

	@Override
	public String toString() {
		return "RoundEndedEvent [e=" + e + "]";
	}

}
