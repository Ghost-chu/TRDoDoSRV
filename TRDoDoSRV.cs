using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;
using DoDo.Open.Sdk.Consts;
using DoDo.Open.Sdk.Models;
using DoDo.Open.Sdk.Models.ChannelMessages;
using DoDo.Open.Sdk.Models.Messages;
using DoDo.Open.Sdk.Services;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TRDoDoSRV.Config;
using TRDoDoSRV.DoDoSRV;
using Color = Microsoft.Xna.Framework.Color;

namespace TRDoDoSRV
{
    public class TRDoDoSRV : Mod
    {
        private OpenEventService openEventService;
        private OpenApiService openApiService;
        private DoDoConfig config;

        public override void Load()
        {
            config = ModContent.GetInstance<DoDoConfig>();
            if (Main.netMode == NetmodeID.Server)
            {
                InitDodoSRV();
                //On_RemadeChatMonitor.AddNewMessage += RemadeChatMonitor_AddNewMessage; //All messages forwarding
                On_ChatHelper.BroadcastChatMessageAs += RemadeChatHelper_BroadcastChatMessageAs;
            }
            else
            {
                Logger.Info("Disabling Mod functions... The mod shouldn't running on the client side.");
            }
        }

        private void RemadeChatHelper_BroadcastChatMessageAs(On_ChatHelper.orig_BroadcastChatMessageAs orig,
            byte messageauthor, NetworkText text, Color color, int excludedplayer)
        {
            orig(messageauthor, text, color, excludedplayer);
            var broadcastPlayerId = (int)messageauthor;
            ForwardDoDoMessage(broadcastPlayerId == 255
                ? $"**{text}**"
                : $"<{Main.player[(int)messageauthor].name}> {text}");
        }

        private void ForwardDoDoMessage(string text)
        {
            if (text.Contains("[DoDo")) return;
            try
            {
                text = RemoveITags(RemoveTerrariaColorCodes(text));
                openApiService.SetChannelMessageSendAsync(new SetChannelMessageSendInput<MessageBodyText>()
                {
                    ChannelId = config.ChannelId,
                    DodoSourceId = null,
                    MessageBody = new MessageBodyText() { Content = text },
                    ReferencedMessageId = null
                });
            }
            catch (Exception ex)
            {
                Logger.Warn(ex);
            }
        }

        private void InitDodoSRV()
        {
            if (config.BotClientId == null || config.BotToken == null)
            {
                throw new ArgumentException("BotClientId or BotToken is null in configuration");
            }

            if (config.ChannelId == null || config.IslandId == null)
            {
                throw new ArgumentException("ChannelId or IslandId is null in configuration");
            }

            openApiService = new OpenApiService(new OpenApiOptions
            {
                BaseApi = "https://botopen.imdodo.com",
                ClientId = config.BotClientId,
                Token = config.BotToken,
                Log = null
            });
            var channelId = config.ChannelId;
            var islandId = config.IslandId;

            Task.Run(() =>
            {
                ConnectDoDoWebSocket(channelId, islandId);
            });

        }

        public async void ConnectDoDoWebSocket(string channelId, string islandId)
        {
            var eventProcessService = new SRVWSEventHandler(this, openApiService, channelId, islandId);
            var openEventOptions = new OpenEventOptions { IsAsync = true };
            openEventService = new OpenEventService(openApiService, eventProcessService, openEventOptions);
            openEventOptions.Protocol = EventProtocolConst.WebSocket;
            openEventOptions.IsReconnect = true;
            await openEventService.ReceiveAsync();
        }

        public override void Unload()
        {
            openEventService?.StopReceiveAsync().Wait();
        }

        public static string RemoveITags(string input)
        {
            return Regex.Replace(input, @"\[i/.*?\]", "");
        }
        public static string RemoveTerrariaColorCodes(string input)
        {
            string output = input;
            int start = output.IndexOf("[c/");
            while (start != -1)
            {
                int end = output.IndexOf("]", start);
                if (end != -1)
                {
                    output = output.Substring(0, start) + output.Substring(start + 10, end - start - 10) + output.Substring(end + 1);
                }
                else
                {
                    break;
                }
                start = output.IndexOf("[c/");
            }
            return output;
        }



    }
}