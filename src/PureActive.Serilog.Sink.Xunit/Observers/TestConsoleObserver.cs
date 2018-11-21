// ***********************************************************************
// Assembly         : PureActive.Serilog.Sink.Xunit
// Author           : SteveBu
// Created          : 11-21-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-21-2018
// ***********************************************************************
// <copyright file="TestConsoleObserver.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary>
//      Reactive Observer that uses the ITestOutputHelper to write to the 
//      xUnit console.
// </summary>
// ***********************************************************************
using System;
using Xunit.Abstractions;

namespace PureActive.Serilog.Sink.Xunit.Observers
{
    /// <summary>
    /// Class TestConsoleObserver.
    /// Implements the <see cref="System.IObserver{T}" /> to write to xUnit console
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.IObserver{T}" />
    public class TestConsoleObserver<T> : IObserver<T>
    {
        /// <summary>
        /// The optional name of the observable
        /// </summary>
        private readonly string _name;
        /// <summary>
        /// xUnit TestOutputHelper interface
        /// </summary>
        private readonly ITestOutputHelper _testOutputHelper;



        /// <summary>
        /// Initializes a new instance of the <see cref="TestConsoleObserver{T}"/> class.
        /// </summary>
        /// <param name="testOutputHelper">xUnit test output helper.</param>
        /// <param name="name">optional name of the observable.</param>
        /// <exception cref="System.ArgumentNullException">testOutputHelper</exception>
        public TestConsoleObserver(ITestOutputHelper testOutputHelper, string name = "")
        {
            _testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));
            _name = name;
        }

        /// <summary>
        /// Provides the observer with new data.
        /// </summary>
        /// <param name="value">The current notification information.</param>
        public void OnNext(T value)
        {
            _testOutputHelper.WriteLine(TestLoggerObserver<T>.OnNextMsgTemplate, _name, value);
        }

        /// <summary>
        /// Notifies the observer that the provider has experienced an error condition.
        /// </summary>
        /// <param name="error">An object that provides additional information about the error.</param>
        public void OnError(Exception error)
        {
            _testOutputHelper.WriteLine(TestLoggerObserver<T>.OnErrorMsgTemplate, _name);
            _testOutputHelper.WriteLine("\t {0}", error);
        }

        /// <summary>
        /// Notifies the observer that the provider has finished sending push-based notifications.
        /// </summary>
        public void OnCompleted()
        {
            _testOutputHelper.WriteLine(TestLoggerObserver<T>.OnCompletedMsgTemplate, _name);
        }
    }
}