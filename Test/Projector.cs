using System;

namespace Test
{
    public class Projector
    {
        #region Properties
        public Inputs Inputs;
        #endregion

        #region Constructor
        public Projector(Inputs inputs)
        {
            Inputs = inputs;
        }
        #endregion

        //Every method needs to be defined before the definition of code. This gives easy readability for testers and third person trying to understand the code
        //This includes what are the parameters of the function and what this function/method does. 
        #region Public Methods
        public double[] GetInflatedCost()
        {
            double[] projections = new double[Inputs.Time];
            double factor = (Inputs.Inflation + 100) / 100;

            for (int i = 0; i < Inputs.Time; i++)
            {
                projections[i] = Inputs.Cost * GetProjectionFactor(factor, i);
            }

            return projections;
        }
        
        public double[] GetInflatedCostWuthDecrement()
        {
            double[] projections = GetInflatedCost();

            double[] projectionsWithDecrement = new double[Inputs.Time];
            double factor = (100 + Inputs._annualChangeInYearlyPayments)/100;

            for (int i = 0; i < projections.Length; i++) // Using while loop helps reduce the time performance when object and its length is accessible 
            {
                projectionsWithDecrement[i] = projections[i] * Math.Pow(factor, i + 1); // Pow in general takes a lot of time to process. we can modify and craete a multiplicative function for the same which is time efficient. 
            }

            return projectionsWithDecrement;
        }

        public void UpdateInflatedCostWithDecrement(double[] salaryProjectionsWithoutDecrement)
        {
            for (int i = 0; i < Inputs.Time; i++) // Use while loop
            {
                salaryProjectionsWithoutDecrement[i] *= Inputs.GetAvgPercentOfAnnualChangeInPayments(i);
            }
        }
        #endregion

        #region Private Methods
        private static double GetProjectionFactor(double factor, int cashFlowYear)
        {
            return (Math.Pow(factor, cashFlowYear));
        }
        #endregion
    }
}