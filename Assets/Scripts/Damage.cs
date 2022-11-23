using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] int attackDamage;
    [SerializeField] CharacterController2D characterController;
    [SerializeField] StunController stunController;

    //Knockback Settings
    public bool KnockBackFromRight;
    public float kbForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Damage Application
        if (collision.gameObject.TryGetComponent<Health>(out Health healthComponent))
        {
            healthComponent.TakeDamage(attackDamage);  
        }

        //Knockback
        if (collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            if (rb.tag == "Enemy")
            {
                if (characterController.m_FacingRight == true)
                {
                    rb.AddForce(transform.right * kbForce, ForceMode2D.Impulse);
                    rb.AddForce(transform.up * kbForce, ForceMode2D.Impulse);
                }

                if (characterController.m_FacingRight == false)
                {
                    rb.AddForce(-transform.right * kbForce, ForceMode2D.Impulse);
                    rb.AddForce(transform.up * kbForce, ForceMode2D.Impulse);
                }
            }
            
            if (rb.tag == "Player")
            {
                
                if (characterController.m_FacingRight == true)
                {
                    rb.AddForce(-transform.right * kbForce, ForceMode2D.Impulse);
                    stunController.InitiateStun();
                }

                if (characterController.m_FacingRight == false)
                {
                    rb.AddForce(transform.right * kbForce, ForceMode2D.Impulse);
                    stunController.InitiateStun();
                }

            }       
        }
    }  
}
