using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BasicPlayer : NetworkBehaviour
{
    protected Vector2 movementInput;
    protected Vector2 mouseMovementInput;
    protected Vector3 cameraOffset;

    // From config
    protected float mouseSensitivity;

    [SerializeField] protected float health;
    protected float maxHealth;
    protected float movementSpeed;

    public float visibleHealth;
    public float visibleMaxHealth;

    [SerializeField] protected Camera camera;
    [SerializeField] protected CharacterController characterController;

    public override void OnStartLocalPlayer()
    {
        /* Get references */
        camera = Camera.main;
        characterController = GetComponent<CharacterController>();

        base.OnStartLocalPlayer();
    }

    protected void LocalUpdate()
    {
        movementInput = MovementInput();
        mouseMovementInput = MouseMovementInput();
    }

    protected void ServerUpdate()
    {
        visibleHealth = health;
        visibleMaxHealth = maxHealth;
    }

    private Vector2 MovementInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal") * movementSpeed, Input.GetAxisRaw("Vertical") * movementSpeed);
    }

    private Vector2 MouseMovementInput()
    {
        return new Vector2(Input.GetAxisRaw("Mouse X") * mouseSensitivity, Input.GetAxisRaw("Mouse Y") * mouseSensitivity);
    }
}
