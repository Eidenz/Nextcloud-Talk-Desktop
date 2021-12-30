using CefSharp;
using System;

namespace Talk
{
    class MyCustomDownloadHandler : IDownloadHandler
    {
        public event EventHandler<DownloadItem> OnBeforeDownloadFired;

        public event EventHandler<DownloadItem> OnDownloadUpdatedFired;

        public void OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            OnBeforeDownloadFired?.Invoke(this, downloadItem);

            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    callback.Continue(
                        downloadItem.SuggestedFileName,
                        showDialog: true
                    );
                }
            }
        }

        /// https://cefsharp.github.io/api/51.0.0/html/T_CefSharp_DownloadItem.htm
        public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            OnDownloadUpdatedFired?.Invoke(this, downloadItem);
        }
    }
}
