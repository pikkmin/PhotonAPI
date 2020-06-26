using PhotonAPI;
using PhotonAPI.API;
using PhotonAPI.API.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Photon API Test = VRChat";
            Authorization VRChatAuthorization = new Authorization();
            var AuthCookie = "authcookie here";
            VRChatAuthorization.AddAuthParameter("token", AuthCookie);
            VRChatAuthorization.AddAuthParameter("user", ClientExtensions.GetUserID(AuthCookie));
            ConnectionDetails VRChatConfiguration = new ConnectionDetails("bf0942f7-9935-4192-b359-f092fa85bef1", "Release_2018_server_944_2.5", PhotonRegion.USW, "ns.exitgames.com", VRChatAuthorization);
            VRChatClient client = new VRChatClient(VRChatConfiguration);
            client.ConnectToRegionMaster("USW");
            Console.ReadLine();
        }
    }
}
