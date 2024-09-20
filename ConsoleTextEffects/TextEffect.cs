using System;
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
                var random = new Random(Thread.CurrentThread.GetHashCode());
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
                bool visible = true;
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
    }
}
