namespace Core.Events
{
    public interface IEventType
    {
        int Value { get; }
        string Name { get; }
    }
}