using System.Collections.Generic;

namespace Test
{
    public class Calcs
    {
        private Engine _engine;

        public Calcs(Engine engine)
        {
            _engine = engine;
        }

        // Summarise the public methods
        public double GetProjections(bool undiscountedProjection, List<Inputs> listOfInputs)
        {
            Result totalProjection = _engine.GetResultProjections(listOfInputs);

            if (undiscountedProjection)
            {
                double[] totalYearlyProjections = Aggregator.AggregateYearlyProjections(totalProjection.TotalProjections, listOfInputs);
                return Aggregator.GetTotalSum(totalYearlyProjections);
            }
            else
            {
                double[] totalYearlyDiscountedProjections = Aggregator.AggregateYearlyProjections(totalProjection.TotalDiscountedProjections, listOfInputs);
                return Aggregator.GetTotalSum(totalYearlyDiscountedProjections);
            }
            // two different variables are craeted inside if-else. single variable could have been craeted and initialised to optimise storage
        }
        
        public double GetRollForwardProjections(bool undiscountedProjection, int rollForwardYears, List<Inputs> listOfInputs)
        {
            Result totalRollForwardProjection = _engine.GetRollForwardProjections(listOfInputs, rollForwardYears);

            if (undiscountedProjection)
            {
                double[] totalYearlyProjections = Aggregator.AggregateYearlyProjections(totalRollForwardProjection.TotalProjections, listOfInputs);
                return Aggregator.GetTotalSum(totalYearlyProjections);
            }
            else
            {
                double[] totalYearlyDiscountedProjections = Aggregator.AggregateYearlyProjections(totalRollForwardProjection.TotalDiscountedProjections, listOfInputs);
                return Aggregator.GetTotalSum(totalYearlyDiscountedProjections);
            }
        }
    }
}
