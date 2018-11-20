using System;
using System.Reactive.Linq;
using PureActive.Core.Reactive.Observers;

namespace PureActive.Core.Reactive.Extensions
{
    public static class ObservableExtensions
    {
        /// <summary>
        ///     Adds a log that prints to the console the notification emitted by the <paramref name="observable" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observable"></param>
        /// <param name="msg">An optional prefix that will be added before each notification</param>
        /// <returns></returns>
        public static IObservable<T> LogToConsole<T>(this IObservable<T> observable, string msg = "")
        {
            return observable.Do(
                x => Console.WriteLine("{0} - OnNext({1})", msg, x),
                ex =>
                {
                    Console.WriteLine("{0} - OnError:", msg);
                    Console.WriteLine("\t {0}", ex);
                },
                () => Console.WriteLine("{0} - OnCompleted()", msg));
        }

        /// <summary>
        ///     Subscribe an observer that prints each notificatio to the console output
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observable"></param>
        /// <param name="name"></param>
        /// <returns>a disposable subscription object</returns>
        public static IDisposable SubscribeConsole<T>(this IObservable<T> observable, string name = "")
        {
            return observable.Subscribe(new ConsoleObserver<T>(name));
        }
    }
}