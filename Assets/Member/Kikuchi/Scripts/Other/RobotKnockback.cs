using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using System.Linq;

public class RobotKnockback : MonoBehaviour
{
    [SerializeField]
    [Tooltip("ノックバックの傾き")]
    private float _knockbackPower;

    [SerializeField]
    [Tooltip("ノックバックの距離")]
    private float _knockbackDistance;

    [SerializeField]
    [Tooltip("ノックバックの時間")]
    private float _knockbackDuration = 1;

    [SerializeField]
    private List<GameObject> _beamTransforms;

    private Transform _robotTransform;

    private Vector3 _initPosition;
    private List<Vector3> _initBeamPositions = new List<Vector3>();
    private List<Vector3> _initBeamRotations = new List<Vector3>();
    [SerializeField]
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _robotTransform = transform;
        _initPosition = _robotTransform.position;
        foreach (var beamTransform in _beamTransforms)
        {
            _initBeamPositions.Add(beamTransform.transform.position);
            _initBeamRotations.Add(beamTransform.transform.eulerAngles);
        }
    }

    public async UniTask Knockback(float duration = 1)
    {
        _animator.speed = 0;
        var tweenTime = _knockbackDuration * duration;

        var move = _robotTransform.DOMoveZ(_initPosition.z - _knockbackDistance, tweenTime / 2)
            .OnUpdate(() =>
            {
                var indexedList = _beamTransforms.Select((x, i) => new { Index = i, Value = x }).ToList();
                foreach (var beamTransform in indexedList)
                {
                    beamTransform.Value.transform.position = _initBeamPositions[beamTransform.Index];
                }
            })
            .SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo).SetLink(_robotTransform.gameObject);
        move.Play();
        await _robotTransform.DORotate(new Vector3(_knockbackPower, 180, 0), tweenTime / 2)
            .OnUpdate(() =>
            {
                var indexedList = _beamTransforms.Select((x, i) => new { Index = i, Value = x }).ToList();
                foreach (var beamTransform in indexedList)
                {
                    beamTransform.Value.transform.eulerAngles = _initBeamRotations[beamTransform.Index];
                }
            })
            .SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo).SetLink(_robotTransform.gameObject).ToUniTask(cancellationToken: this.GetCancellationTokenOnDestroy());

        _animator.speed = 1;
    }
}