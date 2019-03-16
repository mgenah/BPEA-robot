importPackage(Packages.il.ac.bgu.cs.bp.bpjsrobot);
importPackage(Packages.il.ac.bgu.cs.bp.bpjsrobot.events.actions);
importPackage(Packages.il.ac.bgu.cs.bp.bpjsrobot.events.sensors);
importPackage(Packages.robocode.util);

veryFar = 9999.0;
quarterTurn = 90.0;
halfTurn = 180.0;
threeQuarterTurn = 270.0;
fullTurn = 360.0;

var targetflag=false;
var targetBear=null;
var firsttimescanned=0;
var targetIsDead=false;
var moveDirection = 1;

var direction = 1;

var Scanned = bp.EventSet('', function(e) {
	return (e instanceof ScannedRobot);
});

var BStatus = bp.EventSet('', function(e) {
	return (e instanceof Status);
});

var Motiend = bp.EventSet('', function(e) {
	return (e instanceof Status) && e.getStatus().getDistanceRemaining() == 0;
});

var Revend = bp.EventSet('', function(e) {
	return (e instanceof Status) && e.getStatus().getTurnRemaining() == 0;
});

var GunRadRevend = bp.EventSet('', function(e) {
	return (e instanceof Status) && e.getStatus().getGunTurnRemainingRadians() == 0;
});

var GunRevend = bp.EventSet('', function(e) {
	return (e instanceof Status) && e.getStatus().getGunTurnRemaining() == 0;
});

var RadRevend = bp.EventSet('', function(e) {
	if (e instanceof Status){
		bp.log.info('radar: '+e.getStatus().getRadarTurnRemaining());
	}
	return (e instanceof Status) && e.getStatus().getRadarTurnRemaining() == 0.0;
});

var HitWall = bp.EventSet('', function(e) {
	return (e instanceof BpHitWallEvent);
});

var RobotDeath = bp.EventSet('', function(e) {
	return (e instanceof BpRobotDeathEvent);
});

var MotionEvent = bp.EventSet('', function(e) {
	return (e instanceof Ahead) || (e instanceof Back) || (e instanceof Fire);
});

var NotFire = bp.EventSet('', function(e) {
	return !(e instanceof Fire);
});


bp.registerBThread("SmartRadarSpin",function() {
	while (true){
		if (targetIsDead){
			break;
		}
		
		if (targetflag==false){
		    bp.log.info('Running radar scan false');
			bp.sync({ request : TurnRadarRight(10)});
			robot.getInstance().setRadarTurnRemaining(10);
			bp.log.info('Turning radar right');
			bp.sync({ waitFor : RadRevend });
			bp.log.info('radar stopped moving');
		}
		if (targetflag==true){
			bp.log.info('Running radar scan true');
			if (firsttimescanned == 0){
				if (targetBear != null){
					bp.sync({ request : TurnRadarLeft(targetBear)});
					bp.log.info('Turning radar left ' + targetBear);
					robot.getInstance().setRadarTurnRemaining(targetBear);
					bp.sync({ waitFor : RadRevend });
					firsttimescanned=1;
				}
			}
			bp.sync({ request : TurnRadarLeft(10)});
			bp.log.info('Turning radar left ' + targetBear);
			robot.getInstance().setRadarTurnRemaining(10);
			bp.sync({ waitFor : RadRevend });
			//bp.sync({ request : TurnRadarRight(50)});
			//bp.sync({ waitFor : RadRevend });
		}
	}
});

bp.registerBThread("IsThereAny Target",function() {
	var bear=null;
	var oldbear=null;
	while(true){
		if (targetIsDead){
			targetflag = false;
			break;
		}
		var e = bp.sync({ waitFor : Scanned });
		bp.log.info('Any target b-thread: found target');
		var bear=e.getData().getBearing();
		if (bear!=null && oldbear!=bear){
			targetflag=true;
			bp.log.info('flagtrue'+targetflag);
			oldbear=bear;
		}
	}
});

