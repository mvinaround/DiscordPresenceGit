﻿using System;
using System.Linq;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;
using System.Globalization;
using System.IO;

class Program
{
    // Request user's avatar data. Sizes can be powers of 2 between 16 and 2048
    static void FetchAvatar(Discord.ImageManager imageManager, Int64 userID)
    {
        imageManager.Fetch(Discord.ImageHandle.User(userID), (result, handle) =>
        {
            {
                if (result == Discord.Result.Ok)
                {
                    // You can also use GetTexture2D within Unity.
                    // These return raw RGBA.
                    var data = imageManager.GetData(handle);
                    Console.WriteLine("image updated {0} {1}", handle.Id, data.Length);
                }
                else
                {
                    Console.WriteLine("image error {0}", handle.Id);
                }
            }
        });
    }

    static void DotLogo()
	{
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("██████╗  ██████╗ ████████╗    ██████╗  ██████╗    ██████╗ ██████╗ ███████╗███████╗███████╗███╗   ██╗ ██████╗███████╗");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("██╔══██╗██╔═══██╗╚══██╔══╝    ██╔══██╗██╔════╝    ██╔══██╗██╔══██╗██╔════╝██╔════╝██╔════╝████╗  ██║██╔════╝██╔════╝");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("██║  ██║██║   ██║   ██║       ██║  ██║██║         ██████╔╝██████╔╝█████╗  ███████╗█████╗  ██╔██╗ ██║██║     █████╗  ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("██║  ██║██║   ██║   ██║       ██║  ██║██║         ██╔═══╝ ██╔══██╗██╔══╝  ╚════██║██╔══╝  ██║╚██╗██║██║     ██╔══╝  ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("██████╔╝╚██████╔╝   ██║       ██████╔╝╚██████╗    ██║     ██║  ██║███████╗███████║███████╗██║ ╚████║╚██████╗███████╗");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("╚═════╝  ╚═════╝    ╚═╝       ╚═════╝  ╚═════╝    ╚═╝     ╚═╝  ╚═╝╚══════╝╚══════╝╚══════╝╚═╝  ╚═══╝ ╚═════╝╚══════╝");
    }

    // Update user's activity for your game.
    // Party and secrets are vital.
    // Read https://discordapp.com/developers/docs/rich-presence/how-to for more details.
    static void UpdateActivity(Discord.Discord discord, Discord.Lobby lobby)
    {
        var activityManager = discord.GetActivityManager();
        var lobbyManager = discord.GetLobbyManager();

        string Details = "";
        string State = "";
        string LargeImage = "";
        string LargeText = "";
        string SmallImage = "";
        string SmallText = "";
        bool FileExists = true;
        bool WriteToConfig = false;


        Console.Clear();

        string ExeDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

        DotLogo();
        Thread.Sleep(1000);

        Console.WriteLine("");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("");
        Console.WriteLine("########### Executing Directory:" + ExeDirectory);
        Console.WriteLine("");
        Console.WriteLine("########### Checking For Config.");
        Console.WriteLine("########### " + ExeDirectory + "config.txt");
        Console.WriteLine("");
        Console.WriteLine("");
        if (File.Exists(ExeDirectory + "config.txt"))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Config File Exists. Use Previous Config? Y/N");
            string Choice = Console.ReadLine();
            if (Choice == "Y" || Choice == "y")
            {
                //Load config to array
                using (var reader = new StreamReader(ExeDirectory + "config.txt"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        string[] parts = line.Split(",");

                        //Put back into the said variables
                        Details = parts[0];
                        State = parts[1];
                        LargeImage = parts[2];
                        LargeText = parts[3];
                        SmallImage = parts[4];
                        SmallText = parts[5];
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("########### EXISTING CONFIG ###########");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine();
                        Console.WriteLine("## Details (Top Text): " + Details);
                        Console.WriteLine();
                        Console.WriteLine("## State (Bottom Text): " + State);
                        Console.WriteLine();
                        Console.WriteLine("## Large Image (Large Image Name): " + LargeImage);
                        Console.WriteLine();
                        Console.WriteLine("## Large Image Text (Lage Image Alt Text): " + LargeText);
                        Console.WriteLine();
                        Console.WriteLine("## Small Image (Small Image Name): " + SmallImage);
                        Console.WriteLine();
                        Console.WriteLine("## Small Text (Small Image Alt Text): " + SmallText);
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("#######################################");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Press Enter To Apply!");
                        Console.ReadLine();
                        Console.WriteLine("Applying...");
                        Thread.Sleep(1000);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        DotLogo();
                        Console.WriteLine("Back To Discord API...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            else
            {
                FileExists = false;
            }
        }
		else
		{
            FileExists= false;
		}

        if (!FileExists)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("########### Config doesn't exist...");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("########### Create/Update config? Y/N");
            string Choice = Console.ReadLine();
            if(Choice == "Y" || Choice == "y")
			{
                using (StreamWriter fs = File.CreateText(ExeDirectory + "config.txt"))
				{
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Clear();
                    DotLogo();
                    Console.WriteLine();
                    Console.Write("Top Line Text: ");
                    Details = Console.ReadLine();

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Clear();
                    DotLogo();
                    Console.WriteLine();
                    Console.Write("Bottom Line Text: ");
                    State = Console.ReadLine();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Clear();
                    DotLogo();
                    Console.WriteLine();
                    Console.WriteLine("Large Image Name:");
                    LargeImage = Console.ReadLine();

                    Console.Clear();
                    DotLogo();
                    Console.WriteLine("");
                    Console.WriteLine("Large Image Alt Text (On Hover Text):");
                    LargeText = Console.ReadLine();

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Clear();
                    DotLogo();
                    Console.WriteLine();
                    Console.WriteLine("Small Image Name:");
                    SmallImage = Console.ReadLine();

                    Console.Clear();
                    DotLogo();
                    Console.WriteLine();
                    Console.WriteLine("Small Image Alt Text (On Hover Text):");
                    SmallText = Console.ReadLine();

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Clear();
                    DotLogo();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Saving Config O_O");
                    Thread.Sleep(1000);

                    fs.WriteLine(Details + "," + State + "," + LargeImage + "," + LargeText + "," + SmallImage + "," + SmallText);
                    Console.ForegroundColor = ConsoleColor.Red;
                    DotLogo();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("########### CONFIG ###########");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                    Console.WriteLine("## Details (Top Text): " + Details);
                    Console.WriteLine();
                    Console.WriteLine("## State (Bottom Text): " + State);
                    Console.WriteLine();
                    Console.WriteLine("## Large Image (Large Image Name): " + LargeImage);
                    Console.WriteLine();
                    Console.WriteLine("## Large Image Text (Lage Image Alt Text): " + LargeText);
                    Console.WriteLine();
                    Console.WriteLine("## Small Image (Small Image Name): " + SmallImage);
                    Console.WriteLine();
                    Console.WriteLine("## Small Text (Small Image Alt Text): " + SmallText);
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("#######################################");
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Applying...");
                    Thread.Sleep(1000);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    DotLogo();
                    Console.WriteLine("Back To Discord API...");
                    Console.ForegroundColor = ConsoleColor.White;
                }

			}
			else
			{
                Console.WriteLine("########### Set Manually? Y/N");
                string Choice2 = Console.ReadLine();
                if(Choice2 == "Y" || Choice2 == "y")
				{
                    Console.Clear();
                    DotLogo();
                    Console.WriteLine();
                    Console.Write("Top Line Text: ");
                    Details = Console.ReadLine();

                    Console.Clear();
                    DotLogo();
                    Console.WriteLine();
                    Console.Write("Bottom Line Text: ");
                    State = Console.ReadLine();

                    Console.Clear();
                    DotLogo();
                    Console.WriteLine();
                    Console.WriteLine("Large Image Name:");
                    LargeImage = Console.ReadLine();

                    Console.Clear();
                    DotLogo();
                    Console.WriteLine("");
                    Console.WriteLine("Large Image Alt Text (On Hover Text):");
                    LargeText = Console.ReadLine();

                    Console.Clear();
                    DotLogo();
                    Console.WriteLine();
                    Console.WriteLine("Small Image Name:");
                    SmallImage = Console.ReadLine();

                    Console.Clear();
                    DotLogo();
                    Console.WriteLine();
                    Console.WriteLine("Small Image Alt Text (On Hover Text):");
                    SmallText = Console.ReadLine();

                    Console.Clear();
                    DotLogo();
                    Console.WriteLine();
                    Console.WriteLine("Applying...");
                    Thread.Sleep(1000);
                    Console.Clear();
                    DotLogo();
                    Console.WriteLine("Back To Discord API...");


                }
				else
				{
                    Console.WriteLine("Goodbye!");
                    System.Environment.Exit(1);
				}
            }
            
        }

        /*

                Console.WriteLine("https://docs.google.com/document/d/1Cm3_OXq8CtSphx3RCFwoBjy56vQYzNsXr6h6Bi7dhGU/edit?usp=sharing");



                Console.Write("Top Text: ");
                string Details = Console.ReadLine();
                Console.Write("Bottom Text: ");
                string State = Console.ReadLine();



                Console.WriteLine("Type large image name (hit enter), then type large text to display while hovering over large image (then hit enter again).");
                string LargeImage = Console.ReadLine();
                string LargeText = Console.ReadLine();
                Console.WriteLine("Type small image name (hit enter), then type small text to display while hovering over small image (then hit enter for the last time).");
                string SmallImage = Console.ReadLine();
                string SmallText = Console.ReadLine();

                */


        int LobbySize = 42069;



        string[] Array1 = { Details, State, LargeImage, LargeText, SmallImage, SmallText };



        var activity = new Discord.Activity

        {
            Details = Array1[0], // top
            State = Array1[1], // bottom
            Timestamps =
            {
               // Start = 5,
               // End = 6,
            },
            Assets =
            {
                // bo3, caet, pausebruh, playbruh, tamama, vaporman, untity
                LargeImage = Array1[2],
                LargeText = Array1[3],
                SmallImage = Array1[4],
                SmallText = Array1[5],
            },
            Party = {
               Id = lobby.Id.ToString(),
               Size = {
                    CurrentSize = LobbySize,    // lobbyManager.MemberCount(lobby.Id),
                    MaxSize = LobbySize,        // (int)lobby.Capacity,
                },
            },
            Secrets = {
                Join = lobbyManager.GetLobbyActivitySecret(lobby.Id),
            },
            Instance = true,
        };

        activityManager.UpdateActivity(activity, result =>
        {
            Console.WriteLine("Update Activity {0}", result);

            // Send an invite to another user for this activity.
            // Receiver should see an invite in their DM.
            // Use a relationship user's ID for this.
            // activityManager
            //   .SendInvite(
            //       364843917537050624,
            //       Discord.ActivityActionType.Join,
            //       "",
            //       inviteResult =>
            //       {
            //           Console.WriteLine("Invite {0}", inviteResult);
            //       }
            //   );
        });
    }

    static void Main(string[] args)
    {
        // Use your client ID from Discord's developer site.
        var clientID = Environment.GetEnvironmentVariable("DISCORD_CLIENT_ID");
        if (clientID == null)
        {
            clientID = "880583607314440273";
        }
        var discord = new Discord.Discord(Int64.Parse(clientID), (UInt64)Discord.CreateFlags.Default);
        discord.SetLogHook(Discord.LogLevel.Debug, (level, message) =>
        {
            Console.WriteLine("Log[{0}] {1}", level, message);
        });

        var applicationManager = discord.GetApplicationManager();
        // Get the current locale. This can be used to determine what text or audio the user wants.
        Console.WriteLine("Current Locale: {0}", applicationManager.GetCurrentLocale());
        // Get the current branch. For example alpha or beta.
        Console.WriteLine("Current Branch: {0}", applicationManager.GetCurrentBranch());
        // If you want to verify information from your game's server then you can
        // grab the access token and send it to your server.
        //
        // This automatically looks for an environment variable passed by the Discord client,
        // if it does not exist the Discord client will focus itself for manual authorization.
        //
        // By-default the SDK grants the identify and rpc scopes.
        // Read more at https://discordapp.com/developers/docs/topics/oauth2
        // applicationManager.GetOAuth2Token((Discord.Result result, ref Discord.OAuth2Token oauth2Token) =>
        // {
        //     Console.WriteLine("Access Token {0}", oauth2Token.AccessToken);
        // });

        var activityManager = discord.GetActivityManager();
        var lobbyManager = discord.GetLobbyManager();
        // Received when someone accepts a request to join or invite.
        // Use secrets to receive back the information needed to add the user to the group/party/match
        activityManager.OnActivityJoin += secret =>
        {
            Console.WriteLine("OnJoin {0}", secret);
            lobbyManager.ConnectLobbyWithActivitySecret(secret, (Discord.Result result, ref Discord.Lobby lobby) =>
            {
                Console.WriteLine("Connected to lobby: {0}", lobby.Id);
                lobbyManager.ConnectNetwork(lobby.Id);
                lobbyManager.OpenNetworkChannel(lobby.Id, 0, true);
                foreach (var user in lobbyManager.GetMemberUsers(lobby.Id))
                {
                    lobbyManager.SendNetworkMessage(lobby.Id, user.Id, 0,
                        Encoding.UTF8.GetBytes(String.Format("Hello, {0}!", user.Username)));
                }
                UpdateActivity(discord, lobby);
            });
        };
        // Received when someone accepts a request to spectate
        activityManager.OnActivitySpectate += secret =>
        {
            Console.WriteLine("OnSpectate {0}", secret);
        };
        // A join request has been received. Render the request on the UI.
        activityManager.OnActivityJoinRequest += (ref Discord.User user) =>
        {
            Console.WriteLine("OnJoinRequest {0} {1}", user.Id, user.Username);
        };
        // An invite has been received. Consider rendering the user / activity on the UI.
        activityManager.OnActivityInvite += (Discord.ActivityActionType Type, ref Discord.User user, ref Discord.Activity activity2) =>
        {
            Console.WriteLine("OnInvite {0} {1} {2}", Type, user.Username, activity2.Name);
            // activityManager.AcceptInvite(user.Id, result =>
            // {
            //     Console.WriteLine("AcceptInvite {0}", result);
            // });
        };
        // This is used to register the game in the registry such that Discord can find it.
        // This is only needed by games acquired from other platforms, like Steam.
        // activityManager.RegisterCommand();

        var imageManager = discord.GetImageManager();

        var userManager = discord.GetUserManager();
        // The auth manager fires events as information about the current user changes.
        // This event will fire once on init.
        //
        // GetCurrentUser will error until this fires once.
        userManager.OnCurrentUserUpdate += () =>
        {
            var currentUser = userManager.GetCurrentUser();
            Console.WriteLine(currentUser.Username);
            Console.WriteLine(currentUser.Id);
        };
        // If you store Discord user ids in a central place like a leaderboard and want to render them.
        // The users manager can be used to fetch arbitrary Discord users. This only provides basic
        // information and does not automatically update like relationships.
        userManager.GetUser(450795363658366976, (Discord.Result result, ref Discord.User user) =>
        {
            if (result == Discord.Result.Ok)
            {
                Console.WriteLine("user fetched: {0}", user.Username);

                // Request users's avatar data.
                // This can only be done after a user is successfully fetched.
                FetchAvatar(imageManager, user.Id);
            }
            else
            {
                Console.WriteLine("user fetch error: {0}", result);
            }
        });

        var relationshipManager = discord.GetRelationshipManager();
        // It is important to assign this handle right away to get the initial relationships refresh.
        // This callback will only be fired when the whole list is initially loaded or was reset
        relationshipManager.OnRefresh += () =>
        {
            // Filter a user's relationship list to be just friends
            relationshipManager.Filter((ref Discord.Relationship relationship) => { return relationship.Type == Discord.RelationshipType.Friend; });
            // Loop over all friends a user has.
            Console.WriteLine("relationships updated: {0}", relationshipManager.Count());
            for (var i = 0; i < Math.Min(relationshipManager.Count(), 10); i++)
            {
                // Get an individual relationship from the list
                var r = relationshipManager.GetAt((uint)i);
                Console.WriteLine("relationships: {0} {1} {2} {3}", r.Type, r.User.Username, r.Presence.Status, r.Presence.Activity.Name);

                // Request relationship's avatar data.
                FetchAvatar(imageManager, r.User.Id);
            }
        };
        // All following relationship updates are delivered individually.
        // These are fired when a user gets a new friend, removes a friend, or a relationship's presence changes.
        relationshipManager.OnRelationshipUpdate += (ref Discord.Relationship r) =>
        {
            Console.WriteLine("relationship updated: {0} {1} {2} {3}", r.Type, r.User.Username, r.Presence.Status, r.Presence.Activity.Name);
        };

        lobbyManager.OnLobbyMessage += (lobbyID, userID, data) =>
        {
            Console.WriteLine("lobby message: {0} {1}", lobbyID, Encoding.UTF8.GetString(data));
        };
        lobbyManager.OnNetworkMessage += (lobbyId, userId, channelId, data) =>
        {
            Console.WriteLine("network message: {0} {1} {2} {3}", lobbyId, userId, channelId, Encoding.UTF8.GetString(data));
        };
        lobbyManager.OnSpeaking += (lobbyID, userID, speaking) =>
        {
            Console.WriteLine("lobby speaking: {0} {1} {2}", lobbyID, userID, speaking);
        };
        // Create a lobby.
        var transaction = lobbyManager.GetLobbyCreateTransaction();
        transaction.SetCapacity(6);
        transaction.SetType(Discord.LobbyType.Public);
        transaction.SetMetadata("a", "123");
        transaction.SetMetadata("a", "456");
        transaction.SetMetadata("b", "111");
        transaction.SetMetadata("c", "222");

        lobbyManager.CreateLobby(transaction, (Discord.Result result, ref Discord.Lobby lobby) =>
        {
            if (result != Discord.Result.Ok)
            {
                return;
            }

            // Check the lobby's configuration.
            Console.WriteLine("lobby {0} with capacity {1} and secret {2}", lobby.Id, lobby.Capacity, lobby.Secret);

            // Check lobby metadata.
            foreach (var key in new string[] { "a", "b", "c" })
            {
                Console.WriteLine("{0} = {1}", key, lobbyManager.GetLobbyMetadataValue(lobby.Id, key));
            }

            // Print all the members of the lobby.
            foreach (var user in lobbyManager.GetMemberUsers(lobby.Id))
            {
                Console.WriteLine("lobby member: {0}", user.Username);
            }

            // Send everyone a message.
            lobbyManager.SendLobbyMessage(lobby.Id, "Hello from C#!", (_) =>
            {
                Console.WriteLine("sent message");
            });

            // Update lobby.
            var lobbyTransaction = lobbyManager.GetLobbyUpdateTransaction(lobby.Id);
            lobbyTransaction.SetMetadata("d", "e");
            lobbyTransaction.SetCapacity(16);
            lobbyManager.UpdateLobby(lobby.Id, lobbyTransaction, (_) =>
            {
                Console.WriteLine("lobby has been updated");
            });

            // Update a member.
            var lobbyID = lobby.Id;
            var userID = lobby.OwnerId;
            var memberTransaction = lobbyManager.GetMemberUpdateTransaction(lobbyID, userID);
            memberTransaction.SetMetadata("hello", "there");
            lobbyManager.UpdateMember(lobbyID, userID, memberTransaction, (_) =>
            {
                Console.WriteLine("lobby member has been updated: {0}", lobbyManager.GetMemberMetadataValue(lobbyID, userID, "hello"));
            });

            // Search lobbies.
            var query = lobbyManager.GetSearchQuery();
            // Filter by a metadata value.
            query.Filter("metadata.a", Discord.LobbySearchComparison.GreaterThan, Discord.LobbySearchCast.Number, "455");
            query.Sort("metadata.a", Discord.LobbySearchCast.Number, "0");
            // Only return 1 result max.
            query.Limit(1);
            lobbyManager.Search(query, (_) =>
            {
                Console.WriteLine("search returned {0} lobbies", lobbyManager.LobbyCount());
                if (lobbyManager.LobbyCount() == 1)
                {
                    Console.WriteLine("first lobby secret: {0}", lobbyManager.GetLobby(lobbyManager.GetLobbyId(0)).Secret);
                }
            });

            // Connect to voice chat.
            lobbyManager.ConnectVoice(lobby.Id, (_) =>
            {
                Console.WriteLine("Connected to voice chat!");
            });

            // Setup networking.
            lobbyManager.ConnectNetwork(lobby.Id);
            lobbyManager.OpenNetworkChannel(lobby.Id, 0, true);

            // Update activity.
            UpdateActivity(discord, lobby);
        });

        /*
        var overlayManager = discord.GetOverlayManager();
        overlayManager.OnOverlayLocked += locked =>
        {
            Console.WriteLine("Overlay Locked: {0}", locked);
        };
        overlayManager.SetLocked(false);
        */

        var storageManager = discord.GetStorageManager();
        var contents = new byte[20000];
        var random = new Random();
        random.NextBytes(contents);
        Console.WriteLine("storage path: {0}", storageManager.GetPath());
        storageManager.WriteAsync("foo", contents, res =>
        {
            var files = storageManager.Files();
            foreach (var file in files)
            {
                Console.WriteLine("file: {0} size: {1} last_modified: {2}", file.Filename, file.Size, file.LastModified);
            }
            storageManager.ReadAsyncPartial("foo", 400, 50, (result, data) =>
            {
                Console.WriteLine("partial contents of foo match {0}", Enumerable.SequenceEqual(data, new ArraySegment<byte>(contents, 400, 50)));
            });
            storageManager.ReadAsync("foo", (result, data) =>
            {
                Console.WriteLine("length of contents {0} data {1}", contents.Length, data.Length);
                Console.WriteLine("contents of foo match {0}", Enumerable.SequenceEqual(data, contents));
                Console.WriteLine("foo exists? {0}", storageManager.Exists("foo"));
                storageManager.Delete("foo");
                Console.WriteLine("post-delete foo exists? {0}", storageManager.Exists("foo"));
            });
        });

        var storeManager = discord.GetStoreManager();
        storeManager.OnEntitlementCreate += (ref Discord.Entitlement entitlement) =>
        {
            Console.WriteLine("Entitlement Create1: {0}", entitlement.Id);
        };

        // Start a purchase flow.
        // storeManager.StartPurchase(487507201519255552, result =>
        // {
        //     if (result == Discord.Result.Ok)
        //     {
        //         Console.WriteLine("Purchase Complete");
        //     }
        //     else
        //     {
        //         Console.WriteLine("Purchase Canceled");
        //     }
        // });

        // Get all entitlements.
        storeManager.FetchEntitlements(result =>
        {
            if (result == Discord.Result.Ok)
            {
                foreach (var entitlement in storeManager.GetEntitlements())
                {
                    Console.WriteLine("entitlement: {0} - {1} {2}", entitlement.Id, entitlement.Type, entitlement.SkuId);
                }
            }
        });

        // Get all SKUs.
        storeManager.FetchSkus(result =>
        {
            if (result == Discord.Result.Ok)
            {
                foreach (var sku in storeManager.GetSkus())
                {
                    Console.WriteLine("sku: {0} - {1} {2}", sku.Name, sku.Price.Amount, sku.Price.Currency);
                }
            }
        });

        // Pump the event look to ensure all callbacks continue to get fired.
        try
        {
            while (true)
            {
                discord.RunCallbacks();
                lobbyManager.FlushNetwork();
                Thread.Sleep(1000 / 60);
            }
        }
        finally
        {
            discord.Dispose();
        }

    }
}
