using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : GameState
{
    protected override void OnLoadComplete()
    {
    }

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
        GUIManager.HideView("");
    }
}
