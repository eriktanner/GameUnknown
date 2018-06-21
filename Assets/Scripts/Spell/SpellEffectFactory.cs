using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

public static class SpellEffectFactory {

    private static Dictionary<System.Type, SpellEffect> typeToEffect;

    public static SpellEffect GetEffectFromFactory(System.Type spellType)
    {
        if (typeToEffect == null)
            RegisterEffects();
        

        if (typeToEffect.ContainsKey(spellType))
        {
            SpellEffect foundEffect = typeToEffect[spellType];
            return foundEffect;
        }
        return null;
    }

    private static void RegisterEffects()
    {
        var EffectTypes = Assembly.GetAssembly(typeof(SpellEffect)).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(SpellEffect)));

        typeToEffect = new Dictionary<Type, SpellEffect>();

        foreach(var type in EffectTypes)
        {
            var tempEffect = Activator.CreateInstance(type) as SpellEffect;

            if (!typeToEffect.ContainsKey(tempEffect.SpellType))
                typeToEffect.Add(tempEffect.SpellType, tempEffect);
        }

    }
}
