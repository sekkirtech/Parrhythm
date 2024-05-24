using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

//シングルトンクラス
public class ControllerManager : SingletonMonoBehaviour<ControllerManager>
{
    //非破棄オブジェクトにする
    protected override bool dontDestroyOnLoad => true;

    //コントローラーの入力を受け取る
    private @ControllerInput CtrlInput;

    //コントローラーの入力を受け取るSubject
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

    //関追加0524
    private Subject<Unit> l2ButtonUpSubject = new Subject<Unit>();
    public IObservable<Unit> L2ButtonUpObservable=> l2ButtonUpSubject;

    private Subject<Unit> r2ButtonUpSubject = new Subject<Unit>();
    public IObservable<Unit> R2ButtonUpObservable => r2ButtonUpSubject;

    private Subject<Unit> westButtonUpSubject = new Subject<Unit>();
    public IObservable<Unit> WestButtonUpObservable => westButtonUpSubject;


    protected override void Awake()
    {
        base.Awake();

        //コントローラーの入力受取を有効にする
        CtrlInput = new ControllerInput();
        CtrlInput.Enable();
    }

    private void Start()
    {
        SetControllerSubject();
    }

    /// <summary>
    /// コントローラーの入力を受け取るSubjectを設定する
    /// </summary>
    private void SetControllerSubject()
    {
        CtrlInput.Controller.LStick.performed += ctx => lStickSubject.OnNext(ctx.ReadValue<Vector2>());

        CtrlInput.Controller.RStick.performed += ctx => rStickSubject.OnNext(ctx.ReadValue<Vector2>());
        CtrlInput.Controller.DPad.performed += ctx => dPadSubject.OnNext(ctx.ReadValue<Vector2>());

        CtrlInput.Controller.East.performed += ctx => eastButtonSubject.OnNext(Unit.Default);
        CtrlInput.Controller.South.performed += ctx => southButtonSubject.OnNext(Unit.Default);
        CtrlInput.Controller.West.performed += ctx => westButtonSubject.OnNext(Unit.Default);
        CtrlInput.Controller.North.performed += ctx => northButtonSubject.OnNext(Unit.Default);

        //関追加0524
        CtrlInput.Controller.L2.canceled += ctx => l2ButtonUpSubject.OnNext(Unit.Default);
        CtrlInput.Controller.R2.canceled += ctx => r2ButtonUpSubject.OnNext(Unit.Default);
        CtrlInput.Controller.West.canceled += ctx => westButtonUpSubject.OnNext(Unit.Default);
        CtrlInput.Controller.West.canceled += ctx => westButtonUpSubject.OnNext(Unit.Default);

        CtrlInput.Controller.L1.performed += ctx => l1ButtonSubject.OnNext(Unit.Default);
        CtrlInput.Controller.L2.performed += ctx => l2ButtonSubject.OnNext(Unit.Default);
        CtrlInput.Controller.R1.performed += ctx => r1ButtonSubject.OnNext(Unit.Default);
        CtrlInput.Controller.R2.performed += ctx => r2ButtonSubject.OnNext(Unit.Default);

        CtrlInput.Controller.Start.performed += ctx => startButtonSubject.OnNext(Unit.Default);
        CtrlInput.Controller.Select.performed += ctx => selectButtonSubject.OnNext(Unit.Default);
    }    


}
