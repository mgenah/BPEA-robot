#!/bin/sh

RobocodeHome=$1
RobotName=$2
Enemy=$3
NumOfRounds=$4
CommonLibs=$5

java -Xmx1024M -Ddebug=true -DNOSECURITY=true -Dsun.io.useCanonCaches=false -classpath .:${RobocodeHome}/libs/:${RobocodeHome}/libs/robocode.jar:${CommonLibs}/commons-jexl3-3.1.jar:${CommonLibs}/commons-logging-1.2.jar:${CommonLibs}/BPjs-0.10.1.jar:${CommonLibs}/rhino-1.7.9.jar BattleRunner ${RobotName} ${RobocodeHome} false ${NumOfRounds} ${Enemy}
