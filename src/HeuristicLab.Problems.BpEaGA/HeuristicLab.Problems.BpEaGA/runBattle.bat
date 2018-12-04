@echo off

rem java -cp C:/Studies/Thesis/Project/HL/HeuristicLab.Problems.BpEa/HeuristicLab.Problems.BpEa;c:/robocode1/libs/robocode.jar;c:/robocode1/libs/robocode.core-1.9.3.2.jar;c:/robocode1/libs/picocontainer-2.14.2.jar BattleRunner sample.Tracker c:\\robocode1 false 1 sample.SittingDuck
rem ;C:/Users/meytal/.m2/repository/org/apache/commons/commons-jexl3/3.1/commons-jexl3-3.1.jar;C:/Users/meytal/.m2/repository/commons-logging/commons-logging/1.2/commons-logging-1.2.jar;C:/Users/meytal/.m2/repository/com/github/bthink-bgu/BPjs/0.9.2/BPjs-0.9.2.jar;C:/Users/meytal/.m2/repository/org/mozilla/rhino/1.7.7.2/rhino-1.7.7.2.jar

set robot=%1
set enemy=%2
set number_of_rounds=%3

xcopy c:\\Thesis\\robocode\\robots\\Evaluation\* c:\Thesis\\BPEA-robot\src\BPjsRobot\target\classes\il\ac\bgu\cs\bp\bpjsrobot\BPjsRobot.data\
java -cp .;C:/Thesis/BPEA-robot/src/HeuristicLab.Problems.BpEa/HeuristicLab.Problems.BpEa;c:/Thesis/robocode/libs/robocode.jar;c:/Thesis/robocode/libs/robocode.core-1.9.3.3.jar;c:/Thesis/robocode/libs/picocontainer-2.14.2.jar BattleRunner %robot% c:\\Thesis\\robocode false %number_of_rounds% %enemy%