﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonLily : Unit
{
    protected override void Start()
    {
        base.Start();
        damageResistance.Add(DamageType.Fire, 1);
    }
}
