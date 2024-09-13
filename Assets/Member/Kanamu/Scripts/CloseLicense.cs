using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CloseLicense : MonoBehaviour
{

    [SerializeField]
    private GameObject LicensePanel;
    // Start is called before the first frame update
    void Start()
    {
        ControllerManager.Instance.SouthButtonObservable
          .Where(_ => LicensePanel.activeSelf)
          .Subscribe(_ => LicensePanel.SetActive(false)).AddTo(this.gameObject);
    }

}
