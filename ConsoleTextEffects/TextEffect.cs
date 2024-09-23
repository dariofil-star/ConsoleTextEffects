using System;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTextEffects
{
    public static class TextEffect
    {
        private static object _lockObject = new object();
        public static Task SlideText(int line, string text, int delay = 100, int width = 0)
        {
            return Task.Factory.StartNew(() =>
            {
                if (width == 0)
                    width = Console.BufferWidth - text.Length;

                var fullString = string.Empty.PadRight(width) + text;

                for (var i = width; i > 0; i--)
                {
                    fullString = fullString.Remove(0, 1);
                    fullString = fullString + " ";

                    lock (_lockObject)
                    {
                        Console.CursorTop = line;
                        Console.CursorLeft = 0;
                        Console.Write(fullString);
                    }

                    Thread.Sleep(delay);
                }
            });
        }

        public static Task TrickleText(int line, string text, int delay = 100, int length = 0)
        {
            return Task.Factory.StartNew(() =>
            {
                if (length == 0)
                    length = Console.BufferWidth - text.Length;

                var fullString = string.Empty.PadRight(length) + text;

                for (var x = 0; x < text.Length; x++)
                {
                    for (var i = length; i > 0; i--)
                    {
                        fullString = fullString.Remove(x, 1);
                        fullString = fullString.Insert(length + x, " ");

                        lock (_lockObject)
                        {
                            Console.CursorLeft = 0;
                            Console.CursorTop = line;
                            Console.Write(fullString);
                        }

                        Thread.Sleep(delay);
                    }
                }
            });
        }

        public static Task MatrixText(int line, string text, int delay = 5)
        {
            return Task.Factory.StartNew(() =>
            {
                var random = new Random(Thread.CurrentThread.ManagedThreadId);
                var chars =
                    "#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[]^_`abcdefghijklmnopqrstuvwxyz{|}~";


                var matrixString = string.Empty;

                foreach (var c in text)
                {
                    if (c == 32)
                    {
                        matrixString = matrixString + " ";
                        continue;
                    }

                    matrixString = matrixString + chars[random.Next(0, chars.Length)];
                }

                var scrambledString = matrixString;

                while (!string.Equals(scrambledString, text))
                {
                    lock (_lockObject)
                    {
                        Console.CursorTop = line;
                        Console.CursorLeft = 0;
                        Console.Write(scrambledString);
                    }

                    var index = 0;
                    foreach (var c in scrambledString)
                    {
                        if (c == 32 || c.ToString() == text.Substring(index, 1))
                        {
                            index++;
                            continue;
                        }

                        scrambledString = scrambledString.Remove(index, 1)
                            .Insert(index, chars[random.Next(0, chars.Length)].ToString());

                        index++;
                    }

                    Thread.Sleep(delay);
                }

                lock (_lockObject)
                {
                    Console.CursorLeft = 0;
                    Console.CursorTop = line;
                    Console.WriteLine(scrambledString);
                }
            });
        }

        public static Task CenterText(int line, string text, int width = 80)
        {
            return Task.Factory.StartNew(() =>
            {
                lock (_lockObject)
                {
                    Console.CursorTop = line;
                    Console.CursorLeft = (width / 2) - (text.Length / 2);
                    Console.WriteLine(text);
                }
            });
        }

        public static Task WriteBlinkingText(int line, string text, int delay, CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource == null)
                cancellationTokenSource = new CancellationTokenSource();

            return Task.Factory.StartNew(() =>
            {
                var visible = true;
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    lock (_lockObject)
                    {
                        Console.CursorTop = line;
                        Console.CursorLeft = 0;
                        Console.WriteLine((visible ? text : new string(' ', text.Length)));
                        Thread.Sleep(delay);
                        visible = !visible;
                    }
                }
            }, cancellationTokenSource.Token);
        }

        public static Task MatrixText(int width, int height, CancellationToken token)
        {
            if (width == 0) width = Console.WindowWidth;
            if (height == 0) height = Console.WindowHeight;

            return Task.Factory.StartNew(() =>
            {
                var random = new Random(Thread.CurrentThread.ManagedThreadId);
                // Initialize columns with random heights
                var columns = new int[width];
                for (var i = 0; i < columns.Length; i++)
                {
                    columns[i] = random.Next(height);
                }

                while (!token.IsCancellationRequested)
                {
                    for (var x = 0; x < columns.Length; x++)
                    {
                        // Randomly decide if the current column should print a character or not
                        if (random.Next(10) < 2)
                        {
                            Console.SetCursorPosition(x, columns[x]);
                            Console.ForegroundColor = GetRandomGreenShade(random);
                            Console.Write(GetRandomCharacter(random));

                            // Move the column position down
                            columns[x]++;

                            // Reset column to the top when it reaches the bottom
                            if (columns[x] >= height)
                            {
                                columns[x] = 0;
                            }
                        }
                    }

                    Thread.Sleep(50); // Small delay to control the speed of the effect
                }

                return Task.CompletedTask;
            }, token);
        }

        static char GetRandomCharacter(Random random)
        {
            return (char)random.Next(32, 126);
        }

        static ConsoleColor GetRandomGreenShade(Random random)
        {
            // Return different shades of green to simulate the matrix style text
            var colorChoice = random.Next(3);
            switch (colorChoice)
            {
                case 0: return ConsoleColor.DarkGreen;
                case 1: return ConsoleColor.Green;
                case 2: return ConsoleColor.White;
            }
            return ConsoleColor.Green;
        }

        public static Task BlurbText(int line, string text, string justification, int delay = 50)
        {
            return Task.Factory.StartNew(() =>
            {
                var random = new Random(Thread.CurrentThread.ManagedThreadId);

                StringBuilder displayBuilder = new StringBuilder(new string(' ', text.Length));

                for (int i = 0; i < text.Length; i++)
                {
                    if (string.Equals(text[i].ToString(), " "))
                    {
                        displayBuilder[i] = (char)32;
                    }
                    else
                    {
                        displayBuilder[i] = (char)random.Next(32, 126); // A-Z
                    }

                    Console.CursorTop = line;
                    Console.WriteLine(displayBuilder);
                    Thread.Sleep(delay);
                }

                for (int i = 0; i < text.Length; i++)
                {
                    displayBuilder[i] = text[i];
                    Console.CursorTop = line;
                    Console.WriteLine(displayBuilder);
                    Thread.Sleep(delay);
                }

                return Task.CompletedTask;
            });
        }

        
    }
}
