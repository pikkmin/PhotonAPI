using ExitGames.Client.Photon;
using Newtonsoft.Json;
using Photon.Realtime;
using PhotonAPI;
using PhotonAPI.API.Position;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace APITests
{
    public enum RpcTarget
    {
        All,
        Others,
        MasterClient,
        AllBuffered,
        OthersBuffered,
        AllViaServer,
        AllBufferedViaServer
    }

    public class AvatarInformation
    {

        public string name { get; set; }
        public string description { get; set; }
        public string authorId { get; set; }
        public string authorName { get; set; }
        public string imageUrl { get; set; }
        public string thumbnailImageUrl { get; set; }
        public string assetUrl { get; set; }
        public object[] tags { get; set; }
        public string releaseStatus { get; set; }
        public int version { get; set; }
        public string unityPackageUrl { get; set; }
        public object unityPackageUrlObject { get; set; }
        public string unityVersion { get; set; }
        public int assetVersion { get; set; }
        public string platform { get; set; }
        public bool featured { get; set; }
        public bool imported { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string id { get; set; }
        public Unitypackage[] unityPackages { get; set; }

        public class Unitypackage
        {
            public string id { get; set; }
            public string assetUrl { get; set; }
            public object assetUrlObject { get; set; }
            public string unityVersion { get; set; }
            public long unitySortNumber { get; set; }
            public int assetVersion { get; set; }
            public string platform { get; set; }
            public DateTime created_at { get; set; }
        }

    }

    public class LoginResponse
    {
        public string id { get; set; }
        public string username { get; set; }
        public string displayName { get; set; }
        public string bio { get; set; }
        public object[] bioLinks { get; set; }
        public object[] pastDisplayNames { get; set; }
        public bool hasEmail { get; set; }
        public bool hasPendingEmail { get; set; }
        public string email { get; set; }
        public string obfuscatedEmail { get; set; }
        public string obfuscatedPendingEmail { get; set; }
        public bool emailVerified { get; set; }
        public bool hasBirthday { get; set; }
        public bool unsubscribe { get; set; }
        public object[] friends { get; set; }
        public object[] friendGroupNames { get; set; }
        public string currentAvatarImageUrl { get; set; }
        public string currentAvatarThumbnailImageUrl { get; set; }
        public string currentAvatar { get; set; }
        public string currentAvatarAssetUrl { get; set; }
        public object accountDeletionDate { get; set; }
        public int acceptedTOSVersion { get; set; }
        public string steamId { get; set; }
        public object steamDetails { get; set; }
        public string oculusId { get; set; }
        public bool hasLoggedInFromClient { get; set; }
        public string homeLocation { get; set; }
        public bool twoFactorAuthEnabled { get; set; }
        public object feature { get; set; }
        public string status { get; set; }
        public string statusDescription { get; set; }
        public string state { get; set; }
        public object[] tags { get; set; }
        public string developerType { get; set; }
        public DateTime last_login { get; set; }
        public string last_platform { get; set; }
        public bool allowAvatarCopying { get; set; }
        public bool isFriend { get; set; }
        public string friendKey { get; set; }
        public object[] onlineFriends { get; set; }
        public object[] activeFriends { get; set; }
        public object[] offlineFriends { get; set; }
    }

    public class WorldInformation
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool featured { get; set; }
        public string authorId { get; set; }
        public string authorName { get; set; }
        public int capacity { get; set; }
        public string[] tags { get; set; }
        public string releaseStatus { get; set; }
        public string imageUrl { get; set; }
        public string thumbnailImageUrl { get; set; }
        public string assetUrl { get; set; }
        public string pluginUrl { get; set; }
        public string unityPackageUrl { get; set; }
        public string _namespace { get; set; }
        public int version { get; set; }
        public string organization { get; set; }
        public object previewYoutubeId { get; set; }
        public int favorites { get; set; }
        public int visits { get; set; }
        public int popularity { get; set; }
        public int heat { get; set; }
        public int publicOccupants { get; set; }
        public int privateOccupants { get; set; }
        public int occupants { get; set; }
        public object[][] instances { get; set; }
    }

    public static class ClientExtensions
    {
        internal static List<Type> ILHFIJHBPKK = new List<Type>
        {
            typeof(byte),
            typeof(bool),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(string)
        };

        public static Dictionary<string, int> CachedRoomCapacity = new Dictionary<string, int>();

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string GetUserID(string AuthToken)
        {
            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                Uri target = new Uri("https://www.vrchat.com");
                handler.CookieContainer.Add(new Cookie("auth", AuthToken) { Domain = target.Host });
                HttpClient http = new HttpClient(handler);

                var response = http.GetAsync("https://www.vrchat.net/api/1/auth/user?apiKey=JlE5Jldo5Jibnk5O5hTx6XVqsJu4WJ26");
                if (response.Result.StatusCode == HttpStatusCode.OK)
                {
                    var json = JsonConvert.DeserializeObject<LoginResponse>(response.Result.Content.ReadAsStringAsync().Result);
                    return json.id;
                }
            }
            catch (Exception) { }

            return null;
        }
        public static int GetCapacityOfWorld(string roomID)
        {
            string worldID = roomID.Split(':')[0];
            if (CachedRoomCapacity.TryGetValue(worldID, out int capacity)) return capacity;
            HttpClient _client = new HttpClient();
            var response = _client.GetAsync($"https://vrchat.com/api/1/worlds/{worldID}?apiKey=JlE5Jldo5Jibnk5O5hTx6XVqsJu4WJ26");
            if (response.Result.StatusCode == HttpStatusCode.OK) CachedRoomCapacity.Add(worldID, JsonConvert.DeserializeObject<WorldInformation>(response.Result.Content.ReadAsStringAsync().Result).capacity); return JsonConvert.DeserializeObject<WorldInformation>(response.Result.Content.ReadAsStringAsync().Result).capacity;
        }
        public static void JoinRoom(this PhotonClient client, string RoomID)
        {
            EnterRoomParams parms = new EnterRoomParams()
            {
                RoomName = RoomID,
                CreateIfNotExists = true,
            };
            RoomOptions options = new RoomOptions()
            {
                IsOpen = true,
                IsVisible = true,
                MaxPlayers = Convert.ToByte(GetCapacityOfWorld(RoomID) * 2)
            };
            Hashtable table = new Hashtable()
            {
                ["name"] = "name"
            };
            options.CustomRoomProperties = table;
            parms.RoomOptions = options;
            string[] customroompropertiesforlobby = new string[]
            {
                "name"
            };
            options.CustomRoomPropertiesForLobby = customroompropertiesforlobby;
            options.EmptyRoomTtl = 0;
            options.DeleteNullProperties = true;
            options.PublishUserId = false;
            client.OpJoinRoom(parms);
        }
        public static void Instantiate(this PhotonClient client, Vector3 Position, Quaternion Rotation)
        {
            Hashtable hashtable = new Hashtable();
            hashtable[(byte)0] = "VRCPlayer";
            hashtable[(byte)1] = Position;
            hashtable[(byte)2] = Rotation;
            hashtable[(byte)4] = new int[] { int.Parse(client.LocalPlayer.ActorNumber + "00001"), int.Parse(client.LocalPlayer.ActorNumber + "00002") };
            hashtable[(byte)6] = Environment.TickCount;
            hashtable[(byte)7] = int.Parse(client.LocalPlayer.ActorNumber + "00001");

            client.OpRaiseEvent(202, hashtable, new RaiseEventOptions
            {
                CachingOption = EventCaching.AddToRoomCache
            }, new SendOptions()
            {
                DeliveryMode = DeliveryMode.Reliable,
                Reliability = true,
                Channel = 1
            });
        }
        public static FileInfo GetNewestFile(DirectoryInfo directory)
        {
            return directory.EnumerateFiles("*.*", SearchOption.AllDirectories).OrderByDescending(f => f.LastWriteTime).Where(x => x.Extension.Contains("txt")).First();
        }
        public static Player GetPlayer(this PhotonClient client, string userid)
        {
            foreach (var player in client.CurrentRoom.Players.Values)
            {
                var user = player.CustomProperties["user"] as Dictionary<string, object>;
                if (user["id"].ToString() == userid) return player;
            }

            return null;
        }
        public static string GetCurrentRoom(this PhotonClient client)
        {
            var di = new DirectoryInfo($"C:\\Users\\{Environment.UserName}\\AppData\\LocalLow\\VRChat\\VRChat");
            var file = GetNewestFile(di);

            using (var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    string contents = reader.ReadToEnd();

                    string toBeSearched = "[RoomManager] Joining w";
                    int ix = contents.LastIndexOf(toBeSearched);

                    if (ix != -1)
                    {
                        string roomID = contents.Substring(ix + toBeSearched.Length).Split('\n')[0];

                        return "w" + roomID.ToString();
                    }
                }
            }

            return null;
        }
        public static void JoinCurrentRoom(this PhotonClient client)
        {
            var currentRoom = client.GetCurrentRoom();
            client.JoinRoom(currentRoom.ToString());
        }
        public static Hashtable Instantiate(this PhotonClient client, string prefabName, Vector3 position, Quaternion rotation, int[] viewIDs)
        {
            int num = viewIDs[0];
            Hashtable hashtable = new Hashtable();
            hashtable[(byte)0] = prefabName;
            hashtable[(byte)1] = position;
            hashtable[(byte)2] = rotation;
            hashtable[(byte)4] = viewIDs;
            hashtable[(byte)6] = Environment.TickCount;
            hashtable[(byte)7] = num;

            client.OpRaiseEvent(202, hashtable, new RaiseEventOptions
            {
                CachingOption = EventCaching.AddToRoomCache
            }, new SendOptions()
            {
                DeliveryMode = DeliveryMode.Reliable,
                Reliability = true,
                Channel = 1
            });

            return hashtable;
        }
        public static object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();

            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);

            object obj = (object)binForm.Deserialize(memStream);

            return obj;
        }
        public static void RPC(this PhotonClient client, int[] viewIDs, string MethodName, RpcTarget target, bool encrypt, params object[] parameters)
        {
            int number = viewIDs[0];
            Hashtable hashtable = new Hashtable();
            hashtable[(byte)0] = viewIDs;
            hashtable[(byte)1] = number;
            hashtable[(byte)2] = Environment.TickCount;
            hashtable[(byte)3] = MethodName;
            if (parameters.Count() > 0 || parameters != null) hashtable[(byte)4] = parameters;

            switch (target)
            {
                case RpcTarget.All:
                    client.OpRaiseEvent(200, hashtable, new RaiseEventOptions()
                    {
                        Receivers = ReceiverGroup.All
                    }, new SendOptions()
                    {
                        Reliability = true,
                        Encrypt = encrypt
                    });
                    return;
                case RpcTarget.Others:
                    client.OpRaiseEvent(200, hashtable, new RaiseEventOptions()
                    {
                        Receivers = ReceiverGroup.All
                    }, new SendOptions()
                    {
                        Reliability = true,
                        Encrypt = encrypt
                    });
                    return;
                case RpcTarget.MasterClient:
                    client.OpRaiseEvent(200, hashtable, new RaiseEventOptions()
                    {
                        Receivers = ReceiverGroup.MasterClient
                    }, new SendOptions()
                    {
                        Reliability = true,
                        Encrypt = encrypt
                    });
                    return;
            }


        }
        public static void RegisterTypes(this PhotonClient client)
        {
            Type typeFromHandle = typeof(Vector3);
            byte code = 86;
            SerializeMethod Serialize1 = new SerializeMethod(JFAFAPNBFLC);
            DeserializeMethod DeSerialize1 = new DeserializeMethod(IKFICLDHAOI);
            PhotonPeer.RegisterType(typeFromHandle, code, Serialize1, DeSerialize1);
            Type typeFromHandle2 = typeof(Quaternion);
            byte code2 = 81;
            SerializeMethod Serialize2 = new SerializeMethod(FECGOFBPKPP);
            DeserializeMethod DeSerialize2 = new DeserializeMethod(KCFONHKNDJH);
            PhotonPeer.RegisterType(typeFromHandle2, code2, Serialize2, DeSerialize2);
            byte b = 100;
            Type typeFromHandle3 = typeof(Vector2);
            SerializeMethod Serialize3 = new SerializeMethod(KMIBNBFAHEN);
            DeserializeMethod DeSerialize3 = new DeserializeMethod(HHBPLJKDKPI);
            GCJHKDFNJEA(typeFromHandle3, ref b, Serialize3, DeSerialize3);
            Type typeFromHandle4 = typeof(Vector4);
            SerializeMethod Serialize4 = new SerializeMethod(DFMLPAEPHJD);
            DeserializeMethod DeSerialize4 = new DeserializeMethod(GPJDPFFENPL);
            GCJHKDFNJEA(typeFromHandle4, ref b, Serialize4, DeSerialize4);
        }
        private static object GPJDPFFENPL(byte[] FBNNBMOEGOK)
        {
            int num = 0;
            float x;
            Protocol.Deserialize(out x, FBNNBMOEGOK, ref num);
            float y;
            Protocol.Deserialize(out y, FBNNBMOEGOK, ref num);
            float z;
            Protocol.Deserialize(out z, FBNNBMOEGOK, ref num);
            float w;
            Protocol.Deserialize(out w, FBNNBMOEGOK, ref num);
            return new Vector4(x, y, z, w);
        }
        private static byte[] DFMLPAEPHJD(object LIFOGCIHAMI)
        {
            byte[] array = new byte[16];
            int num = 0;
            Protocol.Serialize(((Vector4)LIFOGCIHAMI).x, array, ref num);
            Protocol.Serialize(((Vector4)LIFOGCIHAMI).y, array, ref num);
            Protocol.Serialize(((Vector4)LIFOGCIHAMI).z, array, ref num);
            Protocol.Serialize(((Vector4)LIFOGCIHAMI).w, array, ref num);
            return array;
        }

        private static void GCJHKDFNJEA(Type GMLMGAFFKEL, ref byte JGJGAENECKO, SerializeMethod HNDHEEONDNN, DeserializeMethod EBMBMGANEJJ)
        {
            byte code;
            JGJGAENECKO = Convert.ToByte((code = JGJGAENECKO) + 1);
            if (PhotonPeer.RegisterType(GMLMGAFFKEL, code, HNDHEEONDNN, EBMBMGANEJJ) && !ILHFIJHBPKK.Contains(GMLMGAFFKEL))
            {
                ILHFIJHBPKK.Add(GMLMGAFFKEL);
            }
        }
        private static object HHBPLJKDKPI(byte[] FBNNBMOEGOK)
        {
            int num = 0;
            float x;
            Protocol.Deserialize(out x, FBNNBMOEGOK, ref num);
            float y;
            Protocol.Deserialize(out y, FBNNBMOEGOK, ref num);
            return new Vector2(x, y);
        }

        private static byte[] KMIBNBFAHEN(object LIFOGCIHAMI)
        {
            byte[] array = new byte[8];
            int num = 0;
            Protocol.Serialize(((Vector2)LIFOGCIHAMI).x, array, ref num);
            Protocol.Serialize(((Vector2)LIFOGCIHAMI).y, array, ref num);
            return array;
        }

        private static object KCFONHKNDJH(byte[] FBNNBMOEGOK)
        {
            int num = 0;
            float x;
            Protocol.Deserialize(out x, FBNNBMOEGOK, ref num);
            float y;
            Protocol.Deserialize(out y, FBNNBMOEGOK, ref num);
            float z;
            Protocol.Deserialize(out z, FBNNBMOEGOK, ref num);
            float w;
            Protocol.Deserialize(out w, FBNNBMOEGOK, ref num);
            return new Quaternion(x, y, z, w);
        }
        private static byte[] FECGOFBPKPP(object LIFOGCIHAMI)
        {
            byte[] array = new byte[16];
            int num = 0;
            Protocol.Serialize(((Quaternion)LIFOGCIHAMI).x, array, ref num);
            Protocol.Serialize(((Quaternion)LIFOGCIHAMI).y, array, ref num);
            Protocol.Serialize(((Quaternion)LIFOGCIHAMI).z, array, ref num);
            Protocol.Serialize(((Quaternion)LIFOGCIHAMI).w, array, ref num);
            return array;
        }
        private static object IKFICLDHAOI(byte[] FBNNBMOEGOK)
        {
            int num = 0;
            float x;
            Protocol.Deserialize(out x, FBNNBMOEGOK, ref num);
            float y;
            Protocol.Deserialize(out y, FBNNBMOEGOK, ref num);
            float z;
            Protocol.Deserialize(out z, FBNNBMOEGOK, ref num);
            return new Vector3(x, y, z);
        }
        private static byte[] JFAFAPNBFLC(object LIFOGCIHAMI)
        {
            byte[] array = new byte[12];
            int num = 0;
            Protocol.Serialize(((Vector3)LIFOGCIHAMI).x, array, ref num);
            Protocol.Serialize(((Vector3)LIFOGCIHAMI).y, array, ref num);
            Protocol.Serialize(((Vector3)LIFOGCIHAMI).z, array, ref num);
            return array;
        }
        public static void SetAuthentication(this PhotonClient client, string authCookie, string userID)
        {
            Console.WriteLine(authCookie);
            client.AuthValues = new AuthenticationValues();
            client.AuthValues.AuthType = Photon.Realtime.CustomAuthenticationType.Custom;
            client.AuthValues.AddAuthParameter("token", authCookie);
            client.AuthValues.AddAuthParameter("user", userID);
        }
        public static void SetCustomProperties(this PhotonClient client, string authCookie, string userID, string avatarID)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            Uri target = new Uri("https://www.vrchat.com");
            handler.CookieContainer.Add(new Cookie("auth", authCookie) { Domain = target.Host });
            HttpClient http = new HttpClient(handler);

            var response = http.PutAsync($"https://www.vrchat.com/api/1/users/{userID}/avatar?apiKey=JlE5Jldo5Jibnk5O5hTx6XVqsJu4WJ26", new StringContent("{\"avatarId\":\"" + avatarID + "\"}", Encoding.UTF8, "application/json"));

            Dictionary<string, object> userProperties = new Dictionary<string, object>();
            userProperties["id"] = userID;

            var json = JsonConvert.DeserializeObject<AvatarInformation>(new WebClient().DownloadString($"https://www.vrchat.com/api/1/avatars/{avatarID}?apiKey=JlE5Jldo5Jibnk5O5hTx6XVqsJu4WJ26"));
            Dictionary<string, object> avatarProperties = new Dictionary<string, object>();
            Dictionary<string, object> UnityPackages = new Dictionary<string, object>();

            var unityPackage = json.unityPackages[0];
            UnityPackages.Add("id", unityPackage.id);
            UnityPackages.Add("assetUrl", unityPackage.assetUrl);
            UnityPackages.Add("assetUrlObject", "{}");
            UnityPackages.Add("unityVersion", unityPackage.unityVersion);
            UnityPackages.Add("unitySortNumber", unityPackage.unitySortNumber);
            UnityPackages.Add("assetVersion", unityPackage.assetVersion);
            UnityPackages.Add("platform", unityPackage.platform);
            UnityPackages.Add("created_at", unityPackage.created_at.ToShortDateString());

            avatarProperties.Add("name", json.name);
            avatarProperties.Add("description", json.description);
            avatarProperties.Add("authorId", json.authorId);
            avatarProperties.Add("authorName", json.authorName);
            avatarProperties.Add("imageUrl", json.imageUrl);
            avatarProperties.Add("thumbnailImageUrl", json.thumbnailImageUrl);
            avatarProperties.Add("assetUrl", json.assetUrl);
            avatarProperties.Add("assetUrlObject", "{}");
            avatarProperties.Add("tags", json.tags);
            avatarProperties.Add("releaseStatus", json.releaseStatus);
            avatarProperties.Add("version", json.version);
            avatarProperties.Add("unityPackageUrl", json.unityPackageUrl);
            avatarProperties.Add("unityVersion", json.unityVersion);
            avatarProperties.Add("assetVersion", json.assetVersion);
            avatarProperties.Add("platform", json.platform);
            avatarProperties.Add("featured", json.featured);
            avatarProperties.Add("imported", json.imported);
            avatarProperties.Add("id", json.id);
            avatarProperties.Add("created_at", json.created_at.ToShortDateString());
            avatarProperties.Add("updated_at", json.updated_at.ToShortDateString());
            avatarProperties.Add("unityPackages", UnityPackages);

            Hashtable hashtable = new Hashtable();
            hashtable["user"] = userProperties;
            hashtable["avatarDict"] = avatarProperties;
            hashtable["modTag"] = string.Empty;
            hashtable["isInvisible"] = false;
            hashtable["avatarVariations"] = avatarProperties["id"];
            hashtable["status"] = "active";
            hashtable["statusDescription"] = "oi";
            hashtable["inVRMode"] = false;
            hashtable["showSocialRank"] = true;
            hashtable["steamUserID"] = "0";

            client.LocalPlayer.SetCustomProperties(hashtable);
        }

        public static void SetCustomProperties(this PhotonClient client, Hashtable table) => client.LocalPlayer.SetCustomProperties(table);
    }
}
