using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Health target;
    [SerializeField] int damage = 2;
    void Start()
    {
        target = GetComponent<PlayerController>().ReturnNextPlayer().GetComponent<Health>();
    }

    public void AttackHitEvent()
    {
        if (target == null) return;

        target.TakeDamage(damage);
        print("Attacking " + target);
    }
}
