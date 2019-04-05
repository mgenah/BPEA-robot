package il.ac.bgu.cs.bp.bpjsrobot;

import java.io.Serializable;
import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.locks.ReadWriteLock;
import java.util.concurrent.locks.ReentrantReadWriteLock;
import java.util.function.Consumer;
import java.util.function.Supplier;

import il.ac.bgu.cs.bp.bpjsrobot.events.sensors.ScannedRobot;
import il.ac.bgu.cs.bp.bpjsrobot.events.sensors.Status;
import il.ac.bgu.cs.bp.bpjsrobot.mathexpressionevaluator.ExpressionEvaluator;
import robocode.RobotStatus;
import robocode.ScannedRobotEvent;

public class RobotState implements Serializable {
	private ReadWriteLock lock = new ReentrantReadWriteLock();
	private double energy = 100;
	private double x;
	private double y;
	private double velocity;
	private double bodyTurnRemaining;
	private double radarTurnRemaining;
	private double gunTurnRemaining;
	private double distanceRemaining;
	private double gunHeat;
	
	private double enemyEnergy;
	private double enemyVelocity;
	private double distanceToEnemy;
	
	private double fire;
//	private PrintWriter anOut;
	
//	public int roundNum;
//	public int numRounds;
	
	public RobotState(){
//		File logFile = new File("c:\\temp\\robocodeRun.log");
//		try {
//			anOut = new PrintWriter(logFile);
//		} catch (FileNotFoundException e) {
//			e.printStackTrace();
//		}
	}
	
	public double getEnergy() {
		return read(()->energy);
	}

	public void setEnergy(double energy) {
		set((val)->this.energy=val, energy);
	}

	public double getX() {
		return read(()->x);
	}

	public void setX(double x) {
		set((val)->this.x=val, x);
	}

	public double getY() {
		return read(()->y);
	}

	public void setY(double y) {
		set((val)->this.y=val, y);
	}

	public double getVelocity() {
		return read(()->velocity);
	}

	public void setVelocity(double velocity) {
		set((val)->this.velocity=val, velocity);
	}

	public double getBodyTurnRemaining() {
		return read(()->bodyTurnRemaining);
	}

	public void setBodyTurnRemaining(double bodyTurnRemaining) {
		set((val)->this.bodyTurnRemaining=val, bodyTurnRemaining);
	}

	public double getRadarTurnRemaining() {
		return read(()-> radarTurnRemaining);
	}

	public void setRadarTurnRemaining(double radarTurnRemaining) {
		set((val)->this.radarTurnRemaining=val, radarTurnRemaining);
	}

	public double getGunTurnRemaining() {
		return read(()->gunTurnRemaining);
	}

	public void setGunTurnRemaining(double gunTurnRemaining) {
		set((val)->this.gunTurnRemaining=val, gunTurnRemaining);
	}

	public double getDistanceRemaining() {
		return read(()->distanceRemaining);
	}

	public void setDistanceRemaining(double distanceRemaining) {
		set((val)->this.distanceRemaining=val, distanceRemaining);
	}

	public double getGunHeat() {
		return read(()->gunHeat);
	}

	public void setGunHeat(double gunHeat) {
		set((val)->this.gunHeat=val, gunHeat);
	}

	public double getEnemyEnergy() {
		return read(()->enemyEnergy);
	}

	public void setEnemyEnergy(double enemyEnergy) {
		set((val)->this.enemyEnergy=val, enemyEnergy);
	}

	public double getEnemyVelocity() {
		return read(()->enemyVelocity);
	}

	public void setEnemyVelocity(double enemyVelocity) {
		set((val)->this.enemyVelocity=val, enemyVelocity);
	}

	public double getDistanceToEnemy() {
		return read(()->distanceToEnemy);
	}

	public void setDistanceToEnemy(double distanceToEnemy) {
		set((val)->this.distanceToEnemy=val, distanceToEnemy);
	}

	public double getFire() {
		return read(()->fire);
	}

	public void setFire(double fire) {
		set((val)->this.fire=val, fire);
	}
	
	private <T> T read(Supplier<T> getter){
//		anOut.println("getting a field");
		lock.readLock().lock();
		try{
			return getter.get();
		} finally {
			lock.readLock().unlock();
		}
	}
	
	private <T> void set(Consumer<T> setter, T value){
//		anOut.println("setting a field");
		lock.writeLock().lock();
		try{
			setter.accept(value);
		} finally {
			lock.writeLock().unlock();
		}
	}

	public void update(ScannedRobot scannedRobot){
//		anOut.println("update on scanned robot");
		lock.writeLock().lock();
		try{
			RobotStatus status = scannedRobot.getStatus().getStatus();
			energy = status.getEnergy();
			x = status.getX();
			y = status.getY();
			velocity = status.getVelocity();
			bodyTurnRemaining = status.getTurnRemaining();
			radarTurnRemaining = status.getRadarTurnRemaining();
			gunTurnRemaining = status.getGunTurnRemaining();
			distanceRemaining = status.getDistanceRemaining();
			gunHeat = status.getGunHeat();
			
			ScannedRobotEvent enemyInfo = scannedRobot.getData();
			enemyEnergy = enemyInfo.getEnergy();
			enemyVelocity = enemyInfo.getVelocity();
			distanceToEnemy = enemyInfo.getDistance();
		} finally {
			lock.writeLock().unlock();
		}
	}
	
	public void updateStatus(Status robotStatus){
//		anOut.println("update robot's status");
		lock.writeLock().lock();
		try{
			RobotStatus status = robotStatus.getStatus();
			energy = status.getEnergy();
			x = status.getX();
			y = status.getY();
			velocity = status.getVelocity();
			bodyTurnRemaining = status.getTurnRemaining();
			radarTurnRemaining = status.getRadarTurnRemaining();
			gunTurnRemaining = status.getGunTurnRemaining();
			distanceRemaining = status.getDistanceRemaining();
			gunHeat = status.getGunHeat();
		} finally {
			lock.writeLock().unlock();
		}
	}
	
	public double grade(String featureBasedPolicy){
//		anOut.println("grading robot's state");
		lock.readLock().lock();
		try{
			Map<String, Double> variables = new HashMap<>();
			variables.put("energy", energy);
			variables.put("velocity", velocity);
			variables.put("gunHeat", gunHeat);
			
			variables.put("bodyTurnRemaining", bodyTurnRemaining);
			variables.put("radarTurnRemaining", radarTurnRemaining);
			variables.put("gunTurnRemaining", gunTurnRemaining);
			variables.put("distanceRemaining", distanceRemaining);
			
			variables.put("enemyEnergy", enemyEnergy);
			variables.put("enemyVelocity", enemyVelocity);
			variables.put("distanceToEnemy", distanceToEnemy);
	
			return ExpressionEvaluator.evaluate(featureBasedPolicy, variables);
		} finally {
			lock.readLock().unlock();
		}
	}
	
	public RobotState clone() {
//		anOut.println("clonning the state");
		lock.readLock().lock();
		try{	
			RobotState clone = new RobotState();
			clone.energy = energy;
			clone.x = x;
			clone.y = y;
			clone.velocity = velocity;
			clone.bodyTurnRemaining = bodyTurnRemaining;
			clone.radarTurnRemaining = radarTurnRemaining;
			clone.gunTurnRemaining = gunTurnRemaining;
			clone.distanceRemaining = distanceRemaining;
			clone.gunHeat = gunHeat;
			clone.enemyEnergy = enemyEnergy;
			clone.enemyVelocity = enemyVelocity;
			clone.distanceToEnemy = distanceToEnemy;
			return clone;
		} finally {
			lock.readLock().unlock();
		}
	}

	public void update(RobotState clonedState) {
//		anOut.println("update the entire state");
		lock.writeLock().lock();
		try{
			energy = clonedState.getEnergy();
			x = clonedState.getX();
			y = clonedState.getY();
			velocity = clonedState.getVelocity();
			bodyTurnRemaining = clonedState.getBodyTurnRemaining();
			radarTurnRemaining = clonedState.getRadarTurnRemaining();
			gunTurnRemaining = clonedState.getGunTurnRemaining();
			distanceRemaining = clonedState.getDistanceRemaining();
			gunHeat = clonedState.getGunHeat();
			fire = clonedState.getFire();
			enemyEnergy = clonedState.getEnergy();
			enemyVelocity = clonedState.getVelocity();
			distanceToEnemy = clonedState.getDistanceToEnemy();
		} finally {
			lock.writeLock().unlock();
		}
	}

	@Override
	public String toString() {
		return "RobotState [lock=" + lock + ", energy=" + energy + ", x=" + x + ", y=" + y + ", velocity=" + velocity
				+ ", bodyTurnRemaining=" + bodyTurnRemaining + ", radarTurnRemaining=" + radarTurnRemaining
				+ ", gunTurnRemaining=" + gunTurnRemaining + ", distanceRemaining=" + distanceRemaining + ", gunHeat="
				+ gunHeat + ", enemyEnergy=" + enemyEnergy + ", enemyVelocity=" + enemyVelocity + ", distanceToEnemy="
				+ distanceToEnemy + ", fire=" + fire + "]";
	}
	
}
