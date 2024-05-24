using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    //�ő�HP�ƌ��݂�HP�B
    int maxHp = 155;
    int currentHp;
    //Slider������
    public Slider slider;

    void Start()
    {
        //Slider�𖞃^���ɂ���B
        slider.value = 1;
        //���݂�HP���ő�HP�Ɠ����ɁB
        currentHp = maxHp;
        Debug.Log("Start currentHp : " + currentHp);
    }

    private void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.tag == "Enemy")
        {
            //�_���[�W�̃e�X�g
            //�_���[�W��1�`100�̒��Ń����_���Ɍ��߂�B
            int damage = Random.Range(1, 100);
            Debug.Log("damage : " + damage);

            //���݂�HP����_���[�W������
            currentHp = currentHp - damage;
            Debug.Log("After currentHp : " + currentHp);

            slider.value = (float)currentHp / (float)maxHp; ;
            Debug.Log("slider.value : " + slider.value);
        }
    }
}