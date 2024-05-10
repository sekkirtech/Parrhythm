using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScripts : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] AudioClip haku;
    [SerializeField] AudioClip hakufin;
    [SerializeField] AudioSource AudioSource1;

    private float time = 0.0f;

    int hakucount = 0;
    int MAXCount = 3;
    bool b = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("�U��");
            StartCoroutine(Hakuco());
        }
    }
    IEnumerator Hakuco()
    {
        AudioSource1.clip = haku;
        for (int i = 0; i < MAXCount; i++)
        {
            AudioSource1.Play();
            Debug.Log(i);
            yield return new WaitForSeconds(0.5f);
        }
        StartCoroutine(playerManager.EnemmyAttack());
        AudioSource1.clip = hakufin;
        AudioSource1.Play();
    }
}
