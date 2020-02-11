using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 10;
    [SerializeField] Material standardMaterial;
    [SerializeField] Material damagedMaterial;
    [SerializeField] MeshRenderer visorRenderer;
    int currentHealth;
    bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public int ReturnHealth()
    {
        return currentHealth;
    }

    public bool TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        StartCoroutine(ChangeDamageMaterial());
        if (currentHealth <= 0) return true;
        else return false;
    }

    IEnumerator ChangeDamageMaterial()
    {
        visorRenderer.material = damagedMaterial;
        yield return new WaitForSeconds(0.2f);
        visorRenderer.material = standardMaterial;
    }

    void Die()
    {
        isDead = true;
        MatchManager.Instance.RemovePlayer(GetComponent<PlayerIdentity>());
        Destroy(gameObject);
    }
}
