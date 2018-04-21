using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace PushSharp.Core
{
    public enum LogLevel
    {
        Info,
        Debug,
        Error
    }

    public interface ILogger
    {
        void Write(LogLevel level, string message, params object[] args);
    }

    public static class Log
    {
        static readonly object loggerLock = new object();

        //static List<ILogger> loggers { get; set; }
        static List<ILogger> loggers = new List<ILogger>() { new ConsoleLogger() };
        static Dictionary<CounterToken, Stopwatch> counters = new Dictionary<CounterToken, Stopwatch>();

        //static Log()
        //{
        //    //counters = new Dictionary<CounterToken, Stopwatch>();
        //    //loggers = new List<ILogger>();

        //    //AddLogger(new ConsoleLogger());
        //}

        public static void AddLogger(ILogger logger)
        {
            lock (loggerLock)
                loggers.Add(logger);
        }

        public static void ClearLoggers()
        {
            lock (loggerLock)
                loggers.Clear();
        }

        public static IEnumerable<ILogger> Loggers
        {
            get { return loggers; }
        }

        public static void Write(LogLevel level, [Localizable(false)] string message, params object[] args)
        {
            lock (loggers)
            {
                foreach (var l in loggers)
                    l.Write(level, message, args);
            }
        }

        public static void Info([Localizable(false)] string message, params object[] args)
        {
            Write(LogLevel.Info, message, args);
        }

        public static void Debug(string message, params object[] args)
        {
            Write(LogLevel.Debug, message, args);
        }

        public static void Error(string message, params object[] args)
        {
            Write(LogLevel.Error, message, args);
        }

        public static CounterToken StartCounter()
        {
            var t = new CounterToken
            {
                Id = Guid.NewGuid().ToString()
            };

            var sw = new Stopwatch();

            counters.Add(t, sw);

            sw.Start();

            return t;
        }

        public static TimeSpan StopCounter(CounterToken counterToken)
        {
            if (!counters.ContainsKey(counterToken))
                return TimeSpan.Zero;

            var sw = counters[counterToken];

            sw.Stop();

            counters.Remove(counterToken);

            return sw.Elapsed;
        }

        public static void StopCounterAndLog(CounterToken counterToken, string message)
        {
            Log.StopCounterAndLog(counterToken, message, LogLevel.Info);
        }

        public static void StopCounterAndLog(CounterToken counterToken, string message, LogLevel level)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            var elapsed = StopCounter(counterToken);

            if (!message.Contains("{0}"))
                message += " {0}";

            Log.Write(level, message, elapsed.TotalMilliseconds);
        }
    }

    public static class CounterExtensions
    {
        public static void StopAndLog(this CounterToken counterToken, string message)
        {
            Log.StopCounterAndLog(counterToken, message, LogLevel.Info);
        }

        public static void StopAndLog(this CounterToken counterToken, string message, LogLevel level)
        {
            Log.StopCounterAndLog(counterToken, message, level);
        }

        public static TimeSpan Stop(this CounterToken counterToken)
        {
            return Log.StopCounter(counterToken);
        }
    }

    public class CounterToken
    {
        public string Id { get; set; }
    }

    public class ConsoleLogger : ILogger
    {
        public void Write(LogLevel level, string message, params object[] args)
        {
            var s = message;

            if (args != null && args.Length > 0)
                s = string.Format(CultureInfo.CurrentCulture, message, args);

            var d = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ttt", CultureInfo.CurrentCulture);

            switch (level)
            {
                case LogLevel.Info:
                    Console.Out.WriteLine(d + " [INFO] " + s);
                    break;
                case LogLevel.Debug:
                    Console.Out.WriteLine(d + " [DEBUG] " + s);
                    break;
                case LogLevel.Error:
                    Console.Error.WriteLine(d + " [ERROR] " + s);
                    break;
            }
        }
    }
}

