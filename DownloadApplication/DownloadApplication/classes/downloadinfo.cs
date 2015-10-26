using System.Net;
using System;

namespace MyDownloadApplication
{
    public class Downloadinfo
    {
        public Downloadinfo()
        {
            ProgressArgs = new DownloadInfoChangedEventArgs();
        }
        public Downloadinfo(DownloadProgressChangedEventArgs dp)
        {
            ProgressArgs = new DownloadInfoChangedEventArgs()
            {
                BytesReceived = dp.BytesReceived,
                TotalBytesToReceive = dp.TotalBytesToReceive,
                UserState = dp.UserState
            };
        }
        public DownloadInfoChangedEventArgs ProgressArgs { get; set; }
        public int ProgressPercentage { get { return ProgressArgs != null ? ProgressArgs.ProgressPercentage : 0; } }
        public long Speed { get; set; }
    }
    public class DownloadInfoChangedEventArgs:EventArgs
    {
        public int ProgressPercentage { get { return BytesReceived>0&&TotalBytesToReceive>0? int.Parse(Math.Round(((double.Parse(BytesReceived.ToString()) / double.Parse(TotalBytesToReceive.ToString())) * 100), 0).ToString()):0; } }
        public object UserState { get; set; }
        public long BytesReceived { get; set; }
        public long TotalBytesToReceive { get; set; }
    }
}
