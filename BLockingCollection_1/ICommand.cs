namespace BLockingCollection_1
{
    public interface ICommand
    {
        Task<object> Execute();
    }
}
