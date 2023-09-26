using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace VortexPings.Models
{

    public class Node : IDisposable
    {
        private bool disposedValue;

        private bool _IsInPingerQueue;
        public bool IsInPingerQueue { get { return _IsInPingerQueue; }
            set 
            { 

                if (CancellationTokenSource != null&& CancellationTokenSource.IsCancellationRequested)
                {
                    CancellationTokenSource.Dispose();
                    CancellationTokenSource = new CancellationTokenSource();
                }
                _IsInPingerQueue = value;
                IsInPingerQueueChanged?.Invoke();
            } 
        }
        public NodeData? NodeData { get;  set; }

        public PingResultData? PingResultData { get; private set; }

        public int Order { get; set; }

        public event Action PingResultDataUpdated;
        public event Action IsInPingerQueueChanged;

        private bool isContainError;

        private System.Net.NetworkInformation.Ping NodePing { get; } = new System.Net.NetworkInformation.Ping();

        internal CancellationTokenSource? CancellationTokenSource { get; private set; } = new CancellationTokenSource();
        private TaskCompletionSource<PingReply> pingTaskCompletionSource = null;

        public Node()
        {
            NodeData = new NodeData();
            PingResultData = new PingResultData();
            NodePing.PingCompleted += new PingCompletedEventHandler(OnPingCompleted);
        }
        public async Task<Node> PingAsync()
        {
            try
            {
                pingTaskCompletionSource = new TaskCompletionSource<PingReply>();
               
                using (CancellationTokenSource.Token.Register(() => { pingTaskCompletionSource.TrySetCanceled(); }))
                {

                    NodePing.SendAsync(NodeData.HostOrIPadress, (int)NodeData.TimeOut, NodeData.Buffer, NodeData.PingOptions, CancellationTokenSource.Token);

                    var pingReply = await pingTaskCompletionSource.Task;

                    var pingReplayResult = new PingReplyResult(pingReply);
                    СreatePingResultData(pingReplayResult);              
                }
            }
            catch (OperationCanceledException ex)
            {


            }
            catch (SocketException ex)
            {

            }
            catch (PingException ex)
            {

            }
            catch (Exception ex)
            {
               if(ex.InnerException!=null&&ex.InnerException.InnerException!=null)
                {
                    var socketException = ex.InnerException.InnerException as SocketException;
                    if (socketException != null)
                    {
                        var pingReplayResult = new PingReplyResult(socketException.SocketErrorCode.ToString(), null, 0);
                        СreatePingResultData(pingReplayResult);
                    }
                    else
                    {
                        var pingReplayResult = new PingReplyResult("DestinationHostUnreachable", null, 0);
                        СreatePingResultData(pingReplayResult);
                    }
                }
              
            }
            finally
            {
                if (PingResultData?.LastRoundTripTime != null)
                {
                    int remainingTime = (int)NodeData.PingRepeatTime - (int)PingResultData.LastRoundTripTime;
                    if (remainingTime > 0)
                    {
                        await Task.Delay(remainingTime, CancellationTokenSource.Token);
                    }
                }
                PingResultDataUpdated?.Invoke();

                if(isContainError)
                {
                    await Task.Delay(1000, CancellationTokenSource.Token);
                }
            }

            return this;
        }

        private  void OnPingCompleted(object sender, PingCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                pingTaskCompletionSource.TrySetCanceled();
            }
            else if (e.Error != null)
            {
                pingTaskCompletionSource.TrySetException(e.Error);
                isContainError = true;
            }
            else
            {
                pingTaskCompletionSource.TrySetResult(e.Reply);
                isContainError = false;

            }
        }

        private void СreatePingResultData(PingReplyResult pingReply)
        {
            PingResultData.LastRoundTripTime = pingReply.RoundtripTime;
            PingResultData.PingResult = pingReply.Status;

            if (pingReply.Address != null)
            {
                PingResultData.ResponseAdress = pingReply.Address.ToString();
            }
            else
            {
                PingResultData.ResponseAdress = "None";
            }

            PingResultData.DateTime = DateTime.Now;
            StatusSelector(pingReply);

        }

        private void StatusSelector(PingReplyResult pingReply)
        {
            if (pingReply.Status == "Success")
            {
                if (PingResultData.LastRoundTripTime >= NodeData.WarningTime)
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

        protected async virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)

                    CancellationTokenSource.Cancel();

                    if (pingTaskCompletionSource != null)
                    {
                        var task = pingTaskCompletionSource.Task;
                        while (task.IsCompleted == false || IsInPingerQueue == true)
                        {
                            await Task.Delay(100);
                        }
                    }
                }

                NodePing.PingCompleted -= OnPingCompleted;
                NodePing.Dispose();
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
