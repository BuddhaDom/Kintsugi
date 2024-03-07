namespace Kintsugi.Core
{
    /// <summary>
    /// Dice-rolling utilities.
    /// </summary>
    public static class Dice
    {
        static Random rand = new Random();
        /// <summary>
        /// Roll dice!
        /// </summary>
        /// <param name="amount">How many dice to roll.</param>
        /// <param name="value">The dices' maximum value.</param>
        /// <param name="grabHighest"><c>true</c> if method should only return the highest roll.</param>
        /// <param name="allowZero"><c>true</c> if dices start at <c>0</c> instead of <c>1</c>.</param>
        /// <returns>The result of this dice roll.</returns>
        public static int Roll(int amount, int value, bool grabHighest = false, bool allowZero = false)
        {
            List<int> diceList = new List<int>();
            int roll = 0;
            int result = 0;
            //Console.WriteLine("Dice roll:");
            for (int ctr = 0; ctr < amount; ctr++)
            {
                roll = rand.Next(allowZero ? 0 : 1, value + 1);
                diceList.Add(roll);
                Console.Write("{0,8:N0}", roll);
                if (!grabHighest)
                {
                    result = result + roll; // add roll
                }
            }

            if(grabHighest) result = diceList.Max();

            //Console.WriteLine("\nTotal roll is {0}", result);
            return result;
        }
        // Skewed Dice Results
        // Random float value
        // Return all the dice used
        // 
    }
}
