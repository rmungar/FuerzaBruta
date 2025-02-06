using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        List<string> lines = File.ReadLines("C:\\Users\\usuario\\RiderProjects\\FuerzaBruta\\FuerzaBruta\\passwords.txt").ToList();
        Random random = new Random();
        int numeroDeHilos = 1;
        long mejorTiempo = long.MaxValue;

        while (true)
        {
            string aCrackear = lines[random.Next(lines.Count)];
            string hashedPassword = GetHashString(aCrackear);

            int chunkSize = (int)Math.Ceiling((double)lines.Count / numeroDeHilos);
            CancellationTokenSource cts = new CancellationTokenSource();
            Stopwatch cronometro = Stopwatch.StartNew();

            Task[] tasks = new Task[numeroDeHilos];
            for (int i = 0; i < numeroDeHilos; i++)
            {
                int start = i * chunkSize;
                int end = Math.Min(start + chunkSize, lines.Count);
                
                tasks[i] = Task.Run(() =>
                {
                    for (int j = start; j < end; j++)
                    {
                        if (cts.Token.IsCancellationRequested) break;
                        if (GetHashString(lines[j]) == hashedPassword)
                        {
                            Console.WriteLine($"La password era: {lines[j]} con {numeroDeHilos} hilos");
                            cts.Cancel();
                            break;
                        }
                    }
                }, cts.Token);
            }

            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException) { }
            
            cronometro.Stop();

            long tiempoConcurrido = cronometro.ElapsedMilliseconds;
            Console.WriteLine($"Tiempo con {numeroDeHilos} hilos: {tiempoConcurrido} ms");

            if (tiempoConcurrido >= mejorTiempo)
                break;

            mejorTiempo = tiempoConcurrido;
            numeroDeHilos++;
        }
    }

    static string GetHashString(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }
    }
}
