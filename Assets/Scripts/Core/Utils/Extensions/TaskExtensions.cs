using System;
using System.Threading.Tasks;

namespace Core.Utils.Extensions
{
    public static class TaskExtensions
    {
        public static void ContinueWithOnMainThread<T>(
            this Task<T> task,
            Action<Task<T>> continuation)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (continuation == null) throw new ArgumentNullException(nameof(continuation));

            task.ContinueWith(t =>
            {
                MainThreadDispatcher.Enqueue(() => continuation(t));
            });
        }

        // If you ever need non-generic Task:
        public static void ContinueWithOnMainThread(
            this Task task,
            Action<Task> continuation)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (continuation == null) throw new ArgumentNullException(nameof(continuation));

            task.ContinueWith(t =>
            {
                MainThreadDispatcher.Enqueue(() => continuation(t));
            });
        }
    }
}