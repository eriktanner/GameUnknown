using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTHitCheck {


    protected virtual AbilityData AbilityData { get; set; }

    protected virtual TaskManager.TaskState OverTimeTask { get; set; }


    public virtual void CheckHit(GameObject caster, RaycastHit hit)
    {

    }


}
