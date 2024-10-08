using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    //最大HPと現在のHP。
    int maxHp = 100;
    int currentHp;
    //Sliderを入れる
    [SerializeField]
    private  Slider slider;

 
    //始まった時
    public void Init(int MaxHp)
    {
        maxHp = MaxHp; 
        currentHp = maxHp;
        slider.maxValue = maxHp;
        slider.value = maxHp;

    }
    //敵のHpが減った時
    public void SetHp(int hp)
    {
        currentHp -= hp;
        slider.value = currentHp;  
    }

}