using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    /// <summary>
    /// Generic callback definition for operations.
    /// </summary>
    public delegate void Callback();

    /// <summary>
    /// Generic callback definition for events.
    /// </summary>
    public delegate void Callback<TResult>(TResult result);

    /// <summary>
    /// Generic callback definition for operations.
    /// </summary>
    public delegate void CompletionCallback(Error error);

    /// <summary>
    /// Generic callback definition for operations.
    /// </summary>
    public delegate void EventCallback<TResult>(TResult result, Error error);

    public class CallbackDispatcher : PrivateSingletonBehaviour<CallbackDispatcher>
    {
        #region Fields

        private     Queue<Action>       m_queue         = new Queue<Action>();

        #endregion

        #region Static methods

        public static CallbackDispatcher Initialize()
        {
            return GetSingleton();
        }

        public static void InvokeOnMainThread(Callback callback)
        {
            // validate arguments
            if (callback == null)
            {
                //DebugLogger.LogWarning("Callback is null.");
                return;
            }

            // add request to queue
            var     manager     = GetSingleton();
            if (manager)
            {
                manager.AddAction(action: () => callback.Invoke());
            }
        }

        public static void InvokeOnMainThread(CompletionCallback callback, Error error)
        {
            // validate arguments
            if (callback == null)
            {
                //DebugLogger.LogWarning("Callback is null.");
                return;
            }

            // add request to queue
            var     manager     = GetSingleton();
            if (manager)
            {
                manager.AddAction(action: () => callback.Invoke(error));
            }
        }

        public static void InvokeOnMainThread<TResult>(Callback<TResult> callback, TResult result)
        {
            // validate arguments
            if (callback == null)
            {
                //DebugLogger.LogWarning("Callback is null.");
                return;
            }

            // add request to queue
            var     manager     = GetSingleton();
            if (manager)
            {
                manager.AddAction(() => callback.Invoke(result));
            }
        }

        public static void InvokeOnMainThread<TResult>(EventCallback<TResult> callback, IOperationResultContainer<TResult> resultContainer)
        {
            InvokeOnMainThread(callback, resultContainer.GetResult(), resultContainer.GetError());
        }

        public static void InvokeOnMainThread<TResult>(EventCallback<TResult> callback, TResult result, Error error)
        {
            // validate arguments
            if (callback == null)
            {
                //DebugLogger.LogWarning("Callback is null.");
                return;
            }

            // add request to queue
            var     manager     = GetSingleton();
            if (manager)
            {
                manager.AddAction(() => callback.Invoke(result, error));
            }
        }

        #endregion

        #region Unity methods

        private void LateUpdate()
        {
            try
            {
                // execute pending actions
                while (m_queue.Count > 0)
                {
                    var     action  = m_queue.Dequeue();
                    action();
                }
            }
            catch (Exception expection)
            {
                DebugLogger.LogException(expection);
            }
        }

        #endregion

        #region Private methods

        private void AddAction(Action action)
        {
            m_queue.Enqueue(action);
        }

        #endregion
    }
}