using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Common.Threading
{
    public static class TaskExtension
    {
        public static void WaitAll<T> (this IEnumerable<T> collection, Func<T, Task> taskFunc)
        {
            int count = collection.Count();
            if (count == 0)
                return;

            Task[] tasks = new Task[count];

            int i = 0;
            foreach(var t in collection)
            {
                tasks[i] = taskFunc.Invoke(t);
                

                ++i;
            }

            Task.WaitAll(tasks);
        }
    }
}
