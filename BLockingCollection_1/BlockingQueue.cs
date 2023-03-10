using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLockingCollection_1
{
    public class BlockingQueue : IAsyncDisposable
    {
        private readonly BlockingCollection<ICommand> blockingCollection;
        private readonly Task mainTask;
        public BlockingQueue()
        {
            blockingCollection = new BlockingCollection<ICommand>();
            mainTask = Task.Run(async () =>
              {
                  while (!blockingCollection.IsCompleted)
                  {
                      try
                      {
                          var i = blockingCollection.Take();
                          var result = await i.Execute();

                          Console.WriteLine(result);
                      }
                      catch (Exception)
                      {
                          continue;
                      }
                  }
              });
        }

        public async ValueTask DisposeAsync()
        {
            blockingCollection.CompleteAdding();
            await mainTask;
            blockingCollection.Dispose();
        }

        public Task<T> ExecuteAsync<T>(Func<Task<T>> function)
        {
            var item = new QueueItem<T>(function);
            blockingCollection.Add(item);
            return item.Completion;
        }
    }
}
