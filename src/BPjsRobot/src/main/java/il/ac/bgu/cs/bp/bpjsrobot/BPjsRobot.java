package il.ac.bgu.cs.bp.bpjsrobot;

import java.awt.geom.Point2D;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.util.Arrays;
import java.util.Vector;

import il.ac.bgu.cs.bp.bpjs.execution.BProgramRunner;
import il.ac.bgu.cs.bp.bpjs.model.SingleResourceBProgram;
import il.ac.bgu.cs.bp.bpjs.model.eventselection.LoggingEventSelectionStrategyDecorator;
import il.ac.bgu.cs.bp.bpjs.model.eventselection.SimpleEventSelectionStrategy;
import il.ac.bgu.cs.bp.bpjsrobot.events.sensors.BpHitWallEvent;
import il.ac.bgu.cs.bp.bpjsrobot.events.sensors.BpRobotDeathEvent;
import il.ac.bgu.cs.bp.bpjsrobot.events.sensors.ScannedRobot;
import il.ac.bgu.cs.bp.bpjsrobot.events.sensors.Status;
import robocode.AdvancedRobot;
import robocode.BattleEndedEvent;
import robocode.HitWallEvent;
import robocode.RobotDeathEvent;
import robocode.ScannedRobotEvent;
import robocode.StatusEvent;
import robocode.WinEvent;
import robocode.util.Utils;

public class BPjsRobot extends AdvancedRobot {
	SingleResourceBProgram bprog;
	public AntiGravity antiGravity;
	public WallSmoothing wallSmooth;
	public LinearTargeting linearTargeting;
	
	public BPjsRobot(){
		super();
		antiGravity = new AntiGravity();
		wallSmooth = new WallSmoothing();
//		linearTargeting = new LinearTargeting();
	}

	public void run() {
		double fieldWidth = getBattleFieldWidth();
		double fieldHeight = getBattleFieldHeight();
		wallSmooth.setFieldHeight(fieldHeight);
		wallSmooth.setFieldWidth(fieldWidth);
		antiGravity.setFieldHeight(fieldHeight);
		antiGravity.setFieldWidth(fieldWidth);
//		linearTargeting.setFieldHeight(fieldHeight);
//		linearTargeting.setFieldWidth(fieldWidth);		
		
		out.println("Starting to run robot");
		bprog = new SingleResourceBProgram("BPEARobot.js");
		File logFile = new File("c:\\temp\\robocodeRun.log");
		PrintWriter anOut;
		try {
			anOut = new PrintWriter(logFile);
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			anOut = new PrintWriter(out);
		}
		bprog.setEventSelectionStrategy(/*new SimpleEventSelectionStrategy()*/new LoggingEventSelectionStrategyDecorator(new FeatureBasedEventSelectionStrategy(Arrays.asList("fire"), "fire*5")/*new SimpleEventSelectionStrategy()*/, anOut));
		bprog.putInGlobalScope("robot", this);
		out.println("Created bprog");
		bprog.setDaemonMode(true);
		BProgramRunner runner = new BProgramRunner(bprog); 
		runner.addListener(new RobocodeEventListener(this));
		// go!
		runner.run();
		System.out.println("---- done -----");
	}
	
//	@Override
//	public void execute() {
//		out.println("starting to execute");
//		super.execute();
//		out.println("finished executing");
//	}
	
	@Override
	public void onStatus(StatusEvent e) {
		if (bprog != null)
			bprog.enqueueExternalEvent(new Status(e));
	}

	@Override
	public void onScannedRobot(ScannedRobotEvent e) {
		System.out.println("Scanned robot");
		Vector<StatusEvent> statusEvents = getStatusEvents();
		StatusEvent lastStatus = null;
		if (!statusEvents.isEmpty())
			lastStatus = statusEvents.lastElement();
//		double absBearing = e.getBearingRadians() + getHeadingRadians();
//		antiGravity.updateEnemyLocation(new Point2D.Double(getX()+e.getDistance()*Math.sin(absBearing),getY()+e.getDistance()*Math.cos(absBearing)));

		if (bprog != null)
			bprog.enqueueExternalEvent(new ScannedRobot(e, lastStatus));
	}
	
	@Override
	public void onHitWall(HitWallEvent e) {
		if (bprog != null)
			bprog.enqueueExternalEvent(new BpHitWallEvent(e));
	}
	
	@Override
	public void onRobotDeath(RobotDeathEvent e){
		if (bprog != null)
			bprog.enqueueExternalEvent(new BpRobotDeathEvent(e));
	}
	
	@Override
	public void onWin(WinEvent e){
	}
	
	@Override
	public void onBattleEnded(BattleEndedEvent e){
		if (bprog != null)
			bprog.setDaemonMode(false);
	}
	
	public DecidedActions goTo(double x, double y) {
		/* Transform our coordinates into a vector */
		x -= getX();
		y -= getY();
	 
		/* Calculate the angle to the target position */
		double angleToTarget = Math.atan2(x, y);
	 
		/* Calculate the turn required get there */
		double targetAngle = Utils.normalRelativeAngle(angleToTarget - getHeadingRadians());
	 
		/* 
		 * The Java Hypot method is a quick way of getting the length
		 * of a vector. Which in this case is also the distance between
		 * our robot and the target location.
		 */
		double distance = Math.hypot(x, y);
		
		DecidedActions actions = new DecidedActions();
	 
		/* This is a simple method of performing set front as back */
		double turnAngle = Math.atan(Math.tan(targetAngle));
		actions.TurnRightRadians = turnAngle;
		if(targetAngle == turnAngle) {
			actions.Ahead = distance;
		} else {
			actions.Ahead = -distance;
		}
		return actions;
	}
	
	public void log(String message){
		try(OutputStream os = new FileOutputStream(new File("c:\\temp\\robocodeRun.log"), true)){			
			os.write(message.getBytes(), 0, message.length());
			os.close();
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
}
