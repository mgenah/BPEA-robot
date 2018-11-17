package il.ac.bgu.cs.bp.bpjsrobot.events.sensors;

import robocode.RobotStatus;
import robocode.StatusEvent;

@SuppressWarnings("serial")
public class Status extends RobotSensorEvent {

	private StatusEvent e;

	public Status(StatusEvent e) {
		super("Status");
		this.e = e;
	}

	@Override
	public String toString() {
		return "Status [e=" + e + "]";
	}

	/**
	 * @return the e
	 */
	public RobotStatus getStatus() {
		return e.getStatus();
	}

}
