using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace QBRating_RobertoVinas
{
    /// <summary>
    /// Main page that will calculate the quarterback rating for the given player name and passing data.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region "Variables"
        //Constants
        private const decimal MaxAllowed = (decimal) 2.375;

        //Variables
        private decimal passAttempts = 0;
        private decimal weightedCompletionPercentage = 0;
        private decimal weightedYardsPerAttempt = 0;
        private decimal weightedTouchdownPasses = 0;
        private decimal weightedInterceptions = 0;
        private decimal quarterbackRating = 0;
        #endregion

        #region "Page Functions"
        public MainPage()
        {
            this.InitializeComponent();
        }
        #endregion

        #region "Button Clicks"

        /// <summary>
        /// Calculate QBR button click event.  Will use the data from the inputs and calculate a player's QBR.  
        /// The QBR or any error message encountered will display below the buttons in the GUI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {

            if (decimal.TryParse(txtPassAttempts.Text, out passAttempts))
            {
                if (passAttempts > 0)
                {
                    CalculateQuarterbackRating();
                    txtQuarterbackRating.Text = txtPlayerName.Text + "'s QB Rating is: " + quarterbackRating.ToString();
                }
                else
                {
                    txtQuarterbackRating.Text = "The player must have at least 1 pass attempt.";
                }
            }
            else
            {
                txtQuarterbackRating.Text = "Invalid number in Pass Attempts.";
            }
        }

        /// <summary>
        /// Reset button click event.  This is an additional feature that will be useful to the user.  
        /// Rather than having to manually clear the inputs this button will do that.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtPlayerName.Text = "";
            txtPassAttempts.Text = "";
            txtPassCompletions.Text = "";
            txtPassingTouchdowns.Text = "";
            txtPassingYards.Text = "";
            txtInterceptions.Text = "";
            txtQuarterbackRating.Text = "";           
        }
        #endregion

        #region "Calculation Functions"

        /// <summary>
        /// Uses the Pass Attempts and Pass Completion inputs to calculate the Weighted Completion Percentage.
        /// </summary>
        private void CalculateWeightedCompletionPercentage()
        {
            decimal passCompletions = 0;
            if (!(decimal.TryParse(txtPassCompletions.Text, out passCompletions)))
            {
                passCompletions = 0;
                txtPassCompletions.Text = "0";
            }

            weightedCompletionPercentage = (passCompletions / passAttempts) * 100;
            weightedCompletionPercentage -= 30;
            weightedCompletionPercentage *= (decimal)0.05;
            weightedCompletionPercentage = CheckMinMax(weightedCompletionPercentage);
        }

        /// <summary>
        /// Uses the Pass Attempts and Passing Yards inputs to calculate the Weighted Yards Per Attempt.
        /// </summary>
        private void CalculateWeightedYardsPerAttempt()
        {
            decimal passYards = 0;
            if (!(decimal.TryParse(txtPassingYards.Text, out passYards)))
            {
                passYards = 0;
                txtPassingYards.Text = "0";
            }

            weightedYardsPerAttempt = passYards / passAttempts;
            weightedYardsPerAttempt -= 3;
            weightedYardsPerAttempt *= (decimal)0.25;
            weightedYardsPerAttempt = CheckMinMax(weightedYardsPerAttempt);

        }

        /// <summary>
        /// Uses the Pass Attempts and Passing Touchdowns inputs to calculate the Weighted Touchdown Passes.
        /// </summary>
        private void CalculateWeightedTouchdownPasses()
        {
            decimal passTouchdowns = 0;
            if (!(decimal.TryParse(txtPassingTouchdowns.Text, out passTouchdowns)))
            {
                passTouchdowns = 0;
                txtPassingTouchdowns.Text = "0";
            }

            weightedTouchdownPasses = (passTouchdowns / passAttempts) * 100;
            weightedTouchdownPasses *= (decimal)0.2;
            weightedTouchdownPasses = CheckMinMax(weightedTouchdownPasses);
        }

        /// <summary>
        /// Uses the Pass Attempts and Interceptions inputs to calculate the Weighted Interceptions.
        /// </summary>
        private void CalculateWeightedInterceptions()
        {
            decimal interceptions = 0;
            if (!(decimal.TryParse(txtInterceptions.Text, out interceptions)))
            {
                interceptions = 0;
                txtInterceptions.Text = "0";
            }

            weightedInterceptions = (interceptions / passAttempts) * 100;
            weightedInterceptions *= (decimal)0.25;
            weightedInterceptions = MaxAllowed - weightedInterceptions;
            weightedInterceptions = CheckMinMax(weightedInterceptions);
        }

        /// <summary>
        /// Uses the user input to calculate the Player's Quarterback Rating.
        /// Calls the other calculation methods to get the weighted values needed for a Quarterback Rating.
        /// </summary>
        private void CalculateQuarterbackRating()
        {
            CalculateWeightedCompletionPercentage();
            CalculateWeightedYardsPerAttempt();
            CalculateWeightedTouchdownPasses();
            CalculateWeightedInterceptions();

            quarterbackRating = weightedCompletionPercentage + weightedYardsPerAttempt + weightedTouchdownPasses + weightedInterceptions;
            quarterbackRating /= 6;
            quarterbackRating *= 100;

            quarterbackRating = (decimal)Decimal.Round(quarterbackRating, 1);
        }

        /// <summary>
        /// Simple function to standardize a Minimum value of 0 and a Maximum value using the declared constant.
        /// </summary>
        /// <param name="numberToCheck"></param>
        /// <returns>A decimal value of either Min (0), Max (Constant), or the number sent in.</returns>
        private decimal CheckMinMax(decimal numberToCheck)
        {
            if (numberToCheck > MaxAllowed)
            {
                numberToCheck = MaxAllowed;
            }
            else if (numberToCheck < 0)
            {
                numberToCheck = 0;
            }

            return numberToCheck;
        }
        #endregion
    }
}
