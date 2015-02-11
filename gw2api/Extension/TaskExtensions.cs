using System;
using System.Threading;
using System.Threading.Tasks;

namespace gw2api.Extension
{
    public static class TaskExtensions
    {
        /// <summary>Transfers the result of a Task to the TaskCompletionSource.</summary>
        /// <typeparam name="TResult">Specifies the type of the result.</typeparam>
        /// <param name="resultSetter">The TaskCompletionSource.</param>
        /// <param name="task">The task whose completion results should be transferred.</param>
        /// Copyright (c) Microsoft Corporation.  All rights reserved. 
        public static void SetFromTask<TResult>(this TaskCompletionSource<TResult> resultSetter, Task task)
        {
            switch (task.Status)
            {
                case TaskStatus.RanToCompletion:
                    var taskResult = task as Task<TResult>;
                    resultSetter.SetResult(taskResult != null ? taskResult.Result : default(TResult)); break;
                case TaskStatus.Faulted: resultSetter.SetException(task.Exception.InnerExceptions); break;
                case TaskStatus.Canceled: resultSetter.SetCanceled(); break;
                default: throw new InvalidOperationException("The task was not completed.");
            }
        }

        /// <summary>Transfers the result of a Task to the TaskCompletionSource.</summary>
        /// <typeparam name="TResult">Specifies the type of the result.</typeparam>
        /// <param name="resultSetter">The TaskCompletionSource.</param>
        /// <param name="task">The task whose completion results should be transferred.</param>
        /// Copyright (c) Microsoft Corporation.  All rights reserved. 
        public static void SetFromTask<TResult>(this TaskCompletionSource<TResult> resultSetter, Task<TResult> task)
        {
            SetFromTask(resultSetter, (Task)task);
        }

        public static Task<TResult> Then<TResult, TSource>(this Task<TSource> task,
            Func<Task<TSource>, TResult> continuationFunction)
        {
            TaskCompletionSource<TResult> completionSource = new TaskCompletionSource<TResult>();

            // If task was successful
            task.ContinueWith(continuationFunction, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(t =>
                {
                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        completionSource.SetFromTask(t);
                    }
                });

            // If task was unsuccessful
            task.ContinueWith(t => completionSource.SetFromTask(t), TaskContinuationOptions.NotOnRanToCompletion);

            return completionSource.Task;
        }

        public static Task Then<TSource>(this Task<TSource> task,
            Action<Task<TSource>> continuationFunction)
        {
            return task.Then(t =>
            {
                continuationFunction(t);
                return Nothing.AtAll;
            });
        }

        public static Task StartSTATask(Action action)
        {
            return StartSTATask(() =>
            {
                action();
                return Nothing.AtAll;
            });
        }

        public static Task<T> StartSTATask<T>(Func<T> function)
        {
            var completionSource = new TaskCompletionSource<T>();
            var thread = new Thread(() =>
            {
                try
                {
                    completionSource.SetResult(function());
                }
                catch (Exception e)
                {
                    completionSource.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return completionSource.Task;
        }

        public static Task EmptyTask()
        {
            var completionSource = new TaskCompletionSource<Nothing>();
            completionSource.SetResult(Nothing.AtAll);
            return completionSource.Task;
        }
    }

    /// <summary>
    /// Unit class. Alternatively, you can use System.Reactive.Unit
    /// </summary>
    public sealed class Nothing
    {
        private Nothing() { }
        private readonly static Nothing atAll = new Nothing();
        public static Nothing AtAll
        {
            get { return atAll; }
        }
    }
}
