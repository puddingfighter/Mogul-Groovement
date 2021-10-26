using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ITwitchCommandHandler
{
    void HandleCommmand(TwitchCommandData data);
}

public struct TwitchCommandData {
    public string Author;
    public string Message;
}

public struct TwitchCredentials {
    public string ChannelName;
    public string Username;
    public string Password;
}

public static class TwitchCommands {
    public static readonly string CmdPrefix = "!";
    public static readonly string CmdMessage = "message";
    public static readonly string CmdAddXP = "addxp";
}

// EXAMPLES - This is how I would impletement this interface and create classes with actual command logic

// !message command
public class TwitchDisplayMessageCommand : ITwitchCommandHandler {
    public void HandleCommmand(TwitchCommandData data){
        Debug.Log($"<color=cyan>Raw Message:{data.Message}</color>");

        // strip the !message command from the message and trim the leading whitespace
        string actualMessage = data.Message.Substring(0 + (TwitchCommands.CmdPrefix + TwitchCommands.CmdMessage).Length).TrimStart(' ');
        Debug.Log($"<color=cyan>{data.Author} says {actualMessage}</color>");
    }
}

// !addxp command
public class TwitchAddXPCommand : ITwitchCommandHandler {
    public void HandleCommmand(TwitchCommandData data){
        // add XP to character here
        Debug.Log($"<color=cyan>{data.Author} Adds 10 Expierence Points to player.</color>");
        RPGPlayer.Instance.AddXP(10);
    }
}

public class CommandCollection {

    private Dictionary<string, ITwitchCommandHandler> _commands;

    public CommandCollection(){
        _commands = new Dictionary<string, ITwitchCommandHandler>();
        _commands.Add(TwitchCommands.CmdMessage, new TwitchDisplayMessageCommand());
        _commands.Add(TwitchCommands.CmdAddXP, new TwitchAddXPCommand());
    }

    public bool HasCommand(string command){
        return _commands.ContainsKey(command) ? true : false;
    }

    public void ExecuteCommand(string command, TwitchCommandData data){
        command = command.Substring(1); // remove exclamation point
        if(HasCommand(command)){
            _commands[command].HandleCommmand(data);
        }
    }
}


