using BLockingCollection_1;
using System.Collections.Concurrent;

public class Program
{
    public async static Task Main(string[] args)
    {
        var blockingCollection = new BlockingCollection<ICommand>();

        var taskList = new List<Task>();
        int j = 0;
        taskList.Add(Task.Run(() =>
        {
            var increment = Interlocked.Increment(ref j);
            var func = () => Task.FromResult(increment + 5);
            blockingCollection.Add(new QueueItem<int>(func));
        })
        );

        taskList.Add(Task.Run(() =>
        {
            var increment = Interlocked.Increment(ref j);
            var func = () => Task.FromResult(increment + 5);
            blockingCollection.Add(new QueueItem<int>(func));
        })
       );

        taskList.Add(Task.WhenAll(taskList).ContinueWith(t => blockingCollection.CompleteAdding()));

        taskList.Add(Task.Run(() =>
        {
            try
            {
                while (!blockingCollection.IsCompleted)
                {
                    var result = blockingCollection.Take();
                    var result1=result.Execute();
                    Console.WriteLine(result1.Result);
                }
            }
            catch
            {

            }

        })
        );
        await Task.WhenAll(taskList.ToArray());
        //Console.WriteLine("Count: " + blockingCollection.Count);
        //Console.WriteLine("Hello World");
    }
}