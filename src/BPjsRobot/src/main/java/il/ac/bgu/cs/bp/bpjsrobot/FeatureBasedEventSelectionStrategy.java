package il.ac.bgu.cs.bp.bpjsrobot;

import static java.util.Collections.singleton;

import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Map.Entry;
import java.util.Optional;
import java.util.Set;
import java.util.stream.Collectors;

import org.mozilla.javascript.NativeArray;
import org.mozilla.javascript.NativeObject;

import il.ac.bgu.cs.bp.bpjs.model.BEvent;
import il.ac.bgu.cs.bp.bpjs.model.SyncStatement;
import il.ac.bgu.cs.bp.bpjs.model.eventselection.EventSelectionResult;
import il.ac.bgu.cs.bp.bpjs.model.eventselection.SimpleEventSelectionStrategy;
import il.ac.bgu.cs.bp.bpjs.model.eventsets.EventSet;
import il.ac.bgu.cs.bp.bpjsrobot.features.RobocodeFeature;
import il.ac.bgu.cs.bp.bpjsrobot.mathexpressionevaluator.ExpressionEvaluator;

public class FeatureBasedEventSelectionStrategy extends SimpleEventSelectionStrategy {
	private String featureBasedPolicy;
	private List<String> supportedFeatures;

	public FeatureBasedEventSelectionStrategy(List<String> supportedFeatures, String featureBasedPolicy) {
		this.featureBasedPolicy = featureBasedPolicy;
		this.supportedFeatures = supportedFeatures;
	}

    @Override
    public Optional<EventSelectionResult> select(Set<SyncStatement> statements, List<BEvent> externalEvents, Set<BEvent> selectableEvents) {
        if (selectableEvents.isEmpty()) {
            return Optional.empty();
        }
        
        Map<EventSet, Map<String, Double>> eventsToWaitFor = statements.stream()
        		.filter(s -> selectableEvents.contains(s.getWaitFor()) && s.getWaitFor() != null)
        		.collect(Collectors.toMap(s->s.getWaitFor(), s->parsStatementData(s)));
        
        Map<SyncStatement, Double> statementToGrade = statements.stream()
        		.filter(s -> selectableEvents.containsAll(s.getRequest()) && !s.getRequest().isEmpty())
        		.collect(Collectors.toMap(s->s, s->Double.valueOf(ExpressionEvaluator.evaluate(featureBasedPolicy, getVariablesForCalculation(s, eventsToWaitFor)))));
        
        BEvent chosen;
        if (statementToGrade.isEmpty()){
        	BEvent firstSelectable = (BEvent)selectableEvents.toArray()[0];
        	chosen = firstSelectable;
        }
        else{
        	Entry<SyncStatement, Double> max = statementToGrade.entrySet().stream().max((o1, o2) -> o2.getValue().compareTo(o1.getValue())).get();
        	chosen = (BEvent)max.getKey().getRequest().toArray()[0];
        }
//        System.out.println("Statements that are not blocked:");
//        statements.stream().forEach( e -> System.out.println(" + " + e));
        
//        System.out.println("The chosen event is: " + chosen + " because its statement had grade of: " + max.getValue());
        
        Set<BEvent> requested = statements.stream()
                .filter( stmt -> stmt!=null )
                .flatMap( stmt -> stmt.getRequest().stream() )
                .collect( Collectors.toSet() );
        
        if (requested.contains(chosen)) {
            return Optional.of(new EventSelectionResult(chosen));
        } else {
            // that was an internal event, need to find the first index 
            return Optional.of(new EventSelectionResult(chosen, singleton(externalEvents.indexOf(chosen))));
        }
    }

	private Map<String, Double> getVariablesForCalculation(SyncStatement statement, Map<EventSet, Map<String, Double>> eventsToWaitFor) {
		Map<String, Double> vars = parsStatementData(statement);
		
		for (Entry<EventSet, Map<String, Double>> eventToWaitFor : eventsToWaitFor.entrySet()){
			for (BEvent ev : statement.getRequest()){
				if (eventToWaitFor.getKey().contains(ev)){
					for (Entry<String, Double> featureToUpdate : eventToWaitFor.getValue().entrySet()){
						if (vars.containsKey(featureToUpdate.getKey())){
							String key = featureToUpdate.getKey();
							vars.put(key, vars.get(key) + featureToUpdate.getValue());
						}
					}
				}
			}
		}
		
		return vars;
	}
	
	private Map<String, Double> parsStatementData(SyncStatement statement) {
		NativeObject data = (NativeObject) statement.getData();
		Map<String, Double> vars = new HashMap<>();
		if (data != null){
			NativeArray dataArray = (NativeArray)(data.get("features"));
			dataArray.forEach(obj->{
				NativeObject featureData = (NativeObject)obj;
				String name = (String)featureData.get("name");
				double value = (double)featureData.get("value");
				RobocodeFeature feature = new RobocodeFeature(name, value);
				double factorValue = feature.getValue();
				
				vars.put(feature.getName(), factorValue);
				System.out.println(name+": "+value);
			});
		}
		supportedFeatures.forEach(f->vars.putIfAbsent(f, 0.0));
		return vars;
	}
}