bp.registerBThread("Go To Target",function() {
	while(true){
		bp.log.info('Running go for target b-thread');
		if (targetIsDead){
			break;
		}
		
		var e = bp.sync({ waitFor : Scanned });
	    targetBear=e.getData().getBearing();
		var targetDist=e.getData().getDistance();
		bp.log.info("targetBear="+targetBear);
		bp.log.info("targetDist="+targetDist);
		
		bp.sync({ request : TurnRight(targetBear)}, {features:[{name:"ram",value:0.75}]});
		bp.log.info('Requested to turn body right');
		robot.getInstance().setBodyTurnRemaining(targetBear);
		bp.log.info('Requested to turn body right - finished updating status');
		bp.sync({ waitFor : Revend }, {features:[{name:"ram",value:0.75},{name:"completeOp",value:1}]});
		bp.sync({ request : Back(10) }, {features:[{name:"ram",value:0.9}]});
		bp.log.info('Requested to go back');
		robot.getInstance().setDistanceRemaining(10);
		bp.log.info('Requested to go back - finished updating status');
		bp.sync({ waitFor : Motiend }, {features:[{name:"ram",value:0.9},{name:"completeOp",value:1}]});
		bp.sync({ request : Ahead(targetDist) }, {features:[{name:"ram",value:1}]});
		robot.getInstance().setDistanceRemaining(targetDist);
		bp.sync({ waitFor : Motiend }, {features:[{name:"ram",value:1},{name:"completeOp",value:1}]});
	}
});

bp.registerBThread("Ram target", function() {
	while(true){
		bp.log.info('Running ram target b-thread');
		if (targetIsDead){
			break;
		}
		
		var e = bp.sync({ waitFor : Scanned }, {features:[{name:"intelligence",value:1}]});
	    targetBear=e.getData().getBearing();
		var targetDist=e.getData().getDistance();
		bp.log.info("targetBear="+targetBear);
		bp.log.info("targetDist="+targetDist);
		
		bp.sync({ request : TurnRight(targetBear)}, {features:[{name:"ram",value:0.75},{name:"aim",value:0.75}]});
		robot.getInstance().setBodyTurnRemaining(targetBear);
		bp.sync({ waitFor : Revend }, {features:[{name:"ram",value:0.75},{name:"aim",value:0.75},{name:"completeOp",value:1}]});
		bp.sync({ request : Back(10) }, {features:[{name:"ram",value:0.9}]});
		robot.getInstance().setDistanceRemaining(10);
		bp.sync({ waitFor : Motiend }, {features:[{name:"ram",value:0.9},{name:"completeOp",value:1}]});
		bp.sync({ request : Ahead(targetDist) }, {features:[{name:"ram",value:1}]});
		robot.getInstance().setDistanceRemaining(targetDist);
		bp.sync({ waitFor : Motiend }, {features:[{name:"ram",value:1},{name:"completeOp",value:1}]});
	}
});

bp.registerBThread("Save energy", function() {
	while(true){
		bp.log.info('Running save energy b-thread');
		var e = bp.sync({ waitFor : Scanned }, {features:[{name:"intelligence",value:1}]});
		var energy = e.getStatus().getStatus().getEnergy();
	    if (energy< 50.0){
	    	bp.sync({ request : Stop}, {features:[{name:"power",value:1}]});
	    	robot.getInstance().setDistanceRemaining(0);
	    	robot.getInstance().setBodyTurnRemaining(0);
	    	robot.getInstance().setRadarTurnRemaining(0);
	    	robot.getInstance().setGunTurnRemaining(0);
	    }
	}
});

bp.registerBThread("Control energy", function() {
	while(true){
		bp.log.info('Running control energy b-thread');
		bp.sync({ waitFor : MotionEvent});
	}
});

bp.registerBThread("Update status", function() {
	while(true){
		bp.log.info('Running update status b-thread');
		var e = bp.sync({ waitFor : BStatus});
		robot.getInstance().updateStatus(e);
	}
});

bp.registerBThread("Update scanned", function() {
	while(true){
		bp.log.info('Running update scanned robot b-thread');
		var e = bp.sync({ waitFor : Scanned});
		robot.getInstance().update(e);
	}
});

bp.registerBThread("Not fire", function() {
	while(true){
		bp.log.info('Running not fire robot b-thread');
		var e = bp.sync({ waitFor : NotFire});
		robot.getInstance().setFire(0.0);
	}
});

