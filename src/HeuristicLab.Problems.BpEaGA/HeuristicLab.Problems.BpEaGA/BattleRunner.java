import robocode.control.*;
import robocode.control.events.*;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStream;

import java.io.PrintWriter;
import java.util.*;

public class BattleRunner {
  public static String player = "Evaluation.output";    
  public static List<Double> score = new ArrayList<Double>();

  public static void main(String[] args) {
    if (args.length < 5)
      System.exit(-1);  

    String roboCodePath = "C:\\Thesis\\robocode";
    Boolean visible = false;
    String bots = "";
    int numberOfRounds = 3;
    String[] robots = new String[1 + args.length - 4];

    player = robots[0] = args[0];
    roboCodePath = args[1];
    visible = Boolean.valueOf(args[2]);
    numberOfRounds = Integer.valueOf(args[3]);
    for (int i = 4; i < args.length; i++) {
      robots[i - 3] = args[i];
    }

    File logFile = new File("c:\\temp\\battleRunner.log");
    PrintWriter anOut;
	try {
		anOut = new PrintWriter(logFile);
	} catch (FileNotFoundException e) {
		// TODO Auto-generated catch block
		e.printStackTrace();
		anOut = new PrintWriter(System.out);
	}
    RobocodeEngine.setLogMessagesEnabled(false);
    RobocodeEngine engine = new RobocodeEngine(new java.io.File(roboCodePath));
    engine.setVisible(visible);
    engine.addBattleListener(new BattleObserver());

    BattlefieldSpecification battlefield = new BattlefieldSpecification(800, 600);
    RobotSpecification[] all = engine.getLocalRepository();
    List<RobotSpecification> selectedRobots = new ArrayList<RobotSpecification>();
    
    anOut.println("Supported robots:");
    for(RobotSpecification rs : all) {
    	anOut.println(rs.getClassName());
    	for(String r : robots) {
    		if(r.equals(rs.getClassName())) {
    			selectedRobots.add(rs);
    		}
    	}
    }
    
    anOut.println("*********************");

    for (int i = 1; i < selectedRobots.size(); i++) {
      BattleSpecification battleSpec = new BattleSpecification(numberOfRounds, battlefield, new RobotSpecification[] { selectedRobots.get(0), selectedRobots.get(i) });

      // run our specified battle and wait till the battle finishes
      engine.runBattle(battleSpec, true);
    }
    engine.close();
    anOut.flush();

    System.out.println(avg(score));
    System.exit(0);
  }

  private static double avg(List<Double> lst) {
    double sum = 0;
    for (double val : lst) {
      sum += val;
    }
    return sum / lst.size();
  }
}

class BattleObserver extends BattleAdaptor {
  public void onBattleCompleted(BattleCompletedEvent e) {
    double robotScore = -1.0;
    double opponentScore = -1.0;
    
    File logFile = new File("c:\\temp\\battleRunner.log");
    PrintWriter anOut;
	try {
		anOut = new PrintWriter(logFile);
	} catch (FileNotFoundException ex) {
		// TODO Auto-generated catch block
		ex.printStackTrace();
		anOut = new PrintWriter(System.out);
	}
    
	for (robocode.BattleResults result : e.getSortedResults()) {
		if (result.getTeamLeaderName().contains(BattleRunner.player)) {
			anOut.println("Result of robot: " + BattleRunner.player + " " + result.getScore());
			robotScore = result.getScore();
		} else {
			anOut.println("Result of opponent: " + result.getScore());
			opponentScore = result.getScore();
		}
	}
	
	anOut.flush();
    
    // prevent div 0 which can happen if both robots do not score
    if((robotScore + opponentScore) == 0) {
      BattleRunner.score.add(0.0);
    } else {
      BattleRunner.score.add(robotScore / (robotScore + opponentScore));
    }
  }
}