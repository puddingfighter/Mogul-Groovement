using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class TwitchConnection : MonoBehaviour
{
    TcpClient Twitch;
    StreamReader Reader;
    StreamWriter Writer;
    [SerializeField] Text Usernametxt;
    
    [SerializeField] TMP_InputField oAuthPasstxt;
    
    [SerializeField]  InputField Channeltxt;
    [SerializeField] GameObject LoginUI;
    const string URL="irc.chat.twitch.tv";
    const int PORT = 6667;
    string User;
    string OAuth;
    string Channel="";
    bool connected=false;
    float pingCounter=0;
    [SerializeField] Sprite[] emotes;
    [SerializeField] Image[] emoteTest;
    [SerializeField] SpriteRenderer[] crowd;
    [SerializeField] SpriteRenderer[] crowd1;
    
    [SerializeField] SpriteRenderer[] crowd2;
    
    [SerializeField] SpriteRenderer[] crowd3;
    
    [SerializeField] SpriteRenderer[] crowd4;
    
    [SerializeField] SpriteRenderer[] crowd5;
    [SerializeField] SpeechBubble[] SpeechBubbles;
    [SerializeField] Animator[] BubblesAnim;
    int emoteSlotIDX=0;
    int messageCounter=0;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        foreach(Sprite obj in emotes)
        {
            Debug.Log(obj);
        }
    }
    private void ConnectToTwitch()
    {
       Twitch= new TcpClient(URL,PORT);
       Reader = new StreamReader(Twitch.GetStream());
       Writer = new StreamWriter(Twitch.GetStream());
       Writer.WriteLine("PASS "+ OAuth);
       Writer.WriteLine("NICK "+ User.ToLower());
       Writer.WriteLine("JOIN #"+ Channel.ToLower());
       Writer.Flush();
       //ChangeScene();
       
    }
    public void Connect()
    {
        //didn't have time to encrypt this, While this account is 100% disposable, i would appreciate if you didn't nefariously use it
        //Could ruin the game for some people
        User="DisposableGameJamAcc";
        OAuth="oauth:x4ddunkdtvmbiqjhty8t70clbisbew";
        Channel=Channeltxt.text;

        
        
        ConnectToTwitch();
    }
    public void ChangeScene()
    {
        
    }
    void Update()
    {
        if(Twitch!=null)
        {
            pingCounter+=Time.deltaTime;
            if(pingCounter> 60)
            {
                Writer.WriteLine("PING "+ URL);
                Writer.Flush();
                pingCounter=0;
            }
            if(!Twitch.Connected)
            {
                ConnectToTwitch();
            }
            if(Twitch.Available>0)
            { 
                LoginUI.SetActive(false);
                string message = Reader.ReadLine();
                //:puddingfighter_!puddingfighter_@puddingfighter_.tmi.twitch.tv PRIVMSG #puddingfighter_ :fuck
                //:jaredthesheik!jaredthesheik@jaredthesheik.tmi.twitch.tv PRIVMSG #puddingfighter_ :nice
                if(message.Contains("PRIVMSG"))
                {
                    


                    int SplitPoint = message.IndexOf("!");
                    string chatter = message.Substring(1,SplitPoint -1);

                    SplitPoint= message.IndexOf(":", 1);
                    string msg= message.Substring(SplitPoint+1);

                    if(messageCounter>=10)
                    {
                        messageCounter=0;
                        
                        if(chatter.Length>13)
                            chatter= chatter.Remove(13, chatter.Length-13);

                        int messageindex=(int)UnityEngine.Random.Range(0,SpeechBubbles.Length);

                        SpeechBubbles[messageindex].UpdateText(chatter, msg);
                        BubblesAnim[messageindex].Play("SpeechbubblePop");
                       // test.UpdateText(chatter, ScuffedTextLenghtCalc(msg));
                        //testanim.Play("SpeechbubblePop");
                    }
                    messageCounter++;
                    foreach(Sprite obj in emotes)
                    {
                       
                        if(msg.Contains(obj.name.Split(' ')[0]))
                        {
                            crowd[emoteSlotIDX].sprite=obj;
                            crowd1[emoteSlotIDX].sprite=obj;
                            
                            crowd2[emoteSlotIDX].sprite=obj;
                            
                            crowd3[emoteSlotIDX].sprite=obj;
                            
                            crowd4[emoteSlotIDX].sprite=obj;
                            
                            crowd5[emoteSlotIDX].sprite=obj;
                            //emoteTest[emoteSlotIDX].sprite=obj;
                            emoteSlotIDX++;
                            if(emoteSlotIDX>=30)
                            emoteSlotIDX=0;
                        }
                    }
                    
                    
                }
               
            }
        }
        
    }
    private string ScuffedTextLenghtCalc(string msg)
    {
        
            if(msg.Length>17)   
            {
                msg=msg.Insert(17,Environment.NewLine);
                if(msg.Length>34)
                {
                    msg=msg.Insert(34,Environment.NewLine);
                }
                if(msg.Length>48)
                {
                    msg=msg.Remove(48, msg.Length-48);
                    msg=msg.Insert(48,"..");
                }
            }
            return msg;
    }
    public void CloseMenu()
    {
        LoginUI.SetActive(false);
    }
}
