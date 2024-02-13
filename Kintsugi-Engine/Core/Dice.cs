using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi.Core
{
    public static class Dice
    {
        static Random rand = new Random();
        public static int Roll(int amount, int value, bool grabHighest = false, bool allowZero = false)
        {
            int roll = 0;
            List<int> diceList = new List<int>();
            int result = 0;
            // Generate and display 5 random integers between 0 and 100.
            //Console.WriteLine("Dice roll:");
            for (int ctr = 0; ctr <= amount; ctr++)
            {
                roll = rand.Next(allowZero ? 0 : 1, value + 1);
                diceList.Add(roll);
                //Console.Write("{0,8:N0}", roll);
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
