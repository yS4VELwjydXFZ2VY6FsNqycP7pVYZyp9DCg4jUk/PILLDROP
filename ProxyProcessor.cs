using System;
using System.Collections.Generic;
using System.Net;

namespace PILLDROP
{
    /// <summary>
    /// Basic proxy utilities.
    /// </summary>
    internal struct ProxyProcessor
    {
        #region Variables
        /// <summary>
        /// The proxy scrape URL.
        /// </summary>
        private const string scrape =
            "https://api.proxyscrape.com/v2/?request=getproxies&protocol=http&timeout=500&country=all&ssl=all&anonymity=all&simplified=true";

        /// <summary>
        /// All cached proxies.
        /// </summary>
        public static readonly List<ProxyEntry> proxies = new();

        /// <summary>
        /// A single <see cref="WebClient"/> instance.
        /// </summary>
        private static readonly WebClient client = new();

        /// <summary>
        /// A single <see cref="Random"/> instance.
        /// </summary>
        private static readonly Random rand = new();
        #endregion

        /// <summary>
        /// Grab / Refresh all proxies.
        /// </summary>
        public static void grab_proxies()
        {
            // Clear the proxies to prevent duplicates and outdated entries.
            proxies.Clear();
            const char split = ':';
            var data = client.DownloadString(scrape).Split('\n');
            for (var i = 0; i < data.Length - 1; i++) // -1 because the last entry is empty.
            {
                if (!data[i].Contains(split)) continue;
                var raw = data[i].Split(split);
                proxies.Add(new ProxyEntry {ip = raw[0], port = Convert.ToUInt16(raw[1])});
            }
        }
        
        /// <summary>
        /// Get a random proxy.
        /// </summary>
        /// <returns>A random proxy from <see cref="proxies"/>. If empty, a new non-existent proxy is returned.</returns>
        public static ProxyEntry get_random_proxy() =>
            proxies.Count <= 0 ? new ProxyEntry() : proxies[rand.Next(0, proxies.Count)];
    }
}
