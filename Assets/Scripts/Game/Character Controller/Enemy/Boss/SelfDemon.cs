using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDemon : EnemyController
{
    protected override void InitializeStatesDictionary()
    {
        base.InitializeStatesDictionary();
        states.Add(EnemyStateTypes.CopyPlayer, new CopyPlayerState(this));
    }
    
    
}
