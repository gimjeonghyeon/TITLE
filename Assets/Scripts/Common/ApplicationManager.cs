using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Playground
{
    public class ApplicationManager : MonoBehaviour
    {
        #region MyRegion

        private const string RESTART_SCENE_NAME = "Title";

        #endregion

        private void Start() => SetApplicationException();

        private void SetApplicationException()
        {
            Application.SetStackTraceLogType(LogType.Exception, StackTraceLogType.Full);

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            Application.logMessageReceived             += OnLogMessageReceived;
        }
        
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception == false)
            {
                return;
            }
                
            var ex = (Exception) e.ExceptionObject;

            // TODO: exception handling
        }
        
        private void OnLogMessageReceived(string condition, string stacktrace, LogType type)
        {
            if (type != LogType.Exception)
            {
                return;
            }
            
            // TODO: exception handling
        }
        
        private void OnApplicationPause(bool isPaused)
        {
            if (isPaused)
            {
                // App is paused
            }
            else
            {
               
            }
        }
        
        /// <summary>
        /// 앱 재시작
        /// </summary>
        public void Restart() => SceneManager.LoadScene(RESTART_SCENE_NAME);
    }
}