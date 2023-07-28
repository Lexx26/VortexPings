using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace VortexPings.Models
{

    public class Node : IDisposable
    {
        private bool disposedValue;

        public NodeData? NodeData { get; set; }

        public int Order { get; set; }


        public event Action PingResultDataUpdated;

        public PingResultData? PingResultData { get; private set; }

        private System.Net.NetworkInformation.Ping Pinger { get; } = new System.Net.NetworkInformation.Ping();

        public CancellationTokenSource? CancellationTokenSource { get; private set; } = new CancellationTokenSource();
        private TaskCompletionSource<PingReply> pingTaskCompletionSource = null;
        public async Task PingAsync(int minPingTime)
        {

            var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(CancellationTokenSource.Token);
            try
            {
                pingTaskCompletionSource = new TaskCompletionSource<PingReply>();
                Pinger.PingCompleted += OnPingCompleted;

                using (cancellationTokenSource.Token.Register(() => { pingTaskCompletionSource.TrySetCanceled(); }))
                {
                    
                    Pinger.SendAsync(NodeData.HostOrIPadress, NodeData.TimeOut, NodeData.Buffer, NodeData.PingOptions);

                    PingReply pingReply = await pingTaskCompletionSource.Task;

                    int remainingTime = minPingTime - (int)pingReply.RoundtripTime;
                    if (remainingTime > 0)
                    {
                        await Task.Delay(remainingTime, cancellationTokenSource.Token);
                    }

                    СreatePingResultData(pingReply);
                    PingResultDataUpdated?.Invoke();
                }
            }
            catch (OperationCanceledException)
            {
                CancellationTokenSource.Dispose();
                CancellationTokenSource = new CancellationTokenSource();

            }
            catch (PingException ex)
            {

            }
            finally
            {

            }

        }

        private void OnPingCompleted(object sender, PingCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                pingTaskCompletionSource.TrySetCanceled();
            }
            else if (e.Error != null)
            {
                pingTaskCompletionSource.TrySetException(e.Error);
            }
            else
            {
                pingTaskCompletionSource.TrySetResult(e.Reply);
            }
        }

        private void СreatePingResultData(PingReply pingReply)
        {
            PingResultData = new PingResultData();
            PingResultData.lastRoundTripeTime = pingReply.RoundtripTime;
            PingResultData.PingResult = pingReply.Status.ToString();
            if (PingResultData.PingResult == "11050")
            {
                PingResultData.PingResult = "DestinationHostUnreachable";
            }

            if (pingReply.Address != null)
            {
                PingResultData.ResponseAdress = pingReply.Address.ToString();
            }
            else
            {
                PingResultData.ResponseAdress = "None";
            }

            StatusSelector(pingReply);

        }

        private void StatusSelector(PingReply pingReply)
        {
            if (pingReply.Status == IPStatus.Success)
            {
                if (PingResultData.lastRoundTripeTime >= NodeData.WarningTime)
                {
                    PingResultData.PingStatus = PingStatus.Yellow;
                    return;
                }
                PingResultData.PingStatus = PingStatus.Green;

            }
            else
            {
                PingResultData.PingStatus = PingStatus.Red;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

               

                Pinger.PingCompleted -= OnPingCompleted;
                Pinger.Dispose();
                CancellationTokenSource?.Dispose();

                disposedValue = true;
            }
        }

       
        ~Node()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

}
