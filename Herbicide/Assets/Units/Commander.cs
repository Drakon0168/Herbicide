using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CommanderDeath();

public class Commander : Unit
{
    public CommanderDeath commanderDeath;

    void Start()
    {
    }
    
    void Update()
    {
        
    }

    public override void Die()
    {
        if(commanderDeath != null)
        {
            commanderDeath();
        }
        
        base.Die();
    }
}
