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
            Console.Title = $"PhotonAPI Tests = VRChat = Server Time: {Environment.TickCount}ms = {client.CurrentRoom.Players.Count} Player(s)";
            var CurrentRoom = client.CurrentRoom;
            string info = string.Empty;
            info += $"Room: {CurrentRoom.Name}\n";
            info += $"Player Count: {CurrentRoom.PlayerCount}\n";
            info += $"Players in Instance:\n";
            int Index = 0;
            foreach (var Player in CurrentRoom.Players.Values)
            {
                Index++;
                var tag = Player.CustomProperties["modTag"];
                var user = Player.CustomProperties["user"] as Dictionary<string, object>;
                var avatar = Player.CustomProperties["avatarDict"] as Dictionary<string, object>;
                var avtrID = avatar["id"];
                var userID = user["id"];
                var username = user["username"];
                var displayname = user["displayName"];
                var AvatarCopying = user["allowAvatarCopying"];
                string steamID = Player.CustomProperties["steamUserID"] == null ? "0" : Player.CustomProperties["steamUserID"].ToString();
                if (steamID == "0" && bool.Parse(Player.CustomProperties["inVRMode"].ToString()) == false && Player.ActorNumber != client.LocalPlayer.ActorNumber)
                {
                    info += "================================================\n";
                    info += $"[CHEATER] Player #{Index}:\n";
                    info += $"[CHEATER] Player ActorID: {Player.ActorNumber}\n";
                    info += $"[CHEATER] Player UserID: {userID}\n";
                    info += $"[CHEATER] Player Username: {username}\n";
                    info += $"[CHEATER] Player DisplayName: {displayname}\n";
                    info += $"[CHEATER] Player AvatarID: {avtrID}\n";
                    info += $"[CHEATER] Player Is Master: {Player.IsMasterClient}\n";
                    info += $"[CHEATER] Player Is Moderator: {(tag.ToString() == string.Empty ? "False" : "True")}\n";
                    info += $"[CHEATER] Player Has Cloning Enabled: {AvatarCopying}\n";
                    info += $"[CHEATER] Player SteamID: {(Player.CustomProperties["steamUserID"] == null ? "0" : Player.CustomProperties["steamUserID"].ToString())}\n";
                    info += $"[CHEATER] {username} is using a SteamID Spoofer. Cheater Detected.\n";
                    info += "================================================\n";
                }
                else
                {
                    info += "================================================\n";
                    info += $"Player #{Index}:\n";
                    info += $"Player ActorID: {Player.ActorNumber}\n";
                    info += $"Player UserID: {userID}\n";
                    info += $"Player Username: {username}\n";
                    info += $"Player DisplayName: {displayname}\n";
                    info += $"Player AvatarID: {avtrID}\n";
                    info += $"Player Is Master: {Player.IsMasterClient}\n";
                    info += $"Player Is Moderator: {(tag.ToString() == string.Empty ? "False" : "True")}\n";
                    info += $"Player Has Cloning Enabled: {AvatarCopying}\n";
                    info += $"Player SteamID: {(Player.CustomProperties["steamUserID"] == null ? "0" : Player.CustomProperties["steamUserID"].ToString())}\n";
                    info += "================================================\n";
                }
            }
            Console.WriteLine(info);
        }

        private void VRChatClient_ConnectedToMaster(PhotonClient client)
        {
            this.SetCustomProperties(Details.Authentication.AuthParameters.ElementAt(0).Value, Details.Authentication.AuthParameters.ElementAt(1).Value, "avtr_23c53796-fb1c-4003-8b7a-90c2349bf043"); //example avatar id ok
            this.JoinRoom("wrld_7e10376a-29b6-43af-ac5d-6eb72732e90c:785"); //idk lets find out
        }
    }
}
