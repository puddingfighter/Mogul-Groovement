using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGPlayer : MonoBehaviour
{
    public static RPGPlayer Instance{
        get {
                if(_instance == null)
                {
                    _instance = new RPGPlayer();
                }
                return _instance;
            }
        }

    public int CurrentXP = 0;
    private static RPGPlayer _instance;


    void Awake(){
        _instance = this;
        DontDestroyOnLoad(this);
        CurrentXP = 0;
    }

    public void AddXP(int amount){
        CurrentXP += amount;
        Debug.Log($"<color=green>RPG Player's current xp is: {CurrentXP}</color>");
    }
}
