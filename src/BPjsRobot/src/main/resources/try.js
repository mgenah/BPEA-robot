importPackage(Packages.il.ac.bgu.cs.bp.bpjsrobot);
importPackage(Packages.il.ac.bgu.cs.bp.bpjsrobot.events.actions);
importPackage(Packages.il.ac.bgu.cs.bp.bpjsrobot.events.sensors);
importPackage(Packages.robocode.util);

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
			bp.sync({ request : TurnRadarRight(10)});
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