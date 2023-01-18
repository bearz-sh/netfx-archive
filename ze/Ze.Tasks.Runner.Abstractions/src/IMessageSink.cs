namespace Ze.Tasks.Runner;

public interface IMessageSink
{
    bool OnMessage(IMessage message);
}