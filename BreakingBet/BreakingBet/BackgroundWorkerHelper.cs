using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
//using Microsoft.Practices.ObjectBuilder2;
//using Microsoft.Practices.Unity;

namespace BreakingBet
{
    /// <summary>
    /// Helper for backgroung execution
    /// </summary>
    public class BackgroundWorkerHelper
    {
        private static Dictionary<Guid, Thread> _processDictionary = new Dictionary<Guid, Thread>();
        private static Dictionary<Guid, Thread> _delayProcessDictionary = new Dictionary<Guid, Thread>();

        /// <summary>
        /// Runs action asynchronously
        /// </summary>
        /// <param name="action">Action</param>
        public virtual Guid RunAsync(Action action)
        {
            var res = Guid.NewGuid();
            var thread = new Thread(() =>
            {
                action.Invoke();
                lock (_processDictionary)
                {
                    _processDictionary.Remove(res);
                }
            });
            
            lock (_processDictionary)
            {
                _processDictionary.Add(res, thread);
                thread.Start();
            }

            return res;
        }

        /// <summary>
        /// Runs action asynchronously
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="threadGuid">Wait previous for complete</param>
        public virtual Guid RunAsync(Action action, Guid? threadGuid)
        {
            var res = Guid.NewGuid();
            var thread = new Thread(() =>
            {
                Thread previousThread;

                do
                {
                    previousThread = null;

                    lock (_processDictionary)
                    {
                        if (threadGuid.HasValue && _processDictionary.ContainsKey(threadGuid.Value))
                        {
                            previousThread = _processDictionary[threadGuid.Value];
                        }
                    }

                    if (previousThread != null)
                    {
                        Thread.Sleep(10);
                    }

                } while (previousThread != null);

                try
                {
                    action.Invoke();
                }
                finally
                {
                    lock (_processDictionary)
                    {
                        _processDictionary.Remove(res);
                    }
                }
                
            });

            lock (_processDictionary)
            {
                _processDictionary.Add(res, thread);
                thread.Start();
            }

            return res;
        }

        /// <summary>
        /// Runs action asynchronously
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="threadGuid">Wait previous for complete</param>
        /// <param name="delay">Delay before execution in miliseconds</param>
        public virtual Guid RunAsyncWithDelay(Action action, Guid? threadGuid, int delay)
        {
            var res = Guid.NewGuid();

            lock (_delayProcessDictionary)
            {
                if (threadGuid.HasValue && _delayProcessDictionary.ContainsKey(threadGuid.Value))
                {
                    try
                    {
                        var process = _delayProcessDictionary[threadGuid.Value];
                        _delayProcessDictionary.Remove(threadGuid.Value);
                        process.Abort();
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            var thread = new Thread(()=>
            {
                Thread.Sleep(delay);
                RunAsync(action);
            });

            lock (_delayProcessDictionary)
            {
                _delayProcessDictionary.Add(res, thread);
                thread.Start();
            }

            return res;
        }

        /// <summary>
        /// Suspends the current thread for the specified number of milliseconds.
        /// </summary>
        /// <param name="millisecondsTimeout">The number of milliseconds for which the thread is suspended. If the value of the millisecondsTimeout argument is zero, the thread relinquishes the remainder of its time slice to any thread of equal priority that is ready to run. If there are no other threads of equal priority that are ready to run, execution of the current thread is not suspended.
        /// </param>
        public virtual void Sleep(int millisecondsTimeout)
        {
            Thread.Sleep(millisecondsTimeout);
        }

        /// <summary>
        /// Stops all previously executed processes
        /// </summary>
        public static void StopAll()
        {
            lock (_processDictionary)
            {
                AbortThreadDictionary(_processDictionary);
            }
            lock (_delayProcessDictionary)
            {
                AbortThreadDictionary(_delayProcessDictionary);
            }
        }

        private static void AbortThreadDictionary(Dictionary<Guid, Thread> threads)
        {
            foreach (var f in threads)
            {
                try
                {
                    f.Value.Abort();
                }
                catch
                {
                    // ignored
                }
            }
            threads.Clear();
        }

        public static void Stop(Guid processGuid)
        {
            lock (_processDictionary)
            {
                if (_processDictionary.ContainsKey(processGuid))
                {
                    var process = _processDictionary[processGuid];
                    _processDictionary.Remove(processGuid);
                    process.Abort();
                }
            }
        }
    }
}
