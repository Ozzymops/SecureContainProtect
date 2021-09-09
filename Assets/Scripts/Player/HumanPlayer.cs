using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HumanPlayer : BasicPlayer
{
    #region HumanPlayer unique variables
    private float stamina;
    private float maxStamina;
    private float staminaDrainMultiplier;

    private bool sprinting;
    private float sprintSpeedMultiplier;
    private bool sneaking;
    private float sneakSpeedMultiplier;

    private float blinkTimer;
    private float maxBlinkTimer;
    private float blinkClosedTimer;
    private float blinkTimerDrainMultiplier;

    public float visibleStamina;
    public float visibleMaxStamina;
    public float visibleBlinkTimer;
    public float visibleMaxBlinkTimer;
    #endregion

    #region Equipped items
    private bool equippedGasMask;

    public bool visibleEquippedGasMask;
    #endregion

    public override void OnStartLocalPlayer()
    {
        /* Set stats */
        dead = false;

        health = 100;
        maxHealth = 100;
        stamina = 100;
        maxStamina = 100;
        blinkTimer = 10;
        maxBlinkTimer = 10;

        baseMovementSpeed = 5;
        sprintSpeedMultiplier = 2.0f;
        sneakSpeedMultiplier = 0.66f;

        staminaDrainMultiplier = 1.0f;
        blinkTimerDrainMultiplier = 1.0f;

        /* Equipped items */
        equippedGasMask = false;

        /* Set objects/scripts */
        cameraOffset = new Vector3(0, 1.0f, 0);
        firstPersonModelOffset = new Vector3(1.0f, -0.6f, 0.66f);

        /* Debugging/needs to implemented differently or later */
        mouseSensitivity = 5.0f;
        cameraFOV = 90.0f;

        base.OnStartLocalPlayer();
    }

    private void Update()
    {
        ServerUpdate();
        Model();
        Health();
        Stamina();
        Blink();
        GasMask();

        if (!isLocalPlayer) { return; }

        LocalUpdate();
        Movement();
        sprinting = SprintInput();
        sneaking = SneakInput();

        DebugInput();
    }

    private void Movement()
    {  
        if (sneaking && sprinting)
        {
            modifiedMovementSpeed = baseMovementSpeed;
        }
        else if (sprinting)
        {
            modifiedMovementSpeed = baseMovementSpeed * sprintSpeedMultiplier;
        }
        else if (sneaking)
        {
            modifiedMovementSpeed = baseMovementSpeed * sneakSpeedMultiplier;
        }
        else
        {
            modifiedMovementSpeed = baseMovementSpeed;
        }

        characterController.Move((characterController.transform.right * movementInput.x + characterController.transform.forward * movementInput.y) * Time.deltaTime);
    }

    private void Model()
    {
        playerHead.eulerAngles = new Vector3(Mathf.Clamp(cameraRotation.x, -50, 60), cameraRotation.y, cameraRotation.z);
        transform.eulerAngles = new Vector3(0, cameraRotation.y, 0);
    }

    private void Stamina()
    {
        stamina = Mathf.Clamp(stamina, 0, maxStamina);

        visibleStamina = stamina;
        visibleMaxStamina = maxStamina;

        if (sprinting)
        {
            stamina -= (5.0f * staminaDrainMultiplier) * Time.deltaTime;
        }
        else
        {
            stamina += (3.5f / staminaDrainMultiplier) * Time.deltaTime;
        }
    }

    private void Blink()
    {
        blinkTimer = Mathf.Clamp(blinkTimer, 0, maxBlinkTimer);

        visibleBlinkTimer = blinkTimer;
        visibleMaxBlinkTimer = maxBlinkTimer;

        if (BlinkInput())
        {
            blinkTimer = 0.0f;
        }

        if (blinkTimer > 0.0f)
        {
            blinkTimer -= (1.0f * blinkTimerDrainMultiplier) * Time.deltaTime;
            blinkClosedTimer = 0.33f;
        }
        else
        {
            if (blinkClosedTimer > 0.0f)
            {
                blinkTimer = 0.0f;
                blinkClosedTimer -= 1.0f * Time.deltaTime;
            }
            else
            {
                blinkTimer = maxBlinkTimer;
            }
        }
    }

    private void GasMask()
    {
        visibleEquippedGasMask = equippedGasMask;
    }

    private bool SprintInput()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }

    private bool SneakInput()
    {
        return Input.GetKey(KeyCode.C);
    }

    private bool BlinkInput()
    {
        return Input.GetKey(KeyCode.Q);
    }

    private void DebugInput()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            equippedGasMask = !equippedGasMask;
        }
    }
}
