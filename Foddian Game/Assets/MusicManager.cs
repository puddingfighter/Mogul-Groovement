using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;


public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip[] music;
    AudioSource player;
    [SerializeField] AudioSource crowd;
    [SerializeField] AudioSource LudSFX;
    [SerializeField] Animator musicUI;
    [SerializeField] private InputAction nextSong;
    [SerializeField] TextMeshProUGUI musicTitle;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SfXSlider;
    
    
    int idx=0;
    void Start()
    {
        player = GetComponent<AudioSource>();
        player.clip=music[idx];
        nextSong.Enable();
        player.Play();
    }

   
    void Update()
    {
        
        if(nextSong.triggered && nextSong.ReadValue<float>()==1)
        {
            PlayNextSong();
        }
        if(nextSong.triggered && nextSong.ReadValue<float>()==-1)
        {
            PlayPreviousSong();
        }
        if(player.time>player.clip.length-1)
        {
            PlayNextSong();
        }
    }
    void PlayNextSong()
    {

        idx++;
        if(idx>=music.Length)
            idx=0;
        player.clip= music[idx];
        musicTitle.text = music[idx].name;
        player.Play();
        musicUI.Play("MusicMenupopin",0,0.0f);

    }
    void PlayPreviousSong()
    {

        idx--;
        if(idx<=0)
            idx=music.Length-1;
        player.clip= music[idx];
        
        musicTitle.text = music[idx].name;
        player.Play();
        musicUI.Play("MusicMenupopin",0,0.0f);

    }
    public void changeVolume()
    {
        Debug.Log( musicSlider.value);
        player.volume= musicSlider.value;
    }
    
    public void changeSFX()
    {
        crowd.volume = SfXSlider.value;
        LudSFX.volume = SfXSlider.value;
    }
}
