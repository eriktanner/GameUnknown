using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour {


    public List<Spell> spellList = new List<Spell>();


    public Spell getSpellFromName(string spellName)
    {
        return (Spell) Resources.Load("Spells/" + spellName, typeof(Spell));
    }

    public GameObject createSpellInWorld(Spell spell, Vector3 position, Quaternion rotation)
    {
        GameObject spellObject = Instantiate(spell.prefab, position, rotation);
        spellObject.name = spell.name;
        spellObject.layer = 10;
        Rigidbody rigidBody = spellObject.AddComponent<Rigidbody>();
        rigidBody.useGravity = false;
        rigidBody.velocity = spellObject.transform.forward * spell.projectileSpeed;
        rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        SphereCollider sphereCollider = spellObject.AddComponent<SphereCollider>();
        sphereCollider.radius = spell.projectileRadius;
        spellObject.AddComponent<SpellCollision>();
        spellObject.AddComponent<SpellNetworkInfo>().spellShotBy = spell.shotBy;

        spellObject.transform.parent = GameObject.Find("Managers/SpellManager").transform;
        return spellObject;
    }

    public void DestroySpell(GameObject spellObject, float waitTime)
    {
        if (spellObject.name.Equals("Pain"))
            StartCoroutine(destroyPain(spellObject, waitTime));
        else
            Destroy(spellObject, waitTime);
    }


    /*We want to emit pain collision particle even if it doesnt hit so that player can see range of spell*/
    IEnumerator destroyPain(GameObject spellObject, float waitTime)
    {
        Spell spell = getSpellFromName(spellObject.name);
        yield return new WaitForSeconds(waitTime);

        if (spell != null && spellObject != null)
        {
            GameObject collisionParticles = Instantiate(spell.collisionParticle, spellObject.transform.position, Quaternion.identity);
            Destroy(collisionParticles, 1.0f);
            Destroy(spellObject);
        }
    }
 

    
}
