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

    SpellStats tempSpell = null;
    SpellManager spellManager = null;

    void OnGUI()
    {
        if (spellManager == null)
        {
            spellManager = GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>();
        }

        if (tempSpell)
        {
            tempSpell.spellName = EditorGUILayout.TextField("Spell Name", tempSpell.spellName);
            tempSpell.name = tempSpell.spellName;
            tempSpell.prefab = (GameObject) EditorGUILayout.ObjectField("Spell Prefab", tempSpell.prefab, typeof(GameObject), false);          
            tempSpell.collisionParticle = (GameObject) EditorGUILayout.ObjectField("Collision Effect", tempSpell.collisionParticle, typeof(GameObject), false);
            tempSpell.icon = (Texture2D) EditorGUILayout.ObjectField("Spell Icon", tempSpell.icon, typeof(Texture2D), false);
            tempSpell.cooldown = EditorGUILayout.FloatField("Cooldown", tempSpell.cooldown);
            tempSpell.damage = EditorGUILayout.FloatField("Damage", tempSpell.damage);
            tempSpell.manaCost = EditorGUILayout.FloatField("Mana Cost", tempSpell.manaCost);
            tempSpell.maxRange = EditorGUILayout.FloatField("Max Range", tempSpell.maxRange);
            tempSpell.projectileRadius = EditorGUILayout.FloatField("Projectile Radius", tempSpell.projectileRadius);
            tempSpell.projectileSpeed = EditorGUILayout.FloatField("Projectile Speed", tempSpell.projectileSpeed);
            tempSpell.castTime = EditorGUILayout.FloatField("Cast Time", tempSpell.castTime);

        }

        EditorGUILayout.Space();

        if (tempSpell == null)
        {
            if (GUILayout.Button("Create Spell"))
                tempSpell = CreateInstance<SpellStats>();
        } else
        {
            if (GUILayout.Button("Create Scriptable Object"))
            {
                AssetDatabase.CreateAsset(tempSpell, "Assets/Resources/SpellStats/" + tempSpell.spellName + ".asset");
                AssetDatabase.SaveAssets();
                spellManager.spellList.Add(tempSpell);
                Selection.activeObject = tempSpell;
                tempSpell = null;
            }

            if (GUILayout.Button("Reset"))
                Reset();

        }
        
    }

    void Reset()
    {
        if (!tempSpell)
            return;

        tempSpell.spellName = "";
        tempSpell.prefab = null;
        tempSpell.collisionParticle = null;
        tempSpell.icon = null;
        tempSpell.damage = 0.0f;
        tempSpell.projectileSpeed = 0.0f;

    }

}
