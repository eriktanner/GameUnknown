using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SpellEffect {

    public virtual System.Type SpellType { get; protected set; }

    public virtual void SetupEffect(GameObject playerHit, GameObject particles)
    {

    }

    public virtual void ProcessEffect(GameObject playerHit, GameObject particles) {

    }
	
}
