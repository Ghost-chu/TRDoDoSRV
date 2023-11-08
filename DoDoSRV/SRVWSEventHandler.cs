using System;
using System.Text;
using DoDo.Open.Sdk.Models.Events;
using DoDo.Open.Sdk.Models.Messages;
using DoDo.Open.Sdk.Services;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Color = Microsoft.Xna.Framework.Color;

namespace TRDoDoSRV.DoDoSRV
{
    public class SRVWSEventHandler : DemoEventProcessService
    {
        private readonly TRDoDoSRV modInstance;
        private readonly string channelId;
        private readonly string islandId;

        public SRVWSEventHandler(TRDoDoSRV modInstance, OpenApiService openApiService, string channelId, string islandId) : base(openApiService)
        {
            this.modInstance = modInstance;
            this.channelId = channelId;
            this.islandId = islandId;
        }

        public override void Connected(string message)
        {
            base.Connected(message);
            Console.WriteLine("DoDoSRV 连接已建立：" + message);
        }

        public override void Disconnected(string message)
        {
            Console.WriteLine("DoDoSRV 连接已断开：" + message);
        }

        public override void Reconnected(string message)
        {
            Console.WriteLine("DoDoSRV 已重新连接到事件服务器：" + message);
        }

        public override void ChannelMessageEvent<T>(EventSubjectOutput<EventSubjectDataBusiness<EventBodyChannelMessage<T>>> input)
        {
            var eventBody = input.Data.EventBody;
            StringBuilder sb = new();
            sb.Append("[DoDo消息] ");
            if (eventBody.Reference != null)
            {
                sb.Append($"#回复-> {eventBody.Reference.NickName}# ");
            }

            sb.Append($"<{eventBody.Member.NickName}> ");

            switch (eventBody.MessageBody)
            {
                case MessageBodyText textMessageBody:
                    sb.Append(textMessageBody.Content);
                    break;
                case MessageBodyPicture picMessageBody:
                    sb.Append($"[图片: {picMessageBody.Width}px*{picMessageBody.Height}px]");
                    break;
                case MessageBodyFile fileMessageBody:
                    sb.Append($"[文件: {fileMessageBody.Name}({Util.BytesToString(fileMessageBody.Size)})]");
                    break;
                case MessageBodyVideo videoMessageBody:
                    sb.Append($"[视频: {videoMessageBody.Duration}s]");
                    break;
                case MessageBodyCard cardMessageBody:
                    sb.Append($"[卡片: {cardMessageBody.Content}]");
                    break;
                case MessageBodyShare shareMessageBody:
                    sb.Append($"[分享]");
                    break;
                case MessageBodyRedPacket redPacketMessageBody:
                    sb.Append($"[红包：{redPacketMessageBody.TotalAmount}元，共{redPacketMessageBody.TotalAmount}个]");
                    break;
                case MessageBodyBase unknownMessageBody:
                    sb.Append($"[未知消息：{unknownMessageBody.ToString()}");
                    break;
            }

            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(sb.ToString()), Color.White);
            Console.WriteLine(sb.ToString());
        }

        // public override void ChannelMessageEvent<T>(EventSubjectOutput<EventSubjectDataBusiness<EventBodyChannelMessage<T>>> input)
        // {
        //     var eventBody = input.Data.EventBody;
        //     if (!eventBody.IslandSourceId.Equals(islandId) || !eventBody.ChannelId.Equals(channelId))
        //     {
        //         return;
        //     }
        //     StringBuilder sb = new();
        //     sb.Append("[DoDo消息] ");
        //     if (eventBody.Reference != null)
        //     {
        //         sb.Append($"#回复-> {eventBody.Reference.NickName}# ");
        //     }
        //
        //     sb.Append($"<{eventBody.Member.NickName}> ");
        //
        //     switch (eventBody.MessageBody)
        //     {
        //         case MessageBodyText textMessageBody:
        //             sb.Append(textMessageBody.Content);
        //             break;
        //         case MessageBodyPicture picMessageBody:
        //             sb.Append($"[图片: {picMessageBody.Width}px*{picMessageBody.Height}px]");
        //             break;
        //         case MessageBodyFile fileMessageBody:
        //             sb.Append($"[文件: {fileMessageBody.Name}({Util.BytesToString(fileMessageBody.Size)})]");
        //             break;
        //         case MessageBodyVideo videoMessageBody:
        //             sb.Append($"[视频: {videoMessageBody.Duration}s]");
        //             break;
        //         case MessageBodyCard cardMessageBody:
        //             sb.Append($"[卡片: {cardMessageBody.Content}]");
        //             break;
        //         case MessageBodyShare shareMessageBody:
        //             sb.Append($"[分享]");
        //             break;
        //         case MessageBodyRedPacket redPacketMessageBody:
        //             sb.Append($"[红包：{redPacketMessageBody.TotalAmount}元，共{redPacketMessageBody.TotalAmount}个]");
        //             break;
        //         case MessageBodyBase unknownMessageBody:
        //             sb.Append($"[未知消息：{unknownMessageBody.ToString()}");
        //             break;
        //     }
        //     ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(sb.ToString()), Color.White);
        // }
    }
}
