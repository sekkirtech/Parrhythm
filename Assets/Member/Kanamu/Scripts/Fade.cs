using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
  public void Fade_Button()
    { 
         FadeManager.Instance.LoadScene("FadeTest", 1.0f);
    }
}
