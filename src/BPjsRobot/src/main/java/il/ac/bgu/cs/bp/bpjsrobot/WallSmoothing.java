package il.ac.bgu.cs.bp.bpjsrobot;

import java.awt.geom.Point2D;
import java.io.Serializable;

import robocode.util.Utils;

public final class WallSmoothing implements Serializable {
	private double fieldWidth;
	private double fieldHeight;

	public void setFieldWidth(double fieldWidth) {
		this.fieldWidth = fieldWidth;
	}

	public void setFieldHeight(double fieldHeight) {
		this.fieldHeight = fieldHeight;
	}

	public Point2D.Double calc(double enemyX, double enemyY, double currX, double currY, double direction) {
		final double MARGIN = 18;
		final double STICK_LENGTH = 150;

		Point2D.Double orbitCenter = new Point2D.Double(enemyX, enemyY);
		Point2D.Double position = new Point2D.Double(currX, currY);

		double distanceToOrbitCenter = position.distance(orbitCenter);
		double stick = Math.min(STICK_LENGTH, distanceToOrbitCenter);
		double stickSquared = square(stick);

		int LEFT = -1, RIGHT = 1, TOP = 1, BOTTOM = -1;

		int topOrBottomWall = 0;
		int leftOrRightWall = 0;

		double desiredAngle = Utils
				.normalAbsoluteAngle(absoluteAngle(position, orbitCenter) - direction * Math.PI / 2.0);
		Point2D.Double projected = projectPoint(position, desiredAngle, stick);
		if (projected.x >= 18 && projected.x <= fieldWidth - 18 && projected.y >= 18 && projected.y <= fieldHeight - 18)
			return projected;

		if (projected.x > fieldWidth - MARGIN || position.x > fieldWidth - stick - MARGIN)
			leftOrRightWall = RIGHT;
		else if (projected.x < MARGIN || position.x < stick + MARGIN)
			leftOrRightWall = LEFT;

		if (projected.y > fieldHeight - MARGIN || position.y > fieldHeight - stick - MARGIN)
			topOrBottomWall = TOP;
		else if (projected.y < MARGIN || position.y < stick + MARGIN)
			topOrBottomWall = BOTTOM;

		if (topOrBottomWall == TOP) {
			if (leftOrRightWall == LEFT) {
				if (direction > 0)
					// smooth against top wall
					return new Point2D.Double(
							position.x
									+ direction * Math.sqrt(stickSquared - square(fieldHeight - MARGIN - position.y)),
							fieldHeight - MARGIN);
				else
					// smooth against left wall
					return new Point2D.Double(MARGIN,
							position.y + direction * Math.sqrt(stickSquared - square(position.x - MARGIN)));

			} else if (leftOrRightWall == RIGHT) {
				if (direction > 0)
					// smooth against right wall
					return new Point2D.Double(fieldWidth - MARGIN, position.y
							- direction * Math.sqrt(stickSquared - square(fieldWidth - MARGIN - position.x)));
				else
					// smooth against top wall
					return new Point2D.Double(
							position.x
									+ direction * Math.sqrt(stickSquared - square(fieldHeight - MARGIN - position.y)),
							fieldHeight - MARGIN);

			}
			// Smooth against top wall
			return new Point2D.Double(
					position.x + direction * Math.sqrt(stickSquared - square(fieldHeight - MARGIN - position.y)),
					fieldHeight - MARGIN);
		} else if (topOrBottomWall == BOTTOM) {
			if (leftOrRightWall == LEFT) {
				if (direction > 0)
					// smooth against left wall
					return new Point2D.Double(MARGIN,
							position.y + direction * Math.sqrt(stickSquared - square(position.x - MARGIN)));
				else
					// smooth against bottom wall
					return new Point2D.Double(
							position.x - direction * Math.sqrt(stickSquared - square(position.y - MARGIN)), MARGIN);
			} else if (leftOrRightWall == RIGHT) {
				if (direction > 0)
					// smooth against bottom wall
					return new Point2D.Double(
							position.x - direction * Math.sqrt(stickSquared - square(position.y - MARGIN)), MARGIN);
				else
					// smooth against right wall
					return new Point2D.Double(fieldWidth - MARGIN, position.y
							- direction * Math.sqrt(stickSquared - square(fieldWidth - MARGIN - position.x)));

			}
			// Smooth against bottom wall
			return new Point2D.Double(position.x - direction * Math.sqrt(stickSquared - square(position.y - MARGIN)),
					MARGIN);
		}

		if (leftOrRightWall == LEFT) {
			// smooth against left wall
			return new Point2D.Double(MARGIN,
					position.y + direction * Math.sqrt(stickSquared - square(position.x - MARGIN)));
		} else if (leftOrRightWall == RIGHT) {
			// smooth against right wall
			return new Point2D.Double(fieldWidth - MARGIN,
					position.y - direction * Math.sqrt(stickSquared - square(fieldWidth - MARGIN - position.x)));
		}

		throw new RuntimeException("This code should be unreachable. position = " + position.x + ", " + position.y
				+ "  orbitCenter = " + orbitCenter.x + ", " + orbitCenter.y + " direction = " + direction);
	}

	public static Point2D.Double projectPoint(Point2D.Double origin, double angle, double distance) {
		return new Point2D.Double(origin.x + distance * Math.sin(angle), origin.y + distance * Math.cos(angle));
	}

	public static double absoluteAngle(Point2D.Double origin, Point2D.Double target) {
		return Math.atan2(target.x - origin.x, target.y - origin.y);
	}

	public double square(double x) {
		return x * x;
	}
}
