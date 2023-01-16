using BLockingCollection_1;
using System.Collections.Concurrent;

public class Program
{
    public async static Task Main(string[] args)
    {
        var queue = new BlockingQueue();

        var taskList = new List<Task>();
        int j = 0;
        
        taskList.Add(Task.Run(async () =>
        {
            await queue.ExecuteAsync(() => Task.FromResult(Interlocked.Increment(ref j) + 5));
        }));
        
        taskList.Add(Task.Run(async () =>
        {
            await queue.ExecuteAsync(() => Task.FromResult(Interlocked.Increment(ref j) - 5));
        }));

        var allTasks=new List<Task>(taskList);
        allTasks.Add(Task.WhenAll(taskList).ContinueWith(async t => await queue.DisposeAsync()));

        await Task.WhenAll(allTasks.ToArray());
    }
}