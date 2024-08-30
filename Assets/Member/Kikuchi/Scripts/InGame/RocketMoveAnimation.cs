using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using Unity.VisualScripting;

public class RocketMoveAnimation : MonoBehaviour
{

    [SerializeField]
    [Tooltip("１週にかかる時間")]
    private float moveTime = 1.0f;
    private Transform zRotTF;

    private Tween rotTween;
    // Start is called before the first frame update
    void Start()
    {
        zRotTF = transform.parent;
        RandomRotateRocket();
    }

    private void RandomRotateRocket()
    {
        int random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
            // Y軸回転
                Debug.Log("Y軸回転");
                rotTween?.Kill();
                zRotTF.transform.rotation = Quaternion.Euler(Vector3.zero);
                transform.rotation = Quaternion.Euler(transform.rotation.x,  -100, 0);
                rotTween = transform.DORotate(new Vector3(0, -360, 0), moveTime, RotateMode.WorldAxisAdd)
                .SetEase(Ease.Linear)
                .OnComplete(() => RandomRotateRocket());
                break;
            case 1:
            // X軸回転
                Debug.Log("X軸回転");
                rotTween?.Kill();
                zRotTF.transform.rotation = Quaternion.Euler(Vector3.zero);
                var y = Random.Range(-30, 30);
                transform.localRotation = Quaternion.Euler(100,  y, -90);
                rotTween = transform.DOLocalRotate(new Vector3(-260, y, -90), moveTime, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .OnComplete(() => RandomRotateRocket());
                break;
            case 2:
            // Z軸回転
                int randomZ = Random.Range(0, 2);
                if(randomZ == 0)
                {
                    Debug.Log("Z軸回転1");
                    rotTween?.Kill();
                    zRotTF.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
                    transform.localRotation = Quaternion.Euler(0, 100, 0);
                    rotTween = transform.DOLocalRotate(new Vector3(0, -360, 0), moveTime, RotateMode.WorldAxisAdd)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => RandomRotateRocket());
                }
                else
                {
                    Debug.Log("Z軸回転2");
                    rotTween?.Kill();
                    zRotTF.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -45));
                    transform.localRotation = Quaternion.Euler(0, 100, 0);
                    rotTween = transform.DOLocalRotate(new Vector3(0, -360, 0), moveTime, RotateMode.WorldAxisAdd)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => RandomRotateRocket());
                }
                break;
        }
    }

    void OnDestroy()
    {
        rotTween?.Kill();
    }

}
