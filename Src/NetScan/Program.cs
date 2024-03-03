using System.Net.Sockets;

namespace NetScan;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var iprange = IPv4Range.Create(args[0]);
        var port = int.Parse(args[1]);

        var silent = args.Length > 2 && args[2] == "-s";

        var opts = new ParallelOptions { MaxDegreeOfParallelism = 20 };

        await Parallel.ForEachAsync(iprange.GetIps(), opts, async (ip, ct) =>
        {
            try
            {
                using var ctx = new CancellationTokenSource(TimeSpan.FromSeconds(1));
                using var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                await socket.ConnectAsync(ip, port, ctx.Token);

                if (silent)
                {
                    Console.WriteLine(ip);
                }
                else
                {
                    Console.WriteLine($"{ip}:{port} UP");
                }
            }
            catch (OperationCanceledException)
            {
                if (!silent)
                {
                    Console.WriteLine($"{ip}:{port} timeout");
                }
            }
            catch (Exception ex)
            {
                if (!silent)
                {
                    Console.WriteLine($"Ip {ip} failed with {ex.Message}");
                }
            }
        });
    }
}
