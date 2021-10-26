using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SpeechBubble : MonoBehaviour
{
  [SerializeField] TextMeshPro nametxt;
  
  [SerializeField] TextMeshPro Texttxt;
  public void UpdateText(string name, string text)
  {
      nametxt.text=name;
      Texttxt.text= text;
  
  }
}
