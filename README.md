# TRDoDoSRV

适用于 [tModLoader](https://tmodloader.net/) 的 [DoDo](https://www.imdodo.com/) 双向聊天桥接插件。  
正在寻找适用于 Minecraft Spigot 的 DoDoSRV？[看看这里](https://github.com/Ghost-chu/DoDoSRV)

DoDoSRV 在客户端上不会工作（不会有任何效果！），请将其安装在服务端！（或通过游戏中的多人游戏->“开服并加入游戏”启动本地服务器）

[quote=tModLoader]Developed By Ghost_chu[/quote]

## 演示

![image](https://github.com/Ghost-chu/TRDoDoSRV/assets/30802565/0948c92b-da62-4320-a184-08ef65a75302)
![image](https://github.com/Ghost-chu/TRDoDoSRV/assets/30802565/f7c8fe85-a75f-410c-bf32-e8b3eb169392)
![image](https://github.com/Ghost-chu/TRDoDoSRV/assets/30802565/2287940b-f4b9-49bb-8ef6-f3eb1e84472f)

## 配置文件

TRDoDoSRV_DoDoConfig.json （请手动放置于 `ModConfigs` 目录中）

```json
{
    "BotClientId": "机器人ClientID",
    "BotToken": "机器人Token",
    "IslandId": "你的群组 ID，机器人只会监听此群组并向此群组发送消息",
    "ChannelId": "你的频道 ID，机器人只会监听此频道并仅向此频道发送消息"
}
```
