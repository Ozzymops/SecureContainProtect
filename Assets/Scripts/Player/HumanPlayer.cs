using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HumanPlayer : BasicPlayer
{
    public override void OnStartLocalPlayer()
    {
        /* Set stats */
        dead = false;
        health = 100;
        maxHealth = 100;
        movementSpeed = 5;

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

        if (!isLocalPlayer) { return; }

        LocalUpdate();
        Movement();
    }

    private void Movement()
    {  
        characterController.Move((characterController.transform.right * movementInput.x + characterController.transform.forward * movementInput.y) * Time.deltaTime);
    }

    private void Model()
    {
        playerHead.eulerAngles = new Vector3(Mathf.Clamp(cameraRotation.x, -50, 60), cameraRotation.y, cameraRotation.z);
        transform.eulerAngles = new Vector3(0, cameraRotation.y, 0);
    }
}
