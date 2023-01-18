using System.Collections.Concurrent;

using Ze.Tasks.Runner;

namespace Ze.Tasks.Messages;

// apache 2.0
// https://github.com/xunit/xunit/blob/77a3667654612e3ef7ba870f4e534c8ac7bebd13/src/xunit.v3.core/Internal/MessageBus.cs
public sealed class MessageBus : IMessageBus
{
    private readonly IMessageSink sink;
    private readonly ConcurrentQueue<IMessage> queue = new();
    private readonly Thread dispatchThread;
    private readonly AutoResetEvent resetEvent = new(initialState: false);
    private readonly bool stopOnFail;
    private volatile bool shutdownRequested;
    private volatile bool continueRunning = true;
    private bool disposed;

    public MessageBus(IMessageSink messageSink, bool stopOnFail)
    {
        this.sink = messageSink;
        this.stopOnFail = stopOnFail;
        this.dispatchThread = new Thread(this.Run);
        this.dispatchThread.Start();
    }

    public void Dispose()
    {
        if (this.disposed)
            return;

        this.disposed = true;
        this.shutdownRequested = true;
        this.resetEvent.Set();
        this.dispatchThread.Join();
        this.resetEvent.Dispose();
    }

    public bool QueueMessage(IMessage message)
    {
        if (this.shutdownRequested)
            throw new ObjectDisposedException("MessageBus");

        if (this.stopOnFail && message is IErrorMessage)
            this.continueRunning = false;

        this.queue.Enqueue(message);
        this.resetEvent.Set();
        return this.continueRunning;
    }

    private void Dispatch()
    {
        while (this.queue.TryDequeue(out var message))
        {
            try
            {
                // ReSharper disable once NonAtomicCompoundOperator
                this.continueRunning &= this.sink.OnMessage(message);
            }
            catch (Exception ex)
            {
                try
                {
                    var errorMessage = new ErrorMessage(ex);
                    if (!this.sink.OnMessage(errorMessage))
                        this.continueRunning = false;
                }
                catch
                {
                    // suppress all exceptions from failure to send message;
                }
            }
        }
    }

    private void Run()
    {
        while (!this.shutdownRequested)
        {
            this.resetEvent.WaitOne();
            this.Dispatch();
        }

        // One final dispatch pass
        this.Dispatch();
    }
}