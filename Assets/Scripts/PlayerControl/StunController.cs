using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunController : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Animator playerAnimator;

    // Stun duration 
    [SerializeField] private float hitStunDuration;
    private float hitStunTime;

    public bool stunStatus = false;

    public void InitiateStun()
    {
        hitStunTime = Time.time;
    }

    public bool Stun()
    {
        if (Time.time - hitStunTime >= hitStunDuration)
        {
            return false;
        }
        return true;
    }
}
