using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarMachinePlus : EnemyController
{
    protected override void InitializeStatesDictionary()
    {
        base.InitializeStatesDictionary();
        states.Add(EnemyStateTypes.LaunchCannon, new LaunchCannonState(this));
        states.Add(EnemyStateTypes.MachineGunShooting, new MachineGunShootingState(this));
    }
    
    
}
