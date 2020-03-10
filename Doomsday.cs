using System;
using System.Collections.Generic;

namespace weekday_calculator
{
    public class Doomsday
    {
        /* Use the doomsday rule to find out a date's weekday with the following steps:
         * 1. Calculate the century's "anchor day".
         * 2. Calculate the year's "doomsday".
         * 3. Look for the closest doomsday in the month we're interested in.
         * 4. Count how many days away we are from the closest doomsday to determine 
         *    what weekday the date is.
         * https://en.wikipedia.org/wiki/Doomsday_rule */

        private int year;
        private int month;
        private int day;
        private int anchor;
        private static Dictionary<int, string> weekdays = new Dictionary<int, string>{
            {0, "Sunday"},
            {1, "Monday"},
            {2, "Tuesday"},
            {3, "Wednesday"},
            {4, "Thursday"},
            {5, "Friday"},
            {6, "Saturday"}
        };
        private Dictionary<int, int[]> doomsdays = new Dictionary<int, int[]>(){
            {0,  new int[] {3, 10, 17, 24, 31}}, // January
            {1,  new int[] {7, 14, 21, 28}},     // February
            {2,  new int[] {7, 14, 21, 28}},     // March
            {3,  new int[] {4, 11, 18, 25}},     // April
            {4,  new int[] {2, 9, 16, 23, 30}},  // May
            {5,  new int[] {6, 13, 20, 27}},     // June
            {6,  new int[] {4, 11, 18, 25}},     // July
            {7,  new int[] {1, 8, 15, 22, 29}},  // August
            {8,  new int[] {5, 12, 19, 26}},     // September
            {9,  new int[] {3, 10, 17, 24, 31}}, // October
            {10, new int[] {7, 14, 21, 28}},     // November
            {11, new int[] {5, 12, 19, 26}}      // December
        };

        public bool isLeapYear()
        {
            // Determines if a year is a leap year or not
            if (this.year % 4 != 0) {
                return false;
            } else if (this.year % 100 != 0) {
                return true;
            } else if (this.year % 400 != 0) {
                return false;
            } else {
                return true;
            }
        }

        private void assertCorrectInput(string input)
        {
            // Making sure that the input is actually a valid date

            // Separate year, month, and day and save into separate variables
            char[] separator = {'-', '/', '.', ' '};
            string[] strlist = input.Split(separator, 3,
                    StringSplitOptions.RemoveEmptyEntries);

            try {
                // Control that the year string is 4 digits long
                if (strlist[0].Length != 4) {
                    throw new System.Exception();
                }
                // Try parsing the strings to ints
                this.year = int.Parse(strlist[0]);
                this.month = int.Parse(strlist[1]);
                this.day = int.Parse(strlist[2]);
            } catch (System.Exception) {
                Console.WriteLine("Invalid date format. Please only enter dates in YYYY.MM.DD format.");
                Environment.Exit(-1);
            }

            if (this.month == 2 && this.day > (isLeapYear() ? 29 : 28)) {
                Console.WriteLine($"Day needs to be within range 01-{(isLeapYear() ? 29 : 28)}");
                Environment.Exit(-1);
            } else if (this.month < 1 || this.month > 12) {
                Console.WriteLine("Month needs to be within range 01-12.");
                Environment.Exit(-1);
            } else if (this.day < 1 || this.day > 31) {
                Console.WriteLine("Day needs to be within range 01-31");
                Environment.Exit(-1);
            }
        }

        private int anchorDay()
        {
            /* Gets the century's 'anchor day' using the following formula:
            5 x (c mod 4) mod 7 + 2 [Tuesday]
            Where c = year / 100 */

            int c = this.year / 100;
            return 5 * (c % 4) % 7 + 2;
        }

        private int doomsDay()
        {
            // Calculate the year's doomsday
            // https://en.wikipedia.org/wiki/Doomsday_rule#Finding_a_year's_Doomsday

            int y = this.year % 100; // Last two digits of the year
            int a = y / 12;
            int b = y % 12;
            int c = b / 4;
            int d = (a + b + c) % 7;
            int dday = d + this.anchor;

            return dday % 7; // Modulo 7 to make sure we stay between 1-7
        }

        public string getAnchorDay()
        {
            return weekdays[anchorDay()];
        }

        public string getDoomsDay()
        {
            return weekdays[doomsDay()];
        }

        public string getWeekDay()
        {
            int dday = doomsDay();

            // Loop through each doomsday of the month until we find the closest one
            foreach (int day in doomsdays[this.month-1]) {
                if (this.day - day <= 7 && this.day - day >= 0)
                    // Found the closest doomsday. Now add the difference
                    // between the day we're looking for and the closest day
                    // to the doomsday, and use modulo 7 to get the weekday!
                    return weekdays[(dday + this.day - day) % 7];
            }

            return "Bajs"; // If you get this something went really wrong
        }

        public Doomsday(string date_string)
        {
            assertCorrectInput(date_string); // Check that the string is valid
            this.anchor = anchorDay(); // Save the year's anchor day

            // Update January & February doomsdays accordingly if the year is a leap year
            if (isLeapYear()) {
                this.doomsdays[0] = new int[] {4, 11, 18, 25};
                this.doomsdays[1] = new int[] {1, 8, 15, 22, 29};
            }
        }
    }
}
