using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunController : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Animator playerAnimator;

    // Stun duration 
    [SerializeField] private float hitStunDuration;
    private float hitStunTime = 0f;

    public void InitiateStun()
    {
        hitStunTime = Time.time;
    }

    public bool Stun()
    {
        if (Time.time - hitStunTime >= hitStunDuration && hitStunTime != 0)
        {
            return false;
        }
        else if (hitStunTime != 0) {
            return true;
        }
        else 
        {
            return false;
        }
    }
}
