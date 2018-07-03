using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpellCreator : EditorWindow {

	[MenuItem("Spell Maker/Spell Creator")]

    static void Init()
    {
        SpellCreator spellWindow = (SpellCreator)CreateInstance(typeof(SpellCreator));
        spellWindow.Show();
    }

    AbilityData tempSpell = null;
    SpellManager spellManager = null;

    void OnGUI()
    {
        if (spellManager == null)
        {
            spellManager = GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>();
        }

        if (tempSpell != null)
        {
            /*
            tempSpell.abilityName = EditorGUILayout.TextField("Spell Name", tempSpell.AbilityName);
            tempSpell.name = tempSpell.AbilityName;
            tempSpell.prefab = (GameObject) EditorGUILayout.ObjectField("Spell Prefab", tempSpell.Prefab, typeof(GameObject), false);          
            tempSpell.collisionParticle = (GameObject) EditorGUILayout.ObjectField("Collision Effect", tempSpell.CollisionParticle, typeof(GameObject), false);
            tempSpell.icon = (Texture2D) EditorGUILayout.ObjectField("Spell Icon", tempSpell.Icon, typeof(Texture2D), false);
            tempSpell.cooldown = EditorGUILayout.FloatField("Cooldown", tempSpell.Cooldown);
            tempSpell.damage = EditorGUILayout.FloatField("Damage", tempSpell.Damage);
            tempSpell.manaCost = EditorGUILayout.FloatField("Mana Cost", tempSpell.ManaCost);
            tempSpell.maxRange = EditorGUILayout.FloatField("Max Range", tempSpell.MaxRange);
            tempSpell.projectileRadius = EditorGUILayout.FloatField("Projectile Radius", tempSpell.ProjectileRadius);
            tempSpell.projectileSpeed = EditorGUILayout.FloatField("Projectile Speed", tempSpell.ProjectileSpeed);
            tempSpell.castTime = EditorGUILayout.FloatField("Cast Time", tempSpell.CastTime);
            */
        }

        EditorGUILayout.Space();

        if (tempSpell == null)
        {
            //if (GUILayout.Button("Create Spell"))
                //tempSpell = CreateInstance<AbilityData>();
        } else
        {
            if (GUILayout.Button("Create Scriptable Object"))
            {
                //AssetDatabase.CreateAsset(tempSpell, "Assets/Resources/SpellStats/" + tempSpell.AbilityName + ".asset");
                AssetDatabase.SaveAssets();
                spellManager.spellList.Add(tempSpell);
                //Selection.activeObject = tempSpell;
                tempSpell = null;
            }

            if (GUILayout.Button("Reset"))
                Reset();

        }
        
    }

    void Reset()
    {
        //if (!tempSpell)
        //    return;
        /*
        tempSpell.abilityName = "";
        tempSpell.prefab = null;
        tempSpell.collisionParticle = null;
        tempSpell.icon = null;
        tempSpell.damage = 0.0f;
        tempSpell.projectileSpeed = 0.0f;
        */
    }

}
