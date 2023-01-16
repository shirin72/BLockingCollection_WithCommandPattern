namespace BLockingCollection_1
{
    public class QueueItem<T> : ICommand
    {
        private readonly Func<Task<T>> action;
        private readonly TaskCompletionSource<T> taskCompletionSource;
        public Task<T> Completion => taskCompletionSource.Task;
        public QueueItem(Func<Task<T>> action)
        {
            this.action = action;
            taskCompletionSource = new TaskCompletionSource<T>();
        }


        public async Task<object> Execute()
        {
            var result = await action();

            taskCompletionSource.SetResult(result);

           return result;
        }
    }
}
