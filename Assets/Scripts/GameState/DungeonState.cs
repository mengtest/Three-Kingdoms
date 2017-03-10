using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonState : GameState {
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        GUIManager.HideView("DungeonPanel");
    }

    protected override void OnLoadComplete()
    {
        GUIManager.ShowView("DungeonPanel");
    }
}
