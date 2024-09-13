using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TitelButtonUtil : ButtonUIUtil
{
  enum TaitelButtonType
    {
        Strt,
        License,
    }

    [SerializeField]
    private TaitelButtonType taitelButtonType = TaitelButtonType.Strt;

    [SerializeField]
    private GameObject LicensePanel;

    public bool isSelect = false;

    private Vector3 ActiveSize = Vector3.zero;

    private Vector3 DefaultSize = Vector3.zero;
    public override void OnNext()
    {
        if (LicensePanel.activeSelf)
        {
            LicensePanel.SetActive(false);
            SoundManager.Instance.PlaySE(SESoundData.SE.Select);
            return;
        }

        switch (taitelButtonType)
        {

            case TaitelButtonType.Strt:
                if (LicensePanel.activeSelf) return;
                FadeManager.Instance.LoadScene("StageSelect", 1.0f);
                SoundManager.Instance.PlaySE(SESoundData.SE.Select);
                break;

                case TaitelButtonType.License:
                if (!LicensePanel.activeSelf)
                {
                    LicensePanel.SetActive(true);
                    SoundManager.Instance.PlaySE(SESoundData.SE.Select);
                }

                break ;
        }
        
    }

    private void Start()
    {
        DefaultSize = this.transform.localScale;
        ActiveSize = new Vector3(DefaultSize.x * 1.1f, DefaultSize.y * 1.1f, DefaultSize.z * 1.1f);
    }
    public void Update()
    {
        if (!LicensePanel.activeSelf)
        {
            if (isSelect)
            {
                this.transform.localScale = ActiveSize;
            }
            else
            {
                this.transform.localScale = DefaultSize;
            }
        }
        else
        {
            this.transform.localScale = DefaultSize;
        }
    }
}
