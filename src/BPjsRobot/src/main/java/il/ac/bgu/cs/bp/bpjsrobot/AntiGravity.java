package il.ac.bgu.cs.bp.bpjsrobot;

import java.awt.geom.Point2D;

import robocode.util.Utils;

public class AntiGravity {
	private static Point2D.Double enemyLocation;
	private int count;
	private double fieldWidth;
	private double fieldHeight;

	public void setFieldWidth(double fieldWidth) {
		this.fieldWidth = fieldWidth;
	}

	public void setFieldHeight(double fieldHeight) {
		this.fieldHeight = fieldHeight;
	}
	
	public DecidedActions calc(double currX, double currY, double headingRadians){		
		double xForce=0, yForce= 0;
	    double absBearing = Utils.normalAbsoluteAngle(Math.atan2(enemyLocation.x-currX,enemyLocation.y-currY));
	    double distance = enemyLocation.distance(currX,currY);
	    xForce -= Math.sin(absBearing) / (distance * distance);
	    yForce -= Math.cos(absBearing) / (distance * distance);
		double angle = Math.atan2(xForce, yForce);
		
		
		DecidedActions actions = new DecidedActions();
		if (xForce == 0 && yForce == 0) {
		    // If no force, do nothing
		} else if(Math.abs(angle-headingRadians)<Math.PI/2){
		    actions.TurnRightRadians = Utils.normalRelativeAngle(angle - headingRadians);
		    actions.Ahead = Math.max(fieldHeight, fieldWidth);
		} else {
		    actions.TurnRightRadians = Utils.normalRelativeAngle(angle + Math.PI - headingRadians);
		    actions.Ahead = Math.max(fieldHeight, fieldWidth);
		}
		return actions;
	}

	public void updateEnemyLocation(Point2D.Double loc) {
		enemyLocation = loc;		
	}
}
