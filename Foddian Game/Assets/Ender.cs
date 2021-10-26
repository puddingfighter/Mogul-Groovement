using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ender : MonoBehaviour
{
    [SerializeField] Animator anim;
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        anim.Play("EndForever");
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene("EndGame");
    }
}
