using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace TRDoDoSRV.Config
{
    public class DoDoConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        [Header("BotClientId")]
        [ReloadRequired]
        public string BotClientId;
        [Header("BotToken")]
        [ReloadRequired]
        public string BotToken;
        [Header("IslandId")]
        [ReloadRequired]
        public string IslandId;
        [Header("ChannelId")]
        [ReloadRequired]
        public string ChannelId;
    }
}
