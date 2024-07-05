using UnityEngine;
using DG.Tweening;

public class EnemyHandAnimation : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftHand;
    [SerializeField]
    private GameObject _rightHand;

    [SerializeField]
    private Transform _playerTransForm;

    public enum HandType
    {
        Left,
        Right
    }

    public void MoveHand(HandType handType, float duration)
    {
        var hand = handType == HandType.Left ? _leftHand : _rightHand;

        hand.transform.DOMove(_playerTransForm.localPosition, duration).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
    }

    [SerializeField]
    private float duration = 1.0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveHand(HandType.Left, duration);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveHand(HandType.Right, duration);
        }
    }
}
