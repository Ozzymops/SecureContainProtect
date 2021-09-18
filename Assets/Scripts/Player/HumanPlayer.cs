using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HumanPlayer : NetworkBehaviour
{
    private HumanPlayerState state = new HumanPlayerIdleState();
    private HumanPlayerInventoryState subState = new HumanPlayerNothingInventoryState();

    private CharacterController characterController;

    private float maxHealth = 100.0f;
    private float maxStamina = 100.0f;
    private float maxBlinkTimer = 10.0f;
    private float health;
    private float stamina;
    private float blinkTimer;

    private Vector2 movement;
    private float movementSpeed = 3.0f;

    private GameObject[] inventory;
    private GameObject inventoryHead;
    private GameObject inventoryHand;

    public override void OnStartLocalPlayer()
    {
        characterController = GetComponent<CharacterController>();

        health = maxHealth;
        stamina = maxStamina;
        blinkTimer = maxBlinkTimer;

        base.OnStartLocalPlayer();
    }

    private void Update()
    {
        state = state.handleInput(this);
        subState = subState.handleInput(this);

        if (stamina < maxStamina)
        {
            stamina += 5.0f * Time.deltaTime;
        }
    }

    public float[] GetHealth()
    {
        return new float[] { health, maxHealth };
    }

    public void ModifyHealth(float modification)
    {
        health += modification;
    }

    public float[] GetStamina()
    {
        return new float[] { stamina, maxStamina};
    }

    public void ModifyStamina(float modification)
    {
        stamina += modification;
    }

    public float[] GetBlink()
    {
        return new float[] { blinkTimer, maxBlinkTimer };
    }

    public Vector2 GetMovement()
    {
        return movement;
    }

    public void DoMovement(Vector2 newMovement)
    {
        movement = newMovement;
        characterController.Move((characterController.transform.right * movement.x * movementSpeed + characterController.transform.forward * movement.y * movementSpeed) * Time.deltaTime);
    }

    public bool GetGasMask()
    {
        if (subState is HumanPlayerGasMaskInventoryState)
        {
            return true;
        }

        return false;
    }
}
