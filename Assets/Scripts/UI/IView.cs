using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIPanelLayers
{
    BackgroundLayer = 0,
    DefaultLayer = 10,
    NormalLayer = 20,
    MainLayer = 30,
    MaskLayer = 40,
    PopLayer = 50,
    TipsLayer = 60,
    PromptLayer = 70,
    LoadingLayer = 80
}
public abstract class IView
{
    public UIPanelLayers UILayer = UIPanelLayers.NormalLayer;
    public void Start()
    {
        OnStart();
    }

    public void Destory()
    {
        OnDestory();
    }

    public void Show()
    {
        OnShow();
    }

    public void Hide()
    {
        OnHide();
    }

    public void Click(GameObject sender, object param)
    {
        OnClick(sender, param);
    }

    public void Press(GameObject sender, object param)
    {
        OnPress(sender, param);
    }

    public void Drag(GameObject sender, object param)
    {
        OnDrag(sender, param);
    }

    public virtual void Update() { }
    protected abstract void OnStart();
    protected abstract void OnShow();
    protected abstract void OnHide();
    protected abstract void OnDestory();
    protected abstract void OnClick(GameObject sender, object param);
    protected abstract void OnPress(GameObject sender, object param);
    protected abstract void OnDrag(GameObject sender, object param);
}

