using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings {

	public static bool SelfFire { get { return selfFire; } set { selfFire = value; } }
    static bool selfFire = true;
}
