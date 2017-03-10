using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanel : IView
{
    protected override void OnStart()
    {
        //Debug.LogError("LoginPanel onstart");
    }

    protected override void OnShow()
    {
    }

    protected override void OnHide()
    {
    }

    public override void Update()
    {
        
    }

    protected override void OnDestory()
    {
    }

    protected override void OnClick(GameObject sender, object param)
    {
        if (sender.gameObject.name.Equals("LoginBtn"))
        {
            GameStateManager.LoadScene(2);
        }
    }


    protected override void OnDrag(GameObject sender, object param)
    {
    }

    protected override void OnPress(GameObject sender, object param)
    {
    }
}
