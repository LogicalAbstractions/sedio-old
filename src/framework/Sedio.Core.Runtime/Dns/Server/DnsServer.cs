using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Sedio.Core.Runtime.Dns.Client;
using Sedio.Core.Runtime.Dns.Protocol;
using Sedio.Core.Runtime.Dns.Protocol.Utils;
using Sedio.Core.Runtime.Dns.RequestResolver;

namespace Sedio.Core.Runtime.Dns.Server
{
    public class DnsServer : IDisposable
    {
        private const int DEFAULT_PORT = 53;
        private const int UDP_TIMEOUT  = 2000;

        public delegate void RequestedEventHandler(IDnsRequest request);

        public delegate void RespondedEventHandler(IDnsRequest request, IDnsResponse response);

        public delegate void ListeningEventHandler();

        public delegate void ErroredEventHandler(Exception e);

        public event RequestedEventHandler Requested;

        public event RespondedEventHandler Responded;

        public event ListeningEventHandler Listening;

        public event ErroredEventHandler Errored;

        private bool             run      = true;
        private bool             disposed = false;
        private UdpClient        udp;
        private IDnsRequestResolver resolver;

        public DnsServer(MasterFile masterFile, IPEndPoint endServer) :
            this(new FallbackDnsRequestResolver(masterFile, new UdpDnsRequestResolver(endServer)))
        {
        }

        public DnsServer(MasterFile masterFile, IPAddress endServer, int port = DEFAULT_PORT) :
            this(masterFile, new IPEndPoint(endServer, port))
        {
        }

        public DnsServer(MasterFile masterFile, string endServer, int port = DEFAULT_PORT) :
            this(masterFile, IPAddress.Parse(endServer), port)
        {
        }

        public DnsServer(IPEndPoint endServer) :
            this(new UdpDnsRequestResolver(endServer))
        {
        }

        public DnsServer(IPAddress endServer, int port = DEFAULT_PORT) :
            this(new IPEndPoint(endServer, port))
        {
        }

        public DnsServer(string endServer, int port = DEFAULT_PORT) :
            this(IPAddress.Parse(endServer), port)
        {
        }

        public DnsServer(IDnsRequestResolver resolver)
        {
            this.resolver = resolver;
        }

        public async Task Listen(int port = DEFAULT_PORT)
        {
            await Task.Yield();

            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            IPEndPoint                   ip  = new IPEndPoint(IPAddress.Any, port);

            if (run)
            {
                try
                {
                    udp = new UdpClient(ip);
                }
                catch (SocketException e)
                {
                    OnErrored(e);
                    return;
                }
            }

            AsyncCallback receiveCallback = null;
            receiveCallback = result =>
            {
                byte[] data;

                try
                {
                    data = udp.EndReceive(result, ref ip);
                    HandleRequest(data, ip);
                }
                catch (ObjectDisposedException)
                {
                    // run should already be false
                    run = false;
                }
                catch (SocketException e)
                {
                    OnErrored(e);
                }

                if (run) udp.BeginReceive(receiveCallback, null);
                else tcs.SetResult(null);
            };

            udp.BeginReceive(receiveCallback, null);
            OnListening();
            await tcs.Task;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void OnRequested(IDnsRequest request)
        {
            RequestedEventHandler handlers = Requested;
            if (handlers != null) handlers(request);
        }

        protected virtual void OnResponded(IDnsRequest request, IDnsResponse response)
        {
            RespondedEventHandler handlers = Responded;
            if (handlers != null) handlers(request, response);
        }

        protected virtual void OnListening()
        {
            ListeningEventHandler handlers = Listening;
            if (handlers != null) handlers();
        }

        protected virtual void OnErrored(Exception e)
        {
            ErroredEventHandler handlers = Errored;
            if (handlers != null) handlers(e);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;

                if (disposing)
                {
                    run = false;
                    udp?.Dispose();
                }
            }
        }

        private async void HandleRequest(byte[] data, IPEndPoint remote)
        {
            DefaultDnsRequest request = null;

            try
            {
                request = DefaultDnsRequest.FromArray(data);
                OnRequested(request);

                IDnsResponse response = await resolver.Resolve(request);

                OnResponded(request, response);
                await udp
                    .SendAsync(response.ToArray(), response.Size, remote)
                    .WithCancellationTimeout(UDP_TIMEOUT);
            }
            catch (SocketException e)
            {
                OnErrored(e);
            }
            catch (ArgumentException e)
            {
                OnErrored(e);
            }
            catch (IndexOutOfRangeException e)
            {
                OnErrored(e);
            }
            catch (OperationCanceledException e)
            {
                OnErrored(e);
            }
            catch (IOException e)
            {
                OnErrored(e);
            }
            catch (ObjectDisposedException e)
            {
                OnErrored(e);
            }
            catch (DnsResponseException e)
            {
                IDnsResponse response = e.DnsResponse;

                if (response == null)
                {
                    response = DefaultDnsResponse.FromRequest(request);
                }

                try
                {
                    await udp
                        .SendAsync(response.ToArray(), response.Size, remote)
                        .WithCancellationTimeout(UDP_TIMEOUT);
                }
                catch (SocketException)
                {
                }
                catch (OperationCanceledException)
                {
                }
                finally
                {
                    OnErrored(e);
                }
            }
        }

        private class FallbackDnsRequestResolver : IDnsRequestResolver
        {
            private IDnsRequestResolver[] resolvers;

            public FallbackDnsRequestResolver(params IDnsRequestResolver[] resolvers)
            {
                this.resolvers = resolvers;
            }

            public async Task<IDnsResponse> Resolve(IDnsRequest request)
            {
                IDnsResponse response = null;

                foreach (IDnsRequestResolver resolver in resolvers)
                {
                    response = await resolver.Resolve(request);
                    if (response.AnswerRecords.Count > 0) break;
                }

                return response;
            }
        }
    }
}