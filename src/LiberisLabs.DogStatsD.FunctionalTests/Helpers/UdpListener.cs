using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace LiberisLabs.DogStatsD.FunctionalTests.Helpers
{
    /// <summary>
    /// Udp listener for capturing Udp traffic
    /// </summary>
    public class UdpListener : IDisposable
    {
        private readonly UdpClient _listener;
        private readonly List<string> _stats;

        /// <summary>
        /// Construct a new UdpListener to listen for all local Udp traffic
        /// </summary>
        /// <param name="port">Udp port to listen on. 8125 by default</param>
        public UdpListener(int port = 8125)
        {
            _stats = new List<string>();

            _listener = new UdpClient(port);
        }

        /// <summary>
        /// Start listening to Udp traffic
        /// </summary>
        public void Start()
        {
            _listener.BeginReceive(Handle, _listener);
        }

        private void Handle(IAsyncResult result)
        {
            try
            {
                var receivedIpEndPoint = new IPEndPoint(IPAddress.Loopback, 8125);
                var data = _listener.EndReceive(result, ref receivedIpEndPoint);

                _stats.Add(Encoding.ASCII.GetString(data));
            }
            catch
            {
            }
            finally
            {
                try
                {
                    _listener.BeginReceive(Handle, null);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Stop the UdpListener server
        /// </summary>
        public void Stop()
        {
            Dispose();
        }

        /// <summary>
        /// Dispose of the UdpListener
        /// </summary>
        public void Dispose()
        {
            _listener.Close();
        }

        /// <summary>
        /// Matches statistics that have been gathered for the specific <param name="metric"> name</param>
        /// </summary>
        /// <param name="metric"></param>
        /// <returns></returns>
        public bool Handled(string metric)
        {
            return _stats.Contains(metric);
        }

        /// <summary>
        /// Matches statistics that have been gathered for the specified <param name="regexPattern"></param>
        /// </summary>
        /// <param name="regexPattern">The Regex pattern to match</param>
        /// <returns></returns>
        public bool HandledPattern(string regexPattern)
        {
            return _stats.Any(x => Regex.IsMatch(x, regexPattern, RegexOptions.None));
        }
    }
}
