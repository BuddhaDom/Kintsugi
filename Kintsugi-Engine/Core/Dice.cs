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
        public static int Roll(int amount, int value, bool allowZero = false)
        {
            int roll = 0;
            int result = 0;
            // Generate and display 5 random integers between 0 and 100.
            Console.WriteLine("Dice roll:");
            for (int ctr = 0; ctr <= amount; ctr++)
            {
                roll = rand.Next(allowZero ? 0 : 1, value + 1);
                Console.Write("{0,8:N0}", roll);
                result = result + roll; // add roll
            }

            Console.WriteLine("\nTotal roll is {0}", result);
            return result;

        }
        // Skewed Dice Results
        // Random float value
        // Return all the dice used
        // 
    }
}
