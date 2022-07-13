using System;

namespace Test.Discounter
{
    public class DiscounterContinuous : Discounter, IDiscounter

    {
        #region Constructor
        public DiscounterContinuous(Projector projector) 
            : base(projector)
        {
        }
        #endregion

        #region Public Methods
        public double[] GetDiscountedValue(double[] inflatedCostWithDecrement)
        {
            double[] value = inflatedCostWithDecrement;
            
            for (int i = 0; i < value.Length; i++)
            {
                value[i] = value[i] * GetDiscountFactor(i); // Create a variable and validate the value before calclations
            }
            
            return value;
        }
        #endregion

        #region Private Methods
        private double GetDiscountFactor(int refNumber)
        {
            return Math.Pow(Math.Exp((-Projector.Inputs.DiscountRate)/100), refNumber + 0.5);
        }
        #endregion
    }
}
