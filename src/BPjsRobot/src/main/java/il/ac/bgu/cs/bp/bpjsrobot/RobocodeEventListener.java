package il.ac.bgu.cs.bp.bpjsrobot;

import il.ac.bgu.cs.bp.bpjs.model.BEvent;
import il.ac.bgu.cs.bp.bpjs.model.BProgram;
import il.ac.bgu.cs.bp.bpjs.model.BThreadSyncSnapshot;
import il.ac.bgu.cs.bp.bpjs.model.FailedAssertion;
import il.ac.bgu.cs.bp.bpjs.execution.listeners.BProgramRunnerListener;
import il.ac.bgu.cs.bp.bpjsrobot.events.actions.RobotActionEvent;
import il.ac.bgu.cs.bp.bpjsrobot.events.sensors.RobotSensorEvent;

public class RobocodeEventListener implements BProgramRunnerListener {
	private BPjsRobot robot;

	public RobocodeEventListener(BPjsRobot bPjsRobot) {
		this.robot = bPjsRobot;
	}

	@Override
	public void started(BProgram bp) {
		robot.log("  -:" + bp.getName() + " Started");
	}

	@Override
	public void bthreadAdded(BProgram bp, BThreadSyncSnapshot theBThread) {
		robot.log("  -:" + bp.getName() + " Added " + theBThread.getName());
	}

	@Override
	public void bthreadRemoved(BProgram bp, BThreadSyncSnapshot theBThread) {
		robot.log("  -:" + bp.getName() + " Removed " + theBThread.getName());
	}

	@Override
	public void eventSelected(BProgram bp, BEvent theEvent) {
		robot.log("  -:" + bp.getName() + " selected event " + theEvent.getName());
		if (theEvent instanceof RobotActionEvent) {
			((RobotActionEvent) theEvent).act(robot);
		} else if (theEvent instanceof RobotSensorEvent) {
		} else {
			robot.out.println("Uhandled BPjs event:" + theEvent);
		}
	}

	@Override
	public void superstepDone(BProgram bp) {
		robot.log("before time: "+robot.getTime());
		robot.log("radar remaining: " + BPjsRobot.getInstance().getRadarTurnRemaining());
		robot.execute();
		robot.log("time: "+robot.getTime());
	}

	@Override
	public void ended(BProgram bp) {
	}

	@Override
	public void starting(BProgram bprog) {
		robot.log("started: "+robot.getTime());
	}

	@Override
	public void bthreadDone(BProgram bp, BThreadSyncSnapshot theBThread) {
	}

	@Override
	public void assertionFailed(BProgram arg0, FailedAssertion arg1) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void halted(BProgram bp) {
		// TODO Auto-generated method stub
		
	}
}