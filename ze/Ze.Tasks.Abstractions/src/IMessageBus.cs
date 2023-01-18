using Ze.Tasks;

namespace Ze.Tasks;

public interface IMessageBus : IDisposable
{
    bool QueueMessage(IMessage message);
}