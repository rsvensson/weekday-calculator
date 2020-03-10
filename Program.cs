using System;

namespace weekday_calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter a date in format YYYY.MM.DD: ");
            string date = Console.ReadLine();

            Doomsday dday = new Doomsday(date);
            Console.WriteLine($"\n\nThe date {date} is a {dday.getWeekDay()}.\n\n");
            //Console.WriteLine($"The century's 'anchor date' is a {dday.getAnchorDay()}.");
            //Console.WriteLine($"The year's 'doom's day' is a {dday.getDoomsDay()}.");
        }
    }
}
