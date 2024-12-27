namespace MessagePublisher.Interfaces;

public delegate Task OnDataReceiveEvent<in T>(T data);

public interface IMessagePublisher
{
    Task Subscribe<T>(string queueName, OnDataReceiveEvent<T>? onReceive);

    Task PushAsync<T>(string queueName, T data);
}