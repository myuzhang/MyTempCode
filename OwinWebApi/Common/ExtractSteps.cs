using System.Collections.Generic;

namespace Common
{
    public class ExtractSteps
    {
        public ExtractSteps(string stepsString)
        {
            Steps = new Dictionary<string, string>();
            Extract(stepsString);
        }
        public Dictionary<string, string> Steps { get; set; }

        private void Extract(string stepsString)
        {
            var steps = stepsString.Split(';');
            foreach (var step in steps)
            {
                if (step.Trim().StartsWith("When"))
                {
                    Steps.Add("When", step.Trim().RemoveFromStart(6));
                }
                else if (step.Trim().StartsWith("Then"))
                {
                    Steps.Add("Then", step.Trim().RemoveFromStart(6));
                }
                else if (step.Trim().StartsWith("Given"))
                {
                    Steps.Add("Given", step.Trim().RemoveFromStart(7));
                }
            }
        }
    }
}
