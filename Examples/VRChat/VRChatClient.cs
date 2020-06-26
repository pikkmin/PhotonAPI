using PhotonAPI;
using PhotonAPI.API.Connection;
using PhotonAPI.API.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITests
{
    public class VRChatClient : PhotonClient
    {
        public VRChatClient(ConnectionDetails details) : base(details) { }

        public override void Init()
        {
            this.RegisterTypes();
            this.SetAuthentication(Details.Authentication.AuthParameters.ElementAt(0).Value, Details.Authentication.AuthParameters.ElementAt(1).Value); //lazy ok
            this.ConnectedToMaster += VRChatClient_ConnectedToMaster;
            this.JoinedRoom += VRChatClient_JoinedRoom;
        }

        private void VRChatClient_JoinedRoom(PhotonClient client)
        {
            client.Instantiate(new Vector3(0f, 0.8f, 0f), new Quaternion(360f, 0.8f, 0f, 0f));
            Console.WriteLine("HI I JOINED!");
            Console.WriteLine(client.CurrentRoom.Players.Count);
        }

        private void VRChatClient_ConnectedToMaster(PhotonClient client)
        {
            this.SetCustomProperties(Details.Authentication.AuthParameters.ElementAt(0).Value, Details.Authentication.AuthParameters.ElementAt(1).Value, "avtr_23c53796-fb1c-4003-8b7a-90c2349bf043"); //example avatar id ok
            this.JoinRoom("wrld_7e10376a-29b6-43af-ac5d-6eb72732e90c:785"); //idk lets find out
        }
    }
}
