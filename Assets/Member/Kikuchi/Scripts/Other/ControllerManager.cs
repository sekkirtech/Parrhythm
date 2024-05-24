using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.InputSystem;

/// <summary>
/// �V���O���g���N���X
/// </summary>
public class ControllerManager : SingletonMonoBehaviour<ControllerManager>
{
    // ��j���I�u�W�F�N�g�ɂ���
    protected override bool dontDestroyOnLoad => true;

    // �R���g���[���[�̓��͂��󂯎��
    private ControllerInput CtrlInput;

    // �R���g���[���[�̓��͂��󂯎��Subject
    private Subject<Vector2> lStickSubject = new Subject<Vector2>();
    public IObservable<Vector2> LStickObservable => lStickSubject;

    private Subject<Vector2> rStickSubject = new Subject<Vector2>();
    public IObservable<Vector2> RStickObservable => rStickSubject;

    private Subject<Vector2> dPadSubject = new Subject<Vector2>();
    public IObservable<Vector2> DPadObservable => dPadSubject;

    private Subject<Unit> eastButtonSubject = new Subject<Unit>();
    public IObservable<Unit> EastButtonObservable => eastButtonSubject;

    private Subject<Unit> southButtonSubject = new Subject<Unit>();
    public IObservable<Unit> SouthButtonObservable => southButtonSubject;

    private Subject<Unit> westButtonSubject = new Subject<Unit>();
    public IObservable<Unit> WestButtonObservable => westButtonSubject;

    private Subject<Unit> northButtonSubject = new Subject<Unit>();
    public IObservable<Unit> NorthButtonObservable => northButtonSubject;

    private Subject<Unit> l1ButtonSubject = new Subject<Unit>();
    public IObservable<Unit> L1ButtonObservable => l1ButtonSubject;

    private Subject<Unit> l2ButtonSubject = new Subject<Unit>();
    public IObservable<Unit> L2ButtonObservable => l2ButtonSubject;

    private Subject<Unit> r1ButtonSubject = new Subject<Unit>();
    public IObservable<Unit> R1ButtonObservable => r1ButtonSubject;

    private Subject<Unit> r2ButtonSubject = new Subject<Unit>();
    public IObservable<Unit> R2ButtonObservable => r2ButtonSubject;

    private Subject<Unit> startButtonSubject = new Subject<Unit>();
    public IObservable<Unit> StartButtonObservable => startButtonSubject;

    private Subject<Unit> selectButtonSubject = new Subject<Unit>();
    public IObservable<Unit> SelectButtonObservable => selectButtonSubject;

    // �{�^���̗����ꂽ�C�x���g
    private Subject<Unit> l2ButtonUpSubject = new Subject<Unit>();
    public IObservable<Unit> L2ButtonUpObservable => l2ButtonUpSubject;

    private Subject<Unit> r2ButtonUpSubject = new Subject<Unit>();
    public IObservable<Unit> R2ButtonUpObservable => r2ButtonUpSubject;

    private Subject<Unit> westButtonUpSubject = new Subject<Unit>();
    public IObservable<Unit> WestButtonUpObservable => westButtonUpSubject;

    // �f�b�h�]�[����臒l
    public float deadZone = 0.1f;

    // �O��̓��͏�Ԃ�ێ�����t�B�[���h
    private Vector2 previousLStickInput = Vector2.zero;
    private Vector2 previousRStickInput = Vector2.zero;
    private Vector2 previousDPadInput = Vector2.zero;

    // �f�b�h�]�[���𒴂�������ێ�����t���O
    private bool lStickActive = false;
    private bool rStickActive = false;
    private bool dPadActive = false;

    /// <summary>
    /// �R���g���[���[�̓��͎���L���ɂ���
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        CtrlInput = new ControllerInput();
        CtrlInput.Enable();
    }

    private void Start()
    {
        SetControllerSubject();

        DPadObservable.Subscribe(x => Debug.Log("DPad: " + x));
        LStickObservable.Subscribe(x => Debug.Log("LStick: " + x));
        RStickObservable.Subscribe(x => Debug.Log("RStick: " + x));
    }

    /// <summary>
    /// �R���g���[���[�̓��͂��󂯎��Subject��ݒ肷��
    /// </summary>
    private void SetControllerSubject()
    {
        CtrlInput.Controller.LStick.performed += ctx => CheckDeadZone(ctx.ReadValue<Vector2>(), lStickSubject);
        CtrlInput.Controller.RStick.performed += ctx => CheckDeadZone(ctx.ReadValue<Vector2>(), rStickSubject);
        CtrlInput.Controller.DPad.performed += ctx => CheckDeadZone(ctx.ReadValue<Vector2>(), dPadSubject);

        CtrlInput.Controller.East.performed += ctx => eastButtonSubject.OnNext(Unit.Default);
        CtrlInput.Controller.South.performed += ctx => southButtonSubject.OnNext(Unit.Default);
        CtrlInput.Controller.West.performed += ctx => westButtonSubject.OnNext(Unit.Default);
        CtrlInput.Controller.North.performed += ctx => northButtonSubject.OnNext(Unit.Default);

        CtrlInput.Controller.L2.canceled += ctx => l2ButtonUpSubject.OnNext(Unit.Default);
        CtrlInput.Controller.R2.canceled += ctx => r2ButtonUpSubject.OnNext(Unit.Default);
        CtrlInput.Controller.West.canceled += ctx => westButtonUpSubject.OnNext(Unit.Default);

        CtrlInput.Controller.L1.performed += ctx => l1ButtonSubject.OnNext(Unit.Default);
        CtrlInput.Controller.L2.performed += ctx => l2ButtonSubject.OnNext(Unit.Default);
        CtrlInput.Controller.R1.performed += ctx => r1ButtonSubject.OnNext(Unit.Default);
        CtrlInput.Controller.R2.performed += ctx => r2ButtonSubject.OnNext(Unit.Default);

        CtrlInput.Controller.Start.performed += ctx => startButtonSubject.OnNext(Unit.Default);
        CtrlInput.Controller.Select.performed += ctx => selectButtonSubject.OnNext(Unit.Default);
    }

    /// <summary>
    /// ���͂̃f�b�h�]�[�����`�F�b�N���A���͂��L���Ȃ�Subject�ɒl��ʒm���܂��B
    /// </summary>
    /// <param name="vec2">���̓x�N�g���B</param>
    /// <param name="sub">�ʒm����Subject�B</param>
    private void CheckDeadZone(Vector2 vec2, Subject<Vector2> sub)
    {
        if (vec2.magnitude >= 0.5f)
        {
            sub.OnNext(vec2);
        }
    }
}
