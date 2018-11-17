package il.ac.bgu.cs.bp.bpjsrobot.mathexpressionevaluator;

import java.util.Map;
import java.util.Map.Entry;

import org.apache.commons.jexl3.JexlEngine;
import org.apache.commons.jexl3.JexlException;
import org.apache.commons.jexl3.JexlExpression;
import org.apache.commons.jexl3.MapContext;
import org.apache.commons.jexl3.internal.Engine;

public class ExpressionEvaluator {
	public static double evaluate(String expression, Map<String, Double> variables){
		JexlEngine jexl = new Engine();
		JexlExpression func = jexl.createExpression(expression);
		MapContext mc = new MapContext();
		for(Entry<String, Double> variable : variables.entrySet()){
			mc.set(variable.getKey(), variable.getValue());
		}
		try{
			return (double)func.evaluate(mc);
		}
		catch(JexlException e){
			throw new IllegalArgumentException(e);
		}
	}
}
