using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    //Å‘åHP‚ÆŒ»İ‚ÌHPB
    int maxHp = 100;
    int currentHp;
    //Slider‚ğ“ü‚ê‚é
    [SerializeField]
    private  Slider slider;

 
    //n‚Ü‚Á‚½
    public void Init(int MaxHp)
    {
        maxHp = MaxHp; 
        currentHp = maxHp;
        slider.maxValue = maxHp;
        slider.value = maxHp;

    }
    //“G‚ÌHp‚ªŒ¸‚Á‚½
    public void SetHp(int hp)
    {
        currentHp -= hp;
        slider.value = currentHp;  
    }

}