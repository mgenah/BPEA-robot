package il.ac.bgu.cs.bp.bpjsrobot;

import java.io.File;
import java.io.FileFilter;
import java.io.FileNotFoundException;
import java.io.Serializable;
import java.util.Scanner;
import java.util.Vector;

import il.ac.bgu.cs.bp.bpjs.execution.BProgramRunner;
import il.ac.bgu.cs.bp.bpjs.model.BProgram;
import il.ac.bgu.cs.bp.bpjs.model.StringBProgram;
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
import robocode.control.RobocodeEngine;
import robocode.util.Utils;

public class BPjsRobot extends AdvancedRobot implements Serializable {
	private transient BProgram bprog;
	public transient AntiGravity antiGravity;
	public transient WallSmoothing wallSmooth;
	public transient LinearTargeting linearTargeting;
	public static RobotState uniqueRobotState = new RobotState();
	private transient StatusEvent lastStatus = null;
	
	public BPjsRobot(){
		super();
		antiGravity = new AntiGravity();
		wallSmooth = new WallSmoothing();
		linearTargeting = new LinearTargeting();
	}

	public static RobotState getInstance() {
		return uniqueRobotState;
	}
	
	public void run() {
		double fieldWidth = getBattleFieldWidth();
		double fieldHeight = getBattleFieldHeight();
		wallSmooth.setFieldHeight(fieldHeight);
		wallSmooth.setFieldWidth(fieldWidth);
		antiGravity.setFieldHeight(fieldHeight);
		antiGravity.setFieldWidth(fieldWidth);
		linearTargeting.setFieldHeight(fieldHeight);
		linearTargeting.setFieldWidth(fieldWidth);
		
//		File logFile = new File("c:\\temp\\robocodeRun.log");
//		PrintWriter anOut;
//		try {
//			anOut = new PrintWriter(logFile);
//		} catch (FileNotFoundException e) {
//			e.printStackTrace();
//			anOut = new PrintWriter(out);
//		}
		
		out.println("Starting to run robot");
		out.println("Robot dir: " + getDataDirectory().getAbsolutePath());
		out.flush();
		try {
			String bProgramData = getBProgramData();
			bprog = new StringBProgram(bProgramData);
		} catch (FileNotFoundException e1) {
			System.out.println("Failed to find BProgram file.");
			return;
		}

		String policy;
		try{
			policy = getFeatureBasedPolicy();
		} catch (FileNotFoundException e1) {
			System.out.println("Failed to find policy file.");
			return;
		}
		
		bprog.setEventSelectionStrategy(new FeatureBasedEventSelectionStrategy(out, policy));
		bprog.putInGlobalScope("robot", this);
//		bprog.putInGlobalScope("robotState", getInstance());
		out.println("Created bprog");
		bprog.setWaitForExternalEvents(true);
		BProgramRunner runner = new BProgramRunner(bprog); 
		runner.addListener(new RobocodeEventListener(this));
		// go!
		runner.run();
		System.out.println("---- done -----");
	}
	
	private String getBProgramData() throws FileNotFoundException {
		return readFileToString("BPEARobot.js");
	}
	
	private String getFeatureBasedPolicy() throws FileNotFoundException {
		File dataDirectory = getDataDirectory();
		FileFilter fileFilter = new FileFilter(){
			@Override
			public boolean accept(File file) {
				return file.getName().startsWith("robot");
			}			
		};
		File[] listFiles = dataDirectory.listFiles(fileFilter);
		if (listFiles.length == 0){
			throw new FileNotFoundException();
		}
		return readFileToString(listFiles[0].getName());
	}
	
	private String readFileToString(String fileName) throws FileNotFoundException{
		File dataFile = getDataFile(fileName);
		System.out.println(dataFile.getAbsolutePath());
	    StringBuilder fileContents = new StringBuilder((int)dataFile.length());        

	    try (Scanner scanner = new Scanner(dataFile)) {
	        while(scanner.hasNextLine()) {
	            fileContents.append(scanner.nextLine() + System.lineSeparator());
	        }
	        return fileContents.toString();
	    }
	}
	
	@Override
	public void onStatus(StatusEvent e) {
		if (bprog != null) {
			out.println("Got status event. turn remaining: " + e.getStatus().getTurnRemaining() + " " + e.getStatus().getTurnRemainingRadians());
			bprog.enqueueExternalEvent(new Status(e));
			lastStatus = e;
		}
	}

	@Override
	public void onScannedRobot(ScannedRobotEvent e) {
		out.println("Scanned robot");
		Vector<StatusEvent> statusEvents = getStatusEvents();
		if (!statusEvents.isEmpty())
			lastStatus = statusEvents.lastElement();
//		double absBearing = e.getBearingRadians() + getHeadingRadians();
//		antiGravity.updateEnemyLocation(new Point2D.Double(getX()+e.getDistance()*Math.sin(absBearing),getY()+e.getDistance()*Math.cos(absBearing)));

		if (bprog != null)
			out.println("enqueuing Scanned robot event");
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
			bprog.setWaitForExternalEvents(false);
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
		out.println(message);
//		try(OutputStream os = new FileOutputStream(new File("c:\\temp\\robocodeRun.log"), true)){			
//			os.write(message.getBytes(), 0, message.length());
//			os.close();
//		} catch (FileNotFoundException e) {
//			// TODO Auto-generated catch block
//			e.printStackTrace();
//		} catch (IOException e) {
//			// TODO Auto-generated catch block
//			e.printStackTrace();
//		}
	}
}
