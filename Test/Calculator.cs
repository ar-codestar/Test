using System.Collections.Generic;
using System.Linq;
using Test.Discounter;

namespace Test
{
    public class Calculator
    {
        public Calculator(List<Inputs> listOfAllInputs)
        {
            _listOfAllInputs = listOfAllInputs;
            TermForAllProjections = MaximumTermOfProjection.SetTermOfProjection(listOfAllInputs);

            ProjectedCashflowsForEachOutgoingType = new List<double[]>();
            ProjectedRollForwardCashflowsForEachOutgoingType = new List<double[]>();

            RunProjector();
            RunRollForward();
        }
        
        private readonly List<Inputs> _listOfAllInputs;

        public int TermForAllProjections { get; }
        public List<double[]> ProjectedCashflowsForEachOutgoingType { get; }
        public List<double[]> ProjectedRollForwardCashflowsForEachOutgoingType { get; }
        public double[] AggregatedCashflows { get; private set; }
        public double[] AggregatedRollForwardCashflows { get; private set; }

        public void RunProjector()
        {
            foreach (Inputs input in _listOfAllInputs)
            {
                Projector projector = new Projector(input); // This is creating projector object of projector class for every entry in input variable. Instead, we can use list object for each input
                DiscounterContinuous discounter = new DiscounterContinuous(projector);

                double[] arrayOfDiscountedCashflows = discounter.GetDiscountedValue(projector.GetInflatedCostWuthDecrement());
                ProjectedCashflowsForEachOutgoingType.Add(arrayOfDiscountedCashflows);
            }

            double[] aggregatedCashflow = Aggregator.AggregateYearlyProjections(ProjectedCashflowsForEachOutgoingType, _listOfAllInputs);
            AggregatedCashflows = aggregatedCashflow; // using AggregatedCashflows to get all the values on line 40 can save data storage and would amke the code effective
        }

        public void RunRollForward()
        {
            foreach (Inputs input in _listOfAllInputs)
            {
                Projector projector = new Projector(input); // Instance can be craeted in public space so that each method can access the same instance. this will help in storage optimisation
                RollForward rollForward = new RollForward(projector);

                double[] yearlyInflatedCashflowAtBaseDate = projector.GetInflatedCostWuthDecrement();

                double[] rollForwardProjections = rollForward.GetRollForwardProjections(projector.Inputs.Time, yearlyInflatedCashflowAtBaseDate);
                ProjectedRollForwardCashflowsForEachOutgoingType.Add(rollForwardProjections); // use this at line 53
            }

            double[] aggregatedRollForwardCashflow = Aggregator.AggregateYearlyProjections(ProjectedRollForwardCashflowsForEachOutgoingType, _listOfAllInputs);
            AggregatedRollForwardCashflows = aggregatedRollForwardCashflow;
        }

        public double[] CalculateTotalInfWithDec()
        {
            List<double> sums = new List<double>(); // Initialise

            double[] cashFlow = new Projector(_listOfAllInputs.First()).GetInflatedCostWuthDecrement();

            int currentSumsLength = sums.Count;


            // variables and objects should be validated before using.
            for (int i =0; i < cashFlow.Length; i++)
            {
                if (i > currentSumsLength-1)
                {
                    sums.Add(cashFlow[i]);
                }
                else
                {
                    sums[i] += cashFlow[i];
                }
            }

            double[] totalArray = sums.ToArray();

            return totalArray;
        }

        public double[] CalculateTotalInfCashflows()
        {
            List<double> sums = new List<double>();

            //Calling explicit methods and then using the returned values is more efficient 
            double[] cashFlow = new Projector(_listOfAllInputs.First()).GetInflatedCost();

            int currentSumsLength = sums.Count;

            // validating if "cashflow.Length" exist in the present instance will help reduce errors
            for (int i = 0; i < cashFlow.Length; i++)
            {
                if (i > currentSumsLength - 1)
                {
                    sums.Add(cashFlow[i]);
                }
                else
                {
                    sums[i] += cashFlow[i];
                }
            }

            double[] totalArray = sums.ToArray();

            return totalArray;
        }
        
        public double[] CalculateTotalInfCashflows(bool withDecrement)
        {
            if (withDecrement)
            {
                List<double> sums = new List<double>();

                double[] cashFlow = new Projector(_listOfAllInputs.First()).GetInflatedCostWuthDecrement();

                int currentSumsLength = sums.Count;

                for (int i = 0; i < cashFlow.Length; i++)
                {
                    if (i > currentSumsLength - 1)
                    {
                        sums.Add(cashFlow[i]);
                    }
                    else
                    {
                        sums[i] += cashFlow[i];
                    }
                }

                double[] totalArray = sums.ToArray();

                return totalArray;
            }

            List<double> sumsNoDec = new List<double>();

            double[] cashFlowNoDec = new Projector(_listOfAllInputs.First()).GetInflatedCost();

            int currentSumsLengthNoDec = sumsNoDec.Count;

            for (int i = 0; i < cashFlowNoDec.Length; i++)
            {
                if (i > currentSumsLengthNoDec - 1)
                {
                    sumsNoDec.Add(cashFlowNoDec[i]);
                }
                else
                {
                    sumsNoDec[i] += cashFlowNoDec[i];
                }
            }

            double[] totalArrayNoDec = sumsNoDec.ToArray();

            return totalArrayNoDec;
        }
    }
}
