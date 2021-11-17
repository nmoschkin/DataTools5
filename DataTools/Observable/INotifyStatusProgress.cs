
// ' The INotifyStatusProgress framework.
// ' Copyright (C) 2015 Nathan Moschkin
// ' All Rights Reserved.


using System;

namespace DataTools.PlugInFramework
{

    public delegate void ProgressEventHandler(object sender, StatusProgressEventArgs e);

    public delegate void FinishedEventHandler(object sender, StatusProgressEventArgs e);

    public delegate void StartingEventHandler(object sender, StatusProgressEventArgs e);

    /// <summary>
    /// Status display characteristics.
    /// </summary>
    [Flags]
    public enum StatusModes
    {
        StaticDisplay = 1,
        ProgressBar = 2,
        ProgressStatic = 4,
        Marquis = 8
    }

    /// <summary>
    /// Work progress status codes.
    /// </summary>
    public enum StatusCodes
    {
        Aborted = -1,
        Stopped = 0,
        Starting = 1,
        Running = 2,
        Paused = 3
    }

    /// <summary>
    /// Provides an common interface for classes that do long-running out-of-process work.
    /// </summary>
    public interface INotifyStatusProgress
    {

        /// <summary>
        /// Raised while the work is progressing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        event ProgressEventHandler Progress;

        /// <summary>
        /// Raised when work is completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        event FinishedEventHandler Finished;

        /// <summary>
        /// Raised when work is starting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        event StartingEventHandler Starting;

        /// <summary>
        /// Call this while the work is progressing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnProgress(object sender, StatusProgressEventArgs e);

        /// <summary>
        /// Call this when the work is complete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnFinished(object sender, StatusProgressEventArgs e);

        /// <summary>
        /// Call this when the work is starting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnStarting(object sender, StatusProgressEventArgs e);

        /// <summary>
        /// Call this to invoke a method on another thread or the Dispatcher thread.
        /// The implementing class must provide for the Dispatcher or other means of invoking a method.
        /// </summary>
        /// <param name="func">The delegate function to invoke.</param>
        void Invoke(Delegate func);


        /// <summary>
        /// Call this to begin the process of invoking a method on another thread or the Dispatcher thread.
        /// The implementing class must provide for the Dispatcher or other means of invoking a method.
        /// </summary>
        /// <param name="func">The delegate function to invoke.</param>
        void BeginInvoke(Delegate func);
    }

    /// <summary>
    /// Arguments to be used when calling events implemented in <see cref="INotifyStatusProgress" />
    /// </summary>
    public class StatusProgressEventArgs : EventArgs
    {
        protected int _pos;
        protected int _count;
        protected string _msg;
        protected object _data;
        protected int _msgcode;
        protected string _detail;
        protected StatusModes _mode;
        protected StatusCodes _scode = StatusCodes.Stopped;

        /// <summary>
        /// Gets or sets a value indicating a desire to abort the current process.
        /// </summary>
        /// <returns></returns>
        public bool Abort { get; set; }

        /// <summary>
        /// Gets the current status display characteristics.
        /// </summary>
        /// <returns></returns>
        public StatusModes Mode
        {
            get
            {
                return _mode;
            }

            set
            {
                _mode = value;
            }
        }

        /// <summary>
        /// Gets the current status code.
        /// </summary>
        /// <returns></returns>
        public StatusCodes StatusCode
        {
            get
            {
                return _scode;
            }

            set
            {
                _scode = value;
            }
        }

        /// <summary>
        /// Gets the numeric message code.
        /// </summary>
        /// <returns></returns>
        public int MessageCode
        {
            get
            {
                return _msgcode;
            }

            set
            {
                _msgcode = value;
            }
        }

        /// <summary>
        /// Gets the current position.
        /// </summary>
        /// <returns></returns>
        public int Position
        {
            get
            {
                return _pos;
            }

            set
            {
                _pos = value;
            }
        }

        /// <summary>
        /// Gets the total number of elements.
        /// </summary>
        /// <returns></returns>
        public int Count
        {
            get
            {
                return _count;
            }

            set
            {
                _count = value;
            }
        }

        /// <summary>
        /// Gets the status message text.
        /// </summary>
        /// <returns></returns>
        public string Message
        {
            get
            {
                return _msg;
            }

            set
            {
                _msg = value;
            }
        }

        /// <summary>
        /// Gets the status detail text.
        /// </summary>
        /// <returns></returns>
        public string Detail
        {
            get
            {
                return _detail;
            }

            set
            {
                _detail = value;
            }
        }

        /// <summary>
        /// Gets or sets miscellaneous data.
        /// </summary>
        /// <returns></returns>
        public object Data
        {
            get
            {
                return _data;
            }

            set
            {
                _data = value;
            }
        }

        public StatusProgressEventArgs()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode">A StatusModes value indicating the desired display characteristics.</param>
        /// <param name="msg">The textual status message.</param>
        /// <param name="count">The total number of elements.</param>
        /// <param name="detail">Optional detailed textual status message.</param>
        /// <param name="code">Optional numeric message code.</param>
        /// <param name="data">Optional miscellaneous data.</param>
        public StatusProgressEventArgs(StatusModes mode, string msg, int count, string detail = null, int code = 0, object data = null)
        {
            _scode = StatusCodes.Starting;
            _mode = mode;
            _msg = msg;
            _pos = 0;
            _count = count;
            _data = data;
            _msgcode = code;
            _detail = detail;
        }

        /// <summary>
        /// Creates a new StatusProgressEventArgs object with the specified parameters.
        /// </summary>
        /// <param name="status">A StatusCodes value indicating the type of status update.</param>
        /// <param name="msg">The textual status message.</param>
        /// <param name="pos">The numeric position.</param>
        /// <param name="count">The total number of elements.</param>
        /// <param name="detail">Optional detailed textual status message.</param>
        /// <param name="code">Optional numeric message code.</param>
        /// <param name="data">Optional miscellaneous data.</param>
        /// <param name="mode">Optional StatusModes value indicating the desired display characteristics.</param>
        public StatusProgressEventArgs(StatusCodes status, string msg, int pos, int count, string detail = null, int code = 0, object data = null, StatusModes mode = StatusModes.ProgressBar)
        {
            _scode = status;
            _mode = mode;
            _msg = msg;
            _pos = pos;
            _count = count;
            _data = data;
            _msgcode = code;
            _detail = detail;
        }
    }
}