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
	return (e instanceof Status) && e.getStatus().getRadarTurnRemaining() == 0;
});

var HitWall = bp.EventSet('', function(e) {
	return (e instanceof BpHitWallEvent);
});

var RobotDeath = bp.EventSet('', function(e) {
	return (e instanceof BpRobotDeathEvent);
});

/*bp.registerBThread("attempt", function() {
	while (true){
		bp.log.info('Requesting to go ahead');
		bp.sync({ request : bp.Event("Ahead")});
		bp.log.info('Requested to go ahead');
	}
});
*/

bp.registerBThread("SmartRadarSpin",function() {
	while (true){
//	var i;
//	for(i=0;i<5;i++){
		if (targetIsDead){
			break;
		}
		
		if (targetflag==false){
		    bp.log.info('Running radar scan false');
			bp.sync({ request : TurnRadarRight(10)}, {name:"fire",value:42.1});
			bp.log.info('Running radar scan false 222');
			bp.sync({ waitFor : RadRevend });
			bp.log.info('radar stopped moving');
		}
		if (targetflag==true){
			bp.log.info('Running radar scan true');
			if (firsttimescanned == 0){
				if (targetBear != null){
					bp.sync({ request : TurnRadarLeft(targetBear)});
					bp.sync({ waitFor : RadRevend });
					firsttimescanned=1;
				}
			}
			bp.sync({ request : TurnRadarLeft(10)});
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
		var e = bp.sync({ waitFor : Scanned });	
	    targetBear=e.getData().getBearing();
		var targetDist=e.getData().getDistance();
		bp.log.info("targetBear="+targetBear);
		bp.log.info("targetDist="+targetDist);
		
		bp.sync({ request : TurnRight(targetBear)});
		bp.sync({ waitFor : Revend });
		bp.sync({ request : Back(10) });
		bp.sync({ waitFor : Motiend });
		bp.sync({ request : Ahead(targetDist) });
		bp.sync({ waitFor : Motiend });
	}
});

bp.registerBThread("Ram target", function() {
	while(true){
		bp.log.info('Running ram target b-thread');
		var e = bp.sync({ waitFor : Scanned });	
	    targetBear=e.getData().getBearing();
		var targetDist=e.getData().getDistance();
		bp.log.info("targetBear="+targetBear);
		bp.log.info("targetDist="+targetDist);
		
		bp.sync({ request : TurnRight(targetBear)});
		bp.sync({ waitFor : Revend });
		bp.sync({ request : Back(10) });
		bp.sync({ waitFor : Motiend });
		bp.sync({ request : Ahead(targetDist) });
		bp.sync({ waitFor : Motiend });
	}
});

bp.registerBThread("Aim Target",function() {
	var e = bp.sync({ waitFor : Scanned });	
    targetBear=e.getData().getBearing();
	var targetDist=e.getData().getDistance();
	bp.log.info("targetBear="+targetBear);
	bp.log.info("targetDist="+targetDist);
	
	bp.sync({ request : TurnRight(targetBear)});
	bp.sync({ waitFor : Revend });
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
			bp.sync({ request : Ahead(actions.Ahead) });
			bp.sync({ waitFor : Motiend });
		}
		if (actions.TurnRightRadians != 0){
			bp.sync({ request : TurnRightRadians(actions.TurnRightRadians) });
			bp.sync({ waitFor : Motiend });
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
	bp.sync({ waitFor : RobotDeath });
	bp.log.info('Continuing identify enemy death b-thread');
	targetIsDead = true;
});