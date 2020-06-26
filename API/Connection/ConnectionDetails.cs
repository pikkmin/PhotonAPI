using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotonAPI.API.Connection
{
    public class ConnectionDetails
    {
        public string AppID { get; set; }

        public string AppVersion { get; set; }

        public PhotonRegion Region { get; set; }

        public PhotonGame Game { get; set; }

        public string NameServerHost { get; set; }

        public Authorization Authentication { get; set; }

        public ConnectionDetails(string appID, string appVersion, PhotonRegion region = PhotonRegion.None, string nameserverhost = "ns.exitgames.com", Authorization auth = null)
        {
            AppID = appID;
            AppVersion = appVersion;
            Region = region;
            NameServerHost = nameserverhost;
            Authentication = auth;
        }
    }
}
