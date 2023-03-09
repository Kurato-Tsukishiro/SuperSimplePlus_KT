using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx.IL2CPP.Utils;
using HarmonyLib;
using UnityEngine;
using static System.String;
using System.IO;

namespace SuperSimplePlus.Patches;

// 参考=>https://github.com/ykundesu/SuperNewRoles/blob/master/SuperNewRoles/Patches/ChatHandlerPatch.cs
[HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))]
class AddChatPatch
{
    /// <summary>
    /// チャットに流れた文字をチャットログを作成するメソッドに渡す
    /// </summary>
    /// <param name="sourcePlayer">チャット送信者</param>
    /// <param name="chatText">チャット内容</param>
    /// <returns>true:チャットをチャットに表記する / false:表記しない, 消す</returns>
    public static bool Prefix(PlayerControl sourcePlayer, string chatText)
    {
        SaveChatLogPatch.SaveLog(SaveChatLogPatch.GetChat(sourcePlayer, chatText));
        return true; // Chatは消さない!!
    }
}

internal static class SaveChatLogPatch
{
    /// <summary>
    /// チャット内容をlogに記載する為加工する。
    /// </summary>
    /// <param name="sourcePlayer">チャット送信者</param>
    /// <param name="chatText">チャット内容</param>
    /// <returns> chatLog : 加工した文字列</returns>
    internal static string GetChat(PlayerControl sourcePlayer, string chatText)
    {
        string chatLog = null;
        string date = DateTime.Now.ToString("yy/MM/dd_HH:mm:ss");
        chatLog = $"[{date}] {sourcePlayer.name} :「 {chatText} 」";

        return chatLog;
    }

    /// <summary>
    /// チャットログをファイルに出力する
    /// </summary>
    /// <param name="chatLog"></param>
    internal static void SaveLog(string chatLog)
    {
        Logger.Info(chatLog);
    }
}
