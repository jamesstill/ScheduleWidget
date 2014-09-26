using System;

namespace ScheduleWidget.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            RunDailyScenarios();
            RunWeeklyScenarios();
            RunMonthlyScenarios();

            Console.WriteLine();
            Console.WriteLine("Finished running all scenarios. Press ENTER to exit.");
            Console.ReadLine();
        }

        private static void RunDailyScenarios()
        {
            DailyScenario1.Run();
            DailyScenario2.Run();
        }

        private static void RunWeeklyScenarios()
        {
            WeeklyScenario1.Run();
            WeeklyScenario2.Run(); 
        }

        private static void RunMonthlyScenarios()
        {
            MonthlyScenario1.Run();
            MonthlyScenario2.Run();
        }
    }
}