bp.registerBThread("Aim Target",function() {
	while (true){
		if (targetIsDead){
			break;
		}
		
		var e = bp.sync({ waitFor : Scanned });
	    targetBear=e.getData().getBearing();
		var targetDist=e.getData().getDistance();
		bp.log.info("targetBear="+targetBear);
		bp.log.info("targetDist="+targetDist);
		
		bp.sync({ request : TurnRight(targetBear)});
		robot.getInstance().setBodyTurnRemaining(targetBear);
		bp.sync({ waitFor : Revend });
	}
});

bp.registerBThread(function() {
	bp.log.info('Running fire-on-scan b-thread');
	while (true) {
		if (targetIsDead){
			break;
		}
		var e = bp.sync({ waitFor : Scanned });
		bp.log.info('Continuing fire-on-scan b-thread');
		var energy = e.getData().getEnergy();
		bp.sync({ request : Fire(50.0) });
		var currentFire = robot.getInstance().getFire();
		robot.getInstance().setFire(currentFire+50.0);
	}
});

bp.registerBThread("Wall Smoothing", function() {
	bp.log.info('Running wall smoothing');
	while (true) {
		if (targetIsDead){
			break;
		}
		var e = bp.sync({ waitFor : Scanned });
		bp.log.info('Continuing wall smoothing b-thread');
		
		var angleToEnemy = e.getData().getBearing();
        // Calculate the angle to the scanned robot
        var angle = robot.getHeading() + angleToEnemy % 360;
        var angleRad = angle * Math.PI / 180;

        // Calculate the coordinates of the robot
        var enemyX = (robot.getX() + Math.sin(angle) * e.getData().getDistance());
        var enemyY = (robot.getY() + Math.cos(angle) * e.getData().getDistance());
        var targetPoint = robot.wallSmooth.calc(enemyX, enemyY, robot.getX(), robot.getY(), robot.getHeading());
		var actions = robot.goTo(targetPoint.getX(), targetPoint.getY());
		if (actions.Ahead != 0){
			bp.sync({ request : Ahead(actions.Ahead) }, {features:[{name:"avoidHit",value:1}]});
			robot.getInstance().setDistanceRemaining(actions.Ahead);
			bp.sync({ waitFor : Motiend }, {features:[{name:"avoidHit",value:1},{name:"completeOp",value:1}]});
		}
		if (actions.TurnRightRadians != 0){
			bp.sync({ request : TurnRightRadians(actions.TurnRightRadians) }, {features:[{name:"avoidHit",value:1}]});
			robot.getInstance().setBodyTurnRemaining(actions.TurnRightRadians);
			bp.sync({ waitFor : Motiend }, {features:[{name:"avoidHit",value:1},{name:"completeOp",value:1}]});
		}
	}
});

/*
bp.registerBThread("Anti-gravity", function() {
	bp.log.info('Running Anti-gravity');
	while (true) {
		if (targetIsDead){
			break;
		}
		bp.sync({ waitFor : Scanned });
		
		var actions = robot.antiGravity.calc(robot.getX(), robot.getY(), robot.getHeadingRadians());
		if (actions.Ahead != 0){
			bp.sync({ request : Ahead(actions.Ahead) });
		}
		if (actions.TurnRightRadians != 0){
			bp.sync({ request : TurnRightRadians(actions.TurnRightRadians) });
		}
	}
});
/*
bp.registerBThread("Linear Targeting", function() {
	bp.log.info('Running Linear targeting');
	while (true) {
		if (targetIsDead){
			break;
		}
		var e = bp.sync({ waitFor : Scanned });
		var actions = robot.linearTargeting.calc(robot.getX(), robot.getY(), robot.getHeadingRadians(), robot.getGunHeadingRadians(), e);
		if (actions.Fire != 0){
			bp.sync({ request : Fire(actions.Fire) });
		}
		if (actions.TurnGunRightRadians != 0){
			bp.sync({ request : TurnGunRightRadians(actions.TurnGunRightRadians) });
		}
	}
});
*/

bp.registerBThread("identify enemy death", function() {
	bp.log.info('Running identify enemy death b-thread');
	bp.sync({ waitFor : RobotDeath }, {features:[{name:"intelligence",value:1},{name:"power",value:0.8}]});
	bp.log.info('Continuing identify enemy death b-thread');
	targetIsDead = true;
});