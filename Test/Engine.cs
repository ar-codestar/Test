using System.Collections.Generic;
using Test.Discounter;

namespace Test
{
    public class Engine
    {
        #region Public Methods
        public Result GetResultProjections(List<Inputs> listOfInputs)
        {
            List<double[]> listOfProjections = new List<double[]>(4);
            List<double[]> listOfDiscountedProjections = new List<double[]>(4);
            List<Projector> listOfProjectors = new List<Projector>(4);

            foreach (Inputs inputType in listOfInputs)
            {
                Projector projector = new Projector(inputType);

                double[] inflatedCost = projector.GetInflatedCost();
                double[] inflatedCostWithDec = projector.GetInflatedCost();
                projector.UpdateInflatedCostWithDecrement(inflatedCost);

                listOfProjections.Add(inflatedCost);
                listOfProjectors.Add(projector);

                if (projector.Inputs.IsContinuous)
                {
                    DiscounterContinuous discounterContinuous = new DiscounterContinuous(projector);
                    double[] discountedProjections = discounterContinuous.GetDiscountedValue(inflatedCostWithDec);
                    listOfDiscountedProjections.Add(discountedProjections);
                }
                else
                {
                    DiscountEndOfYear discountEndOfYear = new DiscountEndOfYear(projector);
                    double[] discountedProjections = discountEndOfYear.GetDiscountedValue(inflatedCostWithDec);
                    listOfDiscountedProjections.Add(discountedProjections);
                }
            }
            return new Result(listOfProjections, listOfDiscountedProjections);
        }

        // The method could have first intialised all the variables and then used whenever necessary.  
        public Result GetRollForwardProjections(List<Inputs> listOfInputs, int rollForwardYears)
        {
            List<double[]> rollForwardProjectionsList = new List<double[]>(4);

            List<double[]> rollForwardDiscountedProjectionsList = new List<double[]>(4);

            foreach (var input in listOfInputs)
            {
                Projector projector = new Projector(input);

                double[] preRollForwardProjection = projector.GetInflatedCost();
                projector.UpdateInflatedCostWithDecrement(preRollForwardProjection);

                RollForward rollForward = new RollForward(projector);

                double[] rollForwardProjection = rollForward.GetRollForwardProjections(rollForwardYears, preRollForwardProjection);
                double[] rollForwardProjection2 = rollForward.GetRollForwardProjections(rollForwardYears, preRollForwardProjection);
                rollForwardProjectionsList.Add(rollForwardProjection);

                double[] rollForwardDiscountedProjections = rollForward.GetRollForwardDiscountedProjections(rollForwardYears, rollForwardProjection2);
                rollForwardDiscountedProjectionsList.Add(rollForwardDiscountedProjections);
            }
            return new Result(rollForwardProjectionsList, rollForwardDiscountedProjectionsList);
        }
        #endregion
    }
}