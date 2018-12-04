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

bp.registerBThread("SmartRadarSpin",function() {
	while (true){
		if (targetIsDead){
			break;
		}
		
		if (targetflag==false){
		    bp.log.info('Running radar scan false');
			bp.sync({ request : TurnRadarRight(10)}, {features:[{name:"intelligence",value:1.0}]});
			bp.log.info('Running radar scan false 222');
			bp.sync({ waitFor : RadRevend }, {features:[{name:"intelligence",value:1},{name:"completeOp",value:1}]});
			bp.log.info('radar stopped moving');
		}
		if (targetflag==true){
			bp.log.info('Running radar scan true');
			if (firsttimescanned == 0){
				if (targetBear != null){
					bp.sync({ request : TurnRadarLeft(targetBear)}, {features:[{name:"intelligence",value:1}]});
					bp.sync({ waitFor : RadRevend }, {features:[{name:"intelligence",value:1},{name:"completeOp",value:1}]});
					firsttimescanned=1;
				}
			}
			bp.sync({ request : TurnRadarLeft(10)}, {features:[{name:"intelligence",value:1}]});
			bp.sync({ waitFor : RadRevend }, {features:[{name:"intelligence",value:1},{name:"completeOp",value:1}]});
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
		var e = bp.sync({ waitFor : Scanned }, {features:[{name:"intelligence",value:1}]});
		
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
		var e = bp.sync({ waitFor : Scanned }, {features:[{name:"intelligence",value:1}]});	
	    targetBear=e.getData().getBearing();
		var targetDist=e.getData().getDistance();
		bp.log.info("targetBear="+targetBear);
		bp.log.info("targetDist="+targetDist);
		
		bp.sync({ request : TurnRight(targetBear)}, {features:[{name:"ram",value:0.75}]});
		bp.sync({ waitFor : Revend }, {features:[{name:"ram",value:0.75},{name:"completeOp",value:1}]});
		bp.sync({ request : Back(10) }, {features:[{name:"ram",value:0.9},{name:"power",value:0.2}]});
		bp.sync({ waitFor : Motiend }, {features:[{name:"ram",value:0.9},{name:"power",value:0.2},{name:"completeOp",value:1}]});
		bp.sync({ request : Ahead(targetDist) }, {features:[{name:"ram",value:1}]});
		bp.sync({ waitFor : Motiend }, {features:[{name:"ram",value:1},{name:"completeOp",value:1}]});
	}
});

bp.registerBThread("Ram target", function() {
	while(true){
		bp.log.info('Running ram target b-thread');
		var e = bp.sync({ waitFor : Scanned }, {features:[{name:"intelligence",value:1}]});
	    targetBear=e.getData().getBearing();
		var targetDist=e.getData().getDistance();
		bp.log.info("targetBear="+targetBear);
		bp.log.info("targetDist="+targetDist);
		
		bp.sync({ request : TurnRight(targetBear)}, {features:[{name:"ram",value:0.75},{name:"aim",value:0.75}]});
		bp.sync({ waitFor : Revend }, {features:[{name:"ram",value:0.75},{name:"aim",value:0.75},{name:"completeOp",value:1}]});
		bp.sync({ request : Back(10) }, {features:[{name:"ram",value:0.9},{name:"power",value:0.2}]});
		bp.sync({ waitFor : Motiend }, {features:[{name:"ram",value:0.9},{name:"power",value:0.2},{name:"completeOp",value:1}]});
		bp.sync({ request : Ahead(targetDist) }, {features:[{name:"ram",value:1}]});
		bp.sync({ waitFor : Motiend }, {features:[{name:"ram",value:1},{name:"completeOp",value:1}]});
	}
});

bp.registerBThread("Aim Target",function() {
	var e = bp.sync({ waitFor : Scanned }, {features:[{name:"intelligence",value:1}]});
    targetBear=e.getData().getBearing();
	var targetDist=e.getData().getDistance();
	bp.log.info("targetBear="+targetBear);
	bp.log.info("targetDist="+targetDist);
	
	bp.sync({ request : TurnRight(targetBear)}, {features:[{name:"aim",value:1.0},{name:"fire",value:0.2}]});
	bp.sync({ waitFor : Revend }, {features:[{name:"aim",value:1.0},{name:"fire",value:0.2},{name:"completeOp",value:1}]});
});

bp.registerBThread(function() {
	bp.log.info('Running fire-on-scan b-thread');
	while (true) {
		if (targetIsDead){
			break;
		}
		var e = bp.sync({ waitFor : Scanned }, {features:[{name:"intelligence",value:1}]});
		bp.log.info('Continuing fire-on-scan b-thread');
		var energy = e.getData().getEnergy();
		bp.sync({ request : Fire(50.0) }, {features:[{name:"fire",value:1},{name:"power",value:0.2}]});
	}
});

bp.registerBThread("Wall Smoothing", function() {
	bp.log.info('Running wall smoothing');
	while (true) {
		if (targetIsDead){
			break;
		}
		var e = bp.sync({ waitFor : Scanned }, {features:[{name:"intelligence",value:1}]});
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
			bp.sync({ request : Ahead(actions.Ahead) }, {features:[{name:"avoidHit",value:1},{name:"power",value:0.8}]});
			bp.sync({ waitFor : Motiend }, {features:[{name:"avoidHit",value:1},{name:"power",value:0.8},{name:"completeOp",value:1}]});
		}
		if (actions.TurnRightRadians != 0){
			bp.sync({ request : TurnRightRadians(actions.TurnRightRadians) }, {features:[{name:"avoidHit",value:1},{name:"power",value:0.8}]});
			bp.sync({ waitFor : Motiend }, {features:[{name:"avoidHit",value:1},{name:"power",value:0.8},{name:"completeOp",value:1}]});
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