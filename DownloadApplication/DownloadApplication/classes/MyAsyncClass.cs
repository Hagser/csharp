using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;
using System.ComponentModel;
using System.Threading;

namespace MyDownloadApplication
{
    public class MyAsyncClass
    {
        private MegaVideoLink mvl = new MegaVideoLink();
        private delegate void MyTaskWorkerDelegate(string strurl, bool downloadrmtp, string strTitle);
        private void MyTaskCompletedCallback(IAsyncResult ar)
        {
            // get the original worker delegate and the AsyncOperation instance
            MyTaskWorkerDelegate worker =
              (MyTaskWorkerDelegate)((AsyncResult)ar).AsyncDelegate;
            AsyncOperation async = (AsyncOperation)ar.AsyncState;

            // finish the asynchronous operation
            worker.EndInvoke(ar);

            // clear the running task flag
            lock (async)
            {
                _myTaskIsRunning = false;
            }

            // raise the completed event
            AsyncCompletedEventArgs completedArgs = new AsyncCompletedEventArgs(null,
              false,mvl);
            async.PostOperationCompleted(
              delegate(object e) { OnMyTaskCompleted((AsyncCompletedEventArgs)e); },
              completedArgs);
        }
        public event AsyncCompletedEventHandler MyTaskCompleted;

        protected virtual void OnMyTaskCompleted(AsyncCompletedEventArgs e)
        {
            if (MyTaskCompleted != null)
                MyTaskCompleted(this, e);
        }
        public void MyTaskWorker(string strurl, bool downloadrmtp, string strTitle)
        {
            Thread.Sleep(10000);
        }
        private bool _myTaskIsRunning = false;

        public bool IsBusy
        {
            get { return _myTaskIsRunning; }
        }
    }
}
