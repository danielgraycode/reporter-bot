using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporter_Bot
{
    class MyBot
    {
        DiscordClient discord;


        public MyBot()
        {
            discord = new DiscordClient(x =>
                {
                    x.LogLevel = LogSeverity.Info;
                   
                    
                    
                });

            discord.UsingCommands(x =>
                {
                    x.PrefixChar = ':';
                    x.AllowMentionPrefix = true;
                    x.HelpMode = HelpMode.Public;
                });

            var commands = discord.GetService<CommandService>();
            var spamfilter = 0;
            var welcomemessages = 0;

            commands.CreateCommand("status")
                .Description("For checking the bot status")
                .Do(async (e) =>
                    {

                        Console.WriteLine("Reporter-Bot running: " + Environment.Version);
                        await e.Channel.SendMessage("So um here is some info \n Reporter-bot running: `V" + Environment.Version + "` \n Operating system: `" + Environment.OSVersion + "` \n Directory: `" + Environment.CurrentDirectory + "` \n Dumping files to: `" + "G:/My Applications/LogDumps/Reporter-Bot!" + "`" );
                    });

            commands.CreateCommand("about")
                .Description("Brief info about Reporter-Bot")
                .Do (async (e) =>
                    {
                        await e.Channel.SendMessage(e.User.Mention + " Hi there! I am Reporter-Bot, and my master is Daniel. You can find stuff out about him at https://danielgray.me . Have a good day!");
                   
                    });
            commands.CreateCommand("clearterminal")
                .AddCheck((cmd, u, ch) => u.Id == 137957400765267968)
                .Do(async (e) =>
                {
                    Console.Clear();
                    await e.Channel.SendMessage("Console has been cleared!");
                    Console.WriteLine("Reporter-Bot Running!");
                });
            commands.CreateCommand("getlogs")
               .Description("Sends the log files")
               .Do(async (e) =>
                   {
                       Console.WriteLine("[" + e.Server.Name + "] " + e.User.Name + "#" + e.User.Discriminator + " has requested the logs");
                       await e.Channel.SendFile(@"G:\My Applications\LogDumps\Reporter-Bot\bannedandunbanned.txt");
                       await e.Channel.SendFile(@"G:\My Applications\LogDumps\Reporter-Bot\roles.txt");
                       await e.Channel.SendFile(@"G:\My Applications\LogDumps\Reporter-Bot\members.txt");
                       await e.Channel.SendMessage(e.User.Mention + " here you go!");
                   });
            commands.CreateCommand("myprofile")
                .Description("Get some information about your profile!")
                .Alias("myinfo")
                .Do(async (e) =>
                    {
                        await e.Channel.SendMessage("Info for: " + e.User.Name + "#" + e.User.Discriminator + "\n `Avatar:` " + e.User.AvatarUrl + "\n Joined server on: `" + e.User.JoinedAt + "` \n Current status: `" + e.User.Status + "` \n `User ID: " + e.User.Id + "` \n ```Reporter-Bot by Daniel- https://danielgray.me (THE PROFILE FEATURE IS BUGGY!) ```");
                    });
            commands.CreateCommand("defaultstatus")
                .AddCheck((cmd, u, ch) => u.Id == 137957400765267968)
                .Do(async (e) =>
            {
                Console.WriteLine("Status updated");
                discord.SetGame("Reporting For Duty!");
                await e.Channel.SendMessage(e.User.Mention + " Status updated!");
            });
            commands.CreateCommand("helpstatus")
                 .AddCheck((cmd, u, ch) => u.Id == 137957400765267968)
                 .Do(async (e) =>
                     {
                         Console.WriteLine("Status updated");
                         discord.SetGame("Use ': help' for help!");
                         await e.Channel.SendMessage(e.User.Mention + " Status updated!");
                     });
            commands.CreateCommand("spamfilter")
            .AddCheck((cmd, u, ch) => u.Id == 137957400765267968)
            .Do(async (e) =>
                {
                    if (spamfilter == 0)
                    {
                        spamfilter = 1;
                        await e.Channel.SendMessage(e.User.Mention + " Spam filter updated");
                        Console.WriteLine(e.User.Name + "#" + e.User.Discriminator + " Updated the spam filter");
                    }
                    else if (spamfilter == 1)
                    {
                        spamfilter = 0;
                        await e.Channel.SendMessage(e.User.Mention + " Spam filter updated");
                        Console.WriteLine(e.User.Name + "#" + e.User.Discriminator + " Updated the spam filter");
                    }
                   });
            commands.CreateCommand("welcomemessages")
                .AddCheck((cmd, u, ch) => u.Id == 137957400765267968)
                .Description("Toggles welcome messages")
                .Do(async (e) =>
            {
                if (welcomemessages == 0)
                {
                    welcomemessages = 1;
                    await e.Channel.SendMessage(e.User.Mention + " Welcome Messages Updated");
                    Console.WriteLine(e.User.Name + "#" + e.User.Discriminator + " Toggled the Welcome Messages");
                }
                else if (welcomemessages == 1)
                { 
                    welcomemessages = 0;
                     await e.Channel.SendMessage(e.User.Mention + " Welcome Messages Updated");
                    Console.WriteLine(e.User.Name + "#" + e.User.Discriminator + " Toggled the Welcome Messages");
                }
            });

            discord.MessageReceived += async (s, e) =>
              {
                  var message = e.Message.Text;
                 if (e.User.IsBot) return;
                 if (message.Contains("nigger"))
                 {
                     await e.Message.Delete();
                     await e.Channel.SendMessage(e.User.Mention + " Naughty!");
                 }
                 };
             
            commands.CommandErrored += async (s, e) =>
                {
                    if (e.User.IsBot == true) return;
                    Console.WriteLine("[" + e.Server.Name + "]" + " An error occoured when User: " + e.User.Name + "#" + e.User.Discriminator + " tried to execute a command");
                    await e.Channel.SendMessage(e.User.Mention + " :no_entry_sign: So um.. Something went wrong when I tried to do this, do I have the permissions or does it definately exist? (Use : help if you need) :no_entry_sign:  ");
                };

             discord.UserBanned += async (s, e) =>
            {
                     Console.WriteLine("[" + e.Server.Name + "] " + e.User.Name + "#" + e.User.Discriminator +  " Has Been Banned ");
                     string banneduser = e.User.Name + "#" + e.User.Discriminator + ", user has been banned, server: " + e.Server.Name;
                     System.IO.File.WriteAllText(@"G:\My Applications\LogDumps\Reporter-Bot\bannedandunbanned.txt", banneduser);
            };

             discord.UserUnbanned += async (s, e) =>
                 {
                     Console.WriteLine("[" + e.Server.Name + "] " + e.User.Name + "#" + e.User.Discriminator + " Has Been unbanned ");
                     string unbanneduser = e.User.Name + "#" + e.User.Discriminator + ", user has been unbanned, server: " + e.Server.Name;
                     System.IO.File.WriteAllText(@"G:\My Applications\LogDumps\Reporter-Bot\bannedandunbanned.txt", unbanneduser);
                 };

             discord.UserLeft += async (s, e) =>
                 {
                     Console.WriteLine("[" + e.Server.Name + "] " + e.User.Name + "#" + e.User.Discriminator + " Has left the server");
                     string exuser = e.User.Name + "#" + e.User.Discriminator + ", user has been kicked/left, server: " + e.Server.Name;
                     System.IO.File.WriteAllText(@"G:\My Applications\LogDumps\Reporter-Bot\members.txt", exuser);
                 };

             discord.UserJoined += async (s, e) =>
                 {
                     Console.WriteLine("[" + e.Server.Name + "] " + e.User.Name + "#" + e.User.Discriminator + " Has joined the server");

                     if (welcomemessages == 1) 
                     {
                         await e.Server.DefaultChannel.SendMessage("Please welcome " + e.User.Mention + " to the server! Make sure you obey the rules and enjoy!");
                     };

                     if (spamfilter == 1)
                     {
                         if (e.Server == null) return;
                         if (e.Server.Id == 212916064298729473)
                         {
                             
                             if (e.Server == null) return;
                             if (e.User == null) return;
                             await e.User.AddRoles((e.Server.GetRole(241969040279601153)));
                             await e.Server.DefaultChannel.SendMessage(e.User.Mention + " Hi there! The spam filter is currently enabled, so you cant talk, just wait for an admin to de-rank you.");
                         }
                     }
                 };
                     discord.MessageDeleted += async (s, e) =>
                     {
                         if (e.Server == null) return;
                         if (e.User == null) return;
                         if (e.Channel == null) return;
                         Console.WriteLine("[" + e.Server.Name + "] " + " A message by " + e.User.Name + " has been deleted in channel: " + e.Channel.Name);
                     };
                     discord.JoinedServer += async (s, e) =>
                         {
                             Console.WriteLine("Added to Server: " + e.Server.Name + " Owned by: " + e.Server.Owner);
                         };
                     discord.LeftServer += async (s, e) =>
                         {
                             Console.WriteLine("Removed from Server: " + e.Server.Name + " Owned by: " + e.Server.Owner);
                         };
                     discord.RoleUpdated += async (s, e) =>
                         {
                             Console.WriteLine("[" + e.Server.Name + "]" + " Role Edited: " + e.Before.Name);
                         };
                     discord.ChannelUpdated += async (s, e) =>
                         {
                             Console.WriteLine("[" + e.Server.Name + "]" + " Channel: " + e.Before.Name + " has been updated");
                         };
                     discord.RoleCreated += async (s, e) =>
                         {
                             Console.WriteLine("[" + e.Server.Name + "]" + " Role Created: " + e.Role.Name + " kick and ban perms? " + e.Role.Permissions.KickMembers + " " + e.Role.Permissions.BanMembers);
                             string newrole = e.Role.Name + ", role created, server: " + e.Server.Name;
                             System.IO.File.WriteAllText(@"G:\My Applications\LogDumps\Reporter-Bot\roles.txt", newrole);

                         };
                     discord.RoleDeleted += async (s, e) =>
                         {
                             Console.WriteLine("[" + e.Server.Name + "]" + " Role Deleted: " + e.Role.Name);
                             string deletedrole = e.Role.Name + ", role deleted, server: " + e.Server.Name;
                             System.IO.File.WriteAllText(@"G:\My Applications\LogDumps\Reporter-Bot\roles.txt", deletedrole);
                         };

                     discord.Ready += async (s, e) =>
                         {
                             discord.SetGame("Reporting for duty!");
                         };


                     //Start-up and Login
                     discord.ExecuteAndWait(async () =>
                         {
                             await discord.Connect("nobottokenforyouguys", TokenType.Bot);
                             Console.Title = "Reporter-Bot";
                             Console.WriteLine("Reporter-Bot Running!");

                         });


                 }
        


    }

    }

