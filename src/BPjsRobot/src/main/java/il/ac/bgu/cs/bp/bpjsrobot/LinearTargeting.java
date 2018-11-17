package il.ac.bgu.cs.bp.bpjsrobot;

import il.ac.bgu.cs.bp.bpjsrobot.events.sensors.ScannedRobot;
import robocode.Rules;
import robocode.ScannedRobotEvent;

public class LinearTargeting {
	private double fieldWidth;
	private double fieldHeight;
	
	public void setFieldWidth(double fieldWidth) {
		this.fieldWidth = fieldWidth;
	}
	
	public void setFieldHeight(double fieldHeight) {
		this.fieldHeight = fieldHeight;
	}
	
	public DecidedActions onScannedRobot(double currX, double currY, double headingRadians, double gunHeadingRadians, ScannedRobot scannedRobotEvent) {
		ScannedRobotEvent event = scannedRobotEvent.getData();
		
	    final double FIREPOWER = 2;
	    final double ROBOT_WIDTH = 16,ROBOT_HEIGHT = 16;
	    // Variables prefixed with e- refer to enemy, b- refer to bullet and r- refer to robot	    
	    final double eAbsBearing = headingRadians + event.getBearingRadians();
	    final double rX = currX, rY = currY, bV = Rules.getBulletSpeed(FIREPOWER);
	    final double eX = rX + event.getDistance()*Math.sin(eAbsBearing),
	        eY = rY + event.getDistance()*Math.cos(eAbsBearing),
	        eV = event.getVelocity(),
	        eHd = event.getHeadingRadians();
	    // These constants make calculating the quadratic coefficients below easier
	    final double A = (eX - rX)/bV;
	    final double B = eV/bV*Math.sin(eHd);
	    final double C = (eY - rY)/bV;
	    final double D = eV/bV*Math.cos(eHd);
	    // Quadratic coefficients: a*(1/t)^2 + b*(1/t) + c = 0
	    final double a = A*A + C*C;
	    final double b = 2*(A*B + C*D);
	    final double c = (B*B + D*D - 1);
	    final double discrim = b*b - 4*a*c;
	    DecidedActions actions = new DecidedActions();
	    if (discrim >= 0) {
	        // Reciprocal of quadratic formula
	        final double t1 = 2*a/(-b - Math.sqrt(discrim));
	        final double t2 = 2*a/(-b + Math.sqrt(discrim));
	        final double t = Math.min(t1, t2) >= 0 ? Math.min(t1, t2) : Math.max(t1, t2);
	        // Assume enemy stops at walls
	        final double endX = limit(eX + eV*t*Math.sin(eHd), ROBOT_WIDTH/2, fieldWidth - ROBOT_WIDTH/2);
	        final double endY = limit(eY + eV*t*Math.cos(eHd), ROBOT_HEIGHT/2, fieldHeight - ROBOT_HEIGHT/2);
	        actions.TurnGunRightRadians = robocode.util.Utils.normalRelativeAngle(Math.atan2(endX - rX, endY - rY) - gunHeadingRadians);
	        actions.Fire = FIREPOWER;
	    }
	    return actions;
	}
	 
	private double limit(double value, double min, double max) {
	    return Math.min(max, Math.max(min, value));
	}
}
