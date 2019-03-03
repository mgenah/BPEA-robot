package il.ac.bgu.cs.bp.bpjsrobot;

import static java.util.Collections.emptySet;
import static java.util.Collections.singleton;
import static java.util.stream.Collectors.toSet;

import java.io.PrintStream;
import java.util.Arrays;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.atomic.AtomicInteger;
import java.util.stream.Collectors;

import org.mozilla.javascript.Context;

import il.ac.bgu.cs.bp.bpjs.bprogramio.BProgramSyncSnapshotCloner;
import il.ac.bgu.cs.bp.bpjs.internal.ExecutorServiceMaker;
import il.ac.bgu.cs.bp.bpjs.model.BEvent;
import il.ac.bgu.cs.bp.bpjs.model.BProgramSyncSnapshot;
import il.ac.bgu.cs.bp.bpjs.model.SyncStatement;
import il.ac.bgu.cs.bp.bpjs.model.eventselection.SimpleEventSelectionStrategy;
import il.ac.bgu.cs.bp.bpjs.model.eventsets.ComposableEventSet;
import il.ac.bgu.cs.bp.bpjs.model.eventsets.EventSet;
import il.ac.bgu.cs.bp.bpjs.model.eventsets.EventSets;

public class FeatureBasedEventSelectionStrategy extends SimpleEventSelectionStrategy {
	private String featureBasedPolicy;
	private static final AtomicInteger INSTANCE_COUNTER = new AtomicInteger();
	private ExecutorService executor;
	private PrintStream out;

	public FeatureBasedEventSelectionStrategy(PrintStream out, String featureBasedPolicy) {
		this.out = out;
		this.featureBasedPolicy = featureBasedPolicy;
		this.executor = ExecutorServiceMaker.makeWithName("SimulationBProgramRunner-" + INSTANCE_COUNTER.incrementAndGet());
	}

	@Override
    public Set<BEvent> selectableEvents(BProgramSyncSnapshot bpss) {
        Set<SyncStatement> statements = bpss.getStatements();
        List<BEvent> externalEvents = bpss.getExternalEvents();
        if ( statements.isEmpty() ) {
            // Corner case, not sure this is even possible.
            return externalEvents.isEmpty() ? emptySet() : singleton(externalEvents.get(0));
        }
        
        EventSet blocked = ComposableEventSet.anyOf(statements.stream()
                .filter( stmt -> stmt!=null )
                .map(SyncStatement::getBlock )
                .filter(r -> r != EventSets.none )
                .collect( toSet() ) );
        
        Set<BEvent> requested = statements.stream()
                .filter( stmt -> stmt!=null )
                .flatMap( stmt -> stmt.getRequest().stream() )
                .collect( toSet() );
        
        // Let's see what internal events are requested and not blocked (if any).
        try {
            Context.enter();
            Set<BEvent> requestedAndNotBlocked = requested.stream()
                    .filter( req -> !blocked.contains(req) )
                    .collect( toSet() );

            out.println("external events: " + Arrays.toString(externalEvents.toArray()));
            if (requestedAndNotBlocked.isEmpty()){
            	return externalEvents.stream().filter( e->!blocked.contains(e) ) // No internal events requested, defer to externals.
                        .findFirst().map( e->singleton(e) ).orElse(emptySet());
            }
            Map<BEvent, Double> gradedEvents = new HashMap<>();
            requestedAndNotBlocked.forEach(event->gradedEvents.put(event, Double.valueOf(simulateTheChosenEvent(bpss, event))));
        	Double max = gradedEvents.entrySet().stream().mapToDouble(entry->entry.getValue()).max().getAsDouble();
            return gradedEvents.entrySet().stream().filter(entry->max.equals(entry.getValue())).map(entry->entry.getKey()).collect(Collectors.toSet());
        } finally {
            Context.exit();
        }
    }
    
    private double simulateTheChosenEvent(BProgramSyncSnapshot bpss, BEvent chosenEvent) {
    	BProgramSyncSnapshot clonedSnapshot = BProgramSyncSnapshotCloner.clone(bpss);
    	RobotState clonedState = BPjsRobot.getInstance().clone();
    	try {
			clonedSnapshot.triggerEvent(chosenEvent, executor, Collections.emptySet());
			double grade = BPjsRobot.getInstance().grade(featureBasedPolicy);
			out.println("The grade for choosing event " + chosenEvent + " is: " + grade);
			out.println("Robot state after simulation: " + BPjsRobot.getInstance().toString());
			out.println("Cloned Robot state after simulation: " + clonedState.toString());
			return grade;
		} catch (InterruptedException e) {
			System.out.println("Simulation was interrupted");
		} finally {
			BPjsRobot.getInstance().update(clonedState);
		}
    	return 0.0;
    }
}
