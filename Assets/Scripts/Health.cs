using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public int maxHp;
    [SerializeField] public int currentHp;
    [SerializeField] public float deathTimer;
    private Coroutine deathRoutine;

    void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHp -= damageAmount;

        if (currentHp <= 0 && deathRoutine == null)
            deathRoutine = StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        var timer = 0f;

        while (timer < deathTimer)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
        
    }
}
