using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    //�ő�HP�ƌ��݂�HP�B
    int maxHp = 100;
    int currentHp;
    //Slider������
    [SerializeField]
    private  Slider slider;

 
    //�n�܂�����
    public void Init(int MaxHp)
    {
        maxHp = MaxHp; 
        currentHp = maxHp;
        slider.maxValue = maxHp;
        slider.value = maxHp;

    }
    //�G��Hp����������
    public void SetHp(int hp)
    {
        currentHp -= hp;
        slider.value = currentHp;  
    }

}