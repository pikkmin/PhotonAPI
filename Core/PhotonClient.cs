using ExitGames.Client.Photon;
using Photon.Realtime;
using PhotonAPI.API;
using PhotonAPI.API.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhotonAPI
{
    public class PhotonClient : LoadBalancingClient, IConnectionCallbacks, ILobbyCallbacks, IInRoomCallbacks, IMatchmakingCallbacks
    {
        #region Events
        public delegate void OnReady(PhotonClient client);

        public event OnReady Ready;

        public delegate void WhenConnected(PhotonClient client);

        public event WhenConnected Connected;

        public delegate void WhenConnectedToMaster(PhotonClient client);

        public event WhenConnectedToMaster ConnectedToMaster;

        public delegate void WhenDisconnected(PhotonClient client, DisconnectCause Cause);

        public event WhenDisconnected Disconnected;

        public delegate void WhenCreatedRoom(PhotonClient client);

        public event WhenCreatedRoom CreatedRoom;

        public delegate void WhenCreateRoomFailed(PhotonClient client, short returnCode, string message);

        public event WhenCreateRoomFailed CreateRoomFailed;

        public delegate void WhenJoinRoomFailed(PhotonClient client, short returnCode, string message);

        public event WhenJoinRoomFailed JoinRoomFailed;

        public delegate void WhenJoinRandomRoomFailed(PhotonClient client, short returnCode, string message);

        public event WhenJoinRandomRoomFailed JoinRandomRoomFailed;

        public delegate void WhenJoinedRoom(PhotonClient client);

        public event WhenJoinedRoom JoinedRoom;

        public delegate void WhenJoinedLobby(PhotonClient client);

        public event WhenJoinedLobby JoinedLobby;

        public delegate void WhenLeftRoom(PhotonClient client);

        public event WhenLeftRoom LeftRoom;

        public delegate void WhenLeftLobby(PhotonClient client);

        public event WhenLeftLobby LeftLobby;

        public delegate void WhenMasterClientSwitched(PhotonClient client, Player NewMaster);

        public event WhenMasterClientSwitched MasterClientSwitched;

        public delegate void WhenPlayerEnteredRoom(PhotonClient client, Player NewPlayer);

        public event WhenPlayerEnteredRoom PlayerEnteredRoom;

        public delegate void WhenPlayerLeftRoom(PhotonClient client, Player OldPlayer);

        public event WhenPlayerLeftRoom PlayerLeftRoom;

        public delegate void WhenPlayerPropertiesUpdate(PhotonClient client, Player Player, Hashtable ChangedProperties);

        public event WhenPlayerPropertiesUpdate PlayerPropertiesUpdate;

        public delegate void WhenRoomPropertiesUpdate(PhotonClient client, Hashtable ChangedProperties);

        public event WhenRoomPropertiesUpdate RoomPropertiesUpdate;
        #endregion

        public ConnectionDetails Details { get; set; }

        public PhotonClient(ConnectionDetails details) : base(ConnectionProtocol.Udp)
        {
            Details = details;
            AppId = Details.AppID;
            NameServerHost = Details.NameServerHost;
            AppVersion = Details.AppVersion;
            AddCallbackTarget(this);
            new Thread(() => this.UpdateCallbacks()) { IsBackground = true }.Start();
            Init();
        }

        public virtual void Init()
        {

        }

        private void UpdateCallbacks()
        {
            while (true)
            {
                this.Service();
                Thread.Sleep(25);
            }
        }

        public void LeaveRoom() => OpLeaveRoom(false);

        public void JoinRoom(EnterRoomParams parameters) => OpJoinRoom(parameters);

        public void OnConnected() { if (Connected != null) Connected.Invoke(this); }

        public void OnConnectedToMaster() { if (ConnectedToMaster != null) ConnectedToMaster.Invoke(this); }

        public void OnCreatedRoom() { if (CreatedRoom != null) CreatedRoom.Invoke(this); } 

        public void OnCreateRoomFailed(short returnCode, string message) { if (CreateRoomFailed != null) CreateRoomFailed.Invoke(this, returnCode, message); }

        public void OnCustomAuthenticationFailed(string debugMessage) { }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data) { }

        public void OnDisconnected(DisconnectCause cause) { if (Disconnected != null) Disconnected.Invoke(this, cause); }

        public void OnFriendListUpdate(List<FriendInfo> friendList) { }

        public void OnJoinedLobby() { if (JoinedLobby != null) JoinedLobby.Invoke(this); }

        public void OnJoinedRoom() { if (JoinedRoom != null) JoinedRoom.Invoke(this); }

        public void OnJoinRandomFailed(short returnCode, string message) { if (JoinRandomRoomFailed != null) JoinRandomRoomFailed.Invoke(this, returnCode, message); }

        public void OnJoinRoomFailed(short returnCode, string message) { if (JoinRoomFailed != null) JoinRoomFailed.Invoke(this, returnCode, message); }

        public void OnLeftLobby() { if (LeftLobby != null) LeftLobby.Invoke(this); }

        public void OnLeftRoom() { if (LeftRoom != null) LeftRoom.Invoke(this); }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics) { }

        public void OnMasterClientSwitched(Player newMasterClient) { if (MasterClientSwitched != null) MasterClientSwitched.Invoke(this, newMasterClient); }

        public void OnPlayerEnteredRoom(Player newPlayer) { if (PlayerEnteredRoom != null) PlayerEnteredRoom.Invoke(this, newPlayer); }

        public void OnPlayerLeftRoom(Player otherPlayer) { if (PlayerLeftRoom != null) PlayerLeftRoom.Invoke(this, otherPlayer); }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) { if (PlayerPropertiesUpdate != null) PlayerPropertiesUpdate.Invoke(this, targetPlayer, changedProps); }

        public void OnRegionListReceived(RegionHandler regionHandler) { }

        public void OnRoomListUpdate(List<RoomInfo> roomList) { }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) { if (RoomPropertiesUpdate != null) RoomPropertiesUpdate.Invoke(this, propertiesThatChanged); }
    }
}
