using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HumanPlayer : BasicPlayer
{
    public override void OnStartLocalPlayer()
    {
        /* Set stats */
        health = 100;
        maxHealth = 100;
        movementSpeed = 5;

        /* Set objects/scripts */
        cameraOffset = new Vector3(0, 1.0f, 0);

        /* Debugging/needs to implemented differently or later */
        mouseSensitivity = 5.0f;

        base.OnStartLocalPlayer();
    }

    private void Update()
    {
        ServerUpdate();

        if (!isLocalPlayer) { return; }

        LocalUpdate();
        Movement();
        CameraMovement();
    }

    private void Movement()
    {
        characterController.Move((characterController.transform.right * movementInput.x + characterController.transform.forward * movementInput.y) * Time.deltaTime);
    }

    private void CameraMovement()
    {
        camera.transform.eulerAngles += new Vector3(mouseMovementInput.x, mouseMovementInput.y, 0);
        camera.transform.localPosition = new Vector3(cameraOffset.x, cameraOffset.y, cameraOffset.z);
    }
}
