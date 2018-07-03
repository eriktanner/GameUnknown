using UnityEngine;

/*Base data class for all abilities, extend and implement interfaces to configure a new spell*/
public abstract class AbilityData {

    public virtual string AbilityName { get; protected set; }
    public virtual Ability Ability { get; protected set; }
    public virtual GameObject Prefab { get; protected set; }
    public virtual Texture2D Icon { get; protected set; }
    public virtual float Cost { get; protected set; }
    public virtual float Cooldown { get; protected set; }
    public virtual float MaxRange { get; protected set; }

}

