using System;
using System.Threading;

namespace ConsoleTextEffects
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("These examples will run Synchronously...");

            TextEffect.MatrixText(1, "This is an example of matrix text effect This is an example of matrix text effect").Wait();

            TextEffect.SlideText(2, "This is an example of slide text effect", 20).Wait();

            TextEffect.TrickleText(3, "This is an example of trickle text effect", 1, Console.BufferWidth / 3).Wait();

            TextEffect.CenterText(4, "Example of centered text", Console.BufferWidth).Wait();

            var cancellationTokenSource = new CancellationTokenSource();
            TextEffect.WriteBlinkingText(5, "Example of blinking text", 150, cancellationTokenSource).Wait(TimeSpan.FromSeconds(10));
            cancellationTokenSource.Cancel();

            Console.WriteLine("Press any key to try Asynchronously...");
            Console.ReadKey();


            Console.Clear();
            Console.WriteLine("These examples will run Asynchronously...");
            //You will notice blinking text slows things down significantly because of thread delay and lockobject...

            TextEffect.MatrixText(1, "This is an example of matrix text effect This is an example of matrix text effect");

            TextEffect.SlideText(2, "This is an example of slide text effect", 50, Console.BufferWidth);

            TextEffect.TrickleText(3, "This is an example of trickle text effect", 5, Console.BufferWidth / 2);

            TextEffect.CenterText(4, "Example of centered text", Console.BufferWidth);

            var cancellationTokenSource2 = new CancellationTokenSource();
            TextEffect.WriteBlinkingText(5, "Example of blinking text", 150, cancellationTokenSource2).Wait(TimeSpan.FromSeconds(10));
            cancellationTokenSource2.Cancel();

            Console.WriteLine("Done! Press any key to exit...");
            Console.ReadKey();
        }
    }
}
