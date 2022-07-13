using System;

namespace Test.Discounter
{
    public class DiscounterStandard : Discounter
    {
        public DiscounterStandard(Projector projector) : base(projector)
        {

        }
        
        #region Public methods
        public double[] GetYearlyDiscountedCashflows()
        {
            if (Projector.Inputs.IsContinuous)
            {
                return DiscountingContinuousForceApplied();
            }

            return DiscountingSinglePayment();
        }

        public double SumDiscountedArray(double[] array)
        {
            double sum = 0;
            for (int i = 0; i < Projector.Inputs.YearsToRollForwardBy; i++)
            {
                sum += array[i];
            }
            return sum;
        }

        #endregion


        #region Private methods
        private double[] DiscountingSinglePayment()
        {
            double[] cashflowArrayWithDecrement = Projector.GetInflatedCostWuthDecrement();

            double[] yearlyDiscountedCashflow = new double[Projector.Inputs.YearsToRollForwardBy];
            double discountFactor = Math.Pow(1 + Projector.Inputs.DiscountRate, -1);

            for (int i = 0; i < Projector.Inputs.YearsToRollForwardBy; i++)
            {
                yearlyDiscountedCashflow[i] = cashflowArrayWithDecrement[i] * Math.Pow(discountFactor, i + Projector.Inputs.YearsToRollForwardBy);
            }

            // Most of the places the variables are not initialised. It makes the code very efficient.
            return yearlyDiscountedCashflow;
        }

        private double[] DiscountingContinuousForceApplied()
        {
            double[] cashflowArrayWithDecrement = Projector.GetInflatedCostWuthDecrement();

            double[] yearlyDiscountedCashflow = new double[Projector.Inputs.YearsToRollForwardBy];

            for (int i = 0; i < Projector.Inputs.YearsToRollForwardBy; i++) // Use foreach or while
            {
                yearlyDiscountedCashflow[i] = cashflowArrayWithDecrement[i] * Math.Exp(-((i + Projector.Inputs.YearsToRollForwardBy) * Projector.Inputs.DiscountRate)); // Math.Exp() should be validated as it returns 0 for some calculations of float variables
            }
            return yearlyDiscountedCashflow;
        }
        

        #endregion
    }
}
