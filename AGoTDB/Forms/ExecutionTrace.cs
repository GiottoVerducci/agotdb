using System;
using System.Collections.Generic;
using System.Text;

namespace AGoTDB.Forms
{
    public class ExecutionTrace
    {
        private readonly List<ExecutionStep> _executionSteps = new List<ExecutionStep>();

        public ExecutionStep AddStep(string stepLabel)
        {
            var result = new ExecutionStep(stepLabel, false);
            _executionSteps.Add(result);
            return result;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            int i = 0;
            foreach (var step in _executionSteps)
                result.AppendFormat("#{0:00} - {1}: {2}{3}", ++i, step.Label, step.IsSuccessful ? Resource1.Success : Resource1.Failure, Environment.NewLine);
            return result.ToString();
        }
    }

    public class ExecutionStep
    {
        public string Label { get; set; }
        public bool IsSuccessful { get; set; }

        public ExecutionStep(string label, bool isSuccessful)
        {
            Label = label;
            IsSuccessful = isSuccessful;
        }
    }
}