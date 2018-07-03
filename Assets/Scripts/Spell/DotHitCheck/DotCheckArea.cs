using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotCheckArea : DOTHitCheck {


    public DotCheckArea(AbilityData abilityData)
    {
        AbilityData = abilityData;
    }



    public override void CheckHit(GameObject caster, RaycastHit hit)
    {

        OverTimeTask = TaskManager.CreateTask(TickArea(caster, hit.point));
        
    }

    IEnumerator TickArea(GameObject caster, Vector3 hitpoint)
    {
        ITick iTick = AbilityData as ITick;
        if (iTick == null)
            OverTimeTask.Stop();

        for (int i = 0; i < iTick.NumTicks; i++)
        {
            yield return new WaitForSeconds(iTick.TimeBetweenTicks);

            List<GameObject> playersInRadiusAndLOS = AbilityUtility.FindPlayersWithinRadiusAndLOS(hitpoint, ((IAOE)AbilityData).Radius);

            foreach (GameObject hitPlayer in playersInRadiusAndLOS)
            {
                InterfaceToEffects.ProcessOverTimeEffects(caster, hitPlayer, AbilityData);
            }

        }
    }
}
