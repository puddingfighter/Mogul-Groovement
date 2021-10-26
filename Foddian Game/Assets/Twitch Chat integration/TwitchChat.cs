﻿using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using UnityEngine;

public class TwitchChat : MonoBehaviour
{
    // might want to use these while testing with your own information
    // public string Password;
    // public string Username;
    // public string ChannelName;

    public static TwitchChat Instance {
        get {  
            if(_instance == null) {
                _instance = new TwitchChat();
            }
            return _instance; }
    }
    private static TwitchChat _instance;
    
    private CommandCollection _commands;
    private TcpClient _twitchClient;
    private StreamReader _reader;
    private StreamWriter _writer;


    void Awake(){
        _instance = this;
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if(_twitchClient != null && _twitchClient.Connected){
            ReadChat();    
        }
    }

    public void SetNewCommandCollection(CommandCollection commands){
         _commands = commands;
    }

    public void Connect(TwitchCredentials credentials, CommandCollection commands){
        _commands = commands;
        _twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        _reader = new StreamReader(_twitchClient.GetStream());
        _writer = new StreamWriter(_twitchClient.GetStream());

        _writer.WriteLine("PASS " + credentials.Password);
        _writer.WriteLine("NICK " + credentials.Username);
        _writer.WriteLine("USER " + credentials.Username + " 8 * :" + credentials.Username);
        _writer.WriteLine("JOIN #" + credentials.ChannelName);
        _writer.Flush();
    }

    private void ReadChat(){
        if(_twitchClient.Available > 0){
            string message = _reader.ReadLine();
            Debug.Log(message);
            
            // Twitch sends a PING message every 5 minutes or so. We MUST respond back with PONG or we will be disconnected 
            if (message.Contains("PING")) {
                _writer.WriteLine("PONG");
                _writer.Flush();
                return;
            }

            if (message.Contains("PRIVMSG")){
                var splitPoint = message.IndexOf("!", 1);
                var author = message.Substring(0, splitPoint);
                author = author.Substring(1);

                // users message
                splitPoint = message.IndexOf(":", 1);
                message = message.Substring(splitPoint + 1);

                if(message.StartsWith(TwitchCommands.CmdPrefix)){
                    // get the first word
                    int index =  message.IndexOf(" ");
                    string command = index > -1 ? message.Substring(0, index) : message;
                    _commands.ExecuteCommand(
                        command,
                        new TwitchCommandData {
                            Author = author,
                            Message = message
                    });
                }
            }
        }
    }
}

    

