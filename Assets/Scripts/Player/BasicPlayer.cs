using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BasicPlayer : NetworkBehaviour
{
    protected Vector2 movementInput;
    protected Vector2 mouseMovementInput;
    protected Vector3 cameraOffset;
    protected Vector3 cameraRotation;
    [SerializeField] protected Vector3 firstPersonModelOffset;

    // From config
    protected float mouseSensitivity;
    protected float cameraFOV;

    protected bool dead;
    protected float health;
    protected float maxHealth;
    protected float baseMovementSpeed;
    protected float modifiedMovementSpeed;

    public bool visibleDead;
    public float visibleHealth;
    public float visibleMaxHealth;

    protected Camera camera;
    protected CharacterController characterController;
    [SerializeField] protected Transform playerModelContainer;
    protected Transform[] playerModel;
    protected Transform playerHead;
    [SerializeField] protected Transform firstPersonModelContainer;
    protected Transform[] firstPersonModel;

    public override void OnStartLocalPlayer()
    {
        /* Get references */
        camera = Camera.main;
        characterController = GetComponent<CharacterController>();

        /* Configure */
        camera.transform.SetParent(transform);

        if (playerModelContainer != null)
        {
            playerModel = playerModelContainer.GetComponentsInChildren<Transform>();

            foreach (Transform playerModelPart in playerModel)
            {
                if (playerModelPart.name.Contains("Head"))
                {
                    playerHead = playerModelPart;
                }

                if (!playerModelPart.name.Contains("Model"))
                {
                    playerModelPart.GetComponent<MeshRenderer>().enabled = false;
                }                
            }
        }
        else
        {
            Debug.Log("Assign player model container!");
        }

        firstPersonModelContainer.SetParent(camera.transform);

        base.OnStartLocalPlayer();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        /* Only execute on other clients */
        if (isLocalPlayer) { return; }

        firstPersonModel = firstPersonModelContainer.GetComponentsInChildren<Transform>();

        foreach (Transform playerModelPart in firstPersonModel)
        {
            if (!playerModelPart.name.Contains("FirstPersonModel"))
            {
                playerModelPart.GetComponent<MeshRenderer>().enabled = false;
            }          
        }             
    }

    protected void LocalUpdate()
    {
        movementInput = MovementInput();
        mouseMovementInput = MouseMovementInput();
        firstPersonModelContainer.localPosition = firstPersonModelOffset;

        // From config
        camera.fieldOfView = cameraFOV;

        CameraMovement();
    }

    protected void ServerUpdate()
    {
        visibleDead = dead;
        visibleHealth = health;
        visibleMaxHealth = maxHealth;
    }

    protected void Health()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        if (health <= 0)
        {
            dead = true;
        }
        else
        {
            dead = false;
        }
    }

    private Vector2 MovementInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal") * modifiedMovementSpeed, Input.GetAxisRaw("Vertical") * modifiedMovementSpeed);
    }

    private Vector2 MouseMovementInput()
    {
        return new Vector2(Input.GetAxisRaw("Mouse X") * mouseSensitivity, Input.GetAxisRaw("Mouse Y") * mouseSensitivity);
    }

    private void CameraMovement()
    {
        cameraRotation.x -= mouseMovementInput.y;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -90, 90);
        cameraRotation.y += mouseMovementInput.x;

        camera.transform.eulerAngles = new Vector3(cameraRotation.x, cameraRotation.y, 0);
        camera.transform.localPosition = cameraOffset;
    }
}
