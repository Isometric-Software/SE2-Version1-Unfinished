using System;

namespace SE2
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new SE2()) game.Run();
        }
    }
}