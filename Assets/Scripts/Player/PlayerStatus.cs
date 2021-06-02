using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("Variables")]
    private float health = 100;
    private float stamina = 100;
    private float blink = 10;
    private float maxHealth = 100;
    private float maxStamina = 100;
    private float maxBlink = 10;
    private float blinkTimer = 0.33f;
    private bool blinking = false;

    [Header("Shared")]
    public float[] sharedVariables;
    public bool[] sharedBools;

    public void Update()
    {
        SanityCheck();
        sharedVariables = new float[] { health, maxHealth, stamina, maxStamina, blink, maxBlink };
        sharedBools = new bool[] { blinking };

        // Blinking
        if (blink > 0)
        {
            blink -= 1.0f * Time.deltaTime;
        }
        else
        {
            blinking = true;

            blink = 0;

            blinkTimer -= 1.0f * Time.deltaTime;

            if (blinkTimer <= 0)
            {
                blinking = false;

                blink = maxBlink;
                blinkTimer = 0.33f;
            }
        }
    }

    public void SanityCheck()
    {
        if (health >= maxHealth) { health = maxHealth; }
        if (stamina >= maxStamina) { stamina = maxStamina; }
        if (blink >= maxBlink) { blink = maxBlink; }
        if (health <= 0) { health = 0; }
        if (stamina <= 0) { stamina = 0; }
        if (blink <= 0) { blink = 0; }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void Heal(float heal)
    {
        health += heal;
    }
}
