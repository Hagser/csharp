using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;

namespace PFirewallReader
{
    public class Diag
    {
        /// <summary>
        /// Performs a pathping
        /// </summary>
        /// <param name="ipaTarget">The target</param>
        /// <param name="iHopcount">The maximum hopcount</param>
        /// <param name="iTimeout">The timeout for each ping</param>
        /// <returns>An array of PingReplys for the whole path</returns>
        public static PingReply[] PerformPathping(IPAddress ipaTarget, int iHopcount, int iTimeout)
        {
            System.Collections.ArrayList arlPingReply = new System.Collections.ArrayList();
            Ping myPing = new Ping();
            PingReply prResult = null;
            int iTimeOutCnt = 0;
            for (int iC1 = 1; iC1 < iHopcount && iTimeOutCnt<5; iC1++)
            {
                prResult = myPing.Send(ipaTarget, iTimeout, new byte[10], new PingOptions(iC1, false));
                if (prResult.Status == IPStatus.Success)
                {
                    iC1 = iHopcount;
                    iTimeOutCnt = 0;
                }
                else if (prResult.Status == IPStatus.TtlExpired)
                {
                    iTimeOutCnt = 0;
                }
                else if (prResult.Status == IPStatus.TimedOut)
                {
                    iTimeOutCnt++;
                }
                arlPingReply.Add(prResult);
            }
            PingReply[] prReturnValue = new PingReply[arlPingReply.Count];
            for (int iC1 = 0; iC1 < arlPingReply.Count; iC1++)
            {
                prReturnValue[iC1] = (PingReply)arlPingReply[iC1];
            }
            return prReturnValue;
        }
    }
}
