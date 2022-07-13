using System;

namespace Test.Discounter
{
    public class DiscountEndOfYear:Discounter,IDiscounter
    {
        // Constructor could have been used to to initialise the values
        #region Constructor
        public DiscountEndOfYear(Projector projector) 
            : base(projector)
        {
        }
        #endregion

        #region Public Methods
        public double[] GetDiscountedValue(double[] inflatedCostWithDecrement)
        {
            double[] value = inflatedCostWithDecrement; // no need of creating space for this variable

            for (int i = 0; i < value.Length; i++)
            {
                value[i] = value[i] * GetDiscountFactor(i); //Getting value into some variable and then performing calculation helps the risk of error as we can validate the variables before calculations.
            }

            return value;
        }
        #endregion

        #region Private Methods
        private double GetDiscountFactor(int refNumber)
        {
            return Math.Pow(((100+Projector.Inputs.DiscountRate)/100),-(refNumber+1));
        }
        #endregion
    }
}