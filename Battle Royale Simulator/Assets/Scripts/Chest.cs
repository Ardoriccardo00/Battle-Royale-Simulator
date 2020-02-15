using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Weapon weapon;

    public Weapon GiveWeapon()
    {
        return weapon;
    }
}
