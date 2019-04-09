using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuzzKill : Unit
{
    protected override void Start()
    {
        base.Start();
        damageResistance.Add(DamageType.Acid, 0);
    }
}
