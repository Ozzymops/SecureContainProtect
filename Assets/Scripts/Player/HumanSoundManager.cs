using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HumanSoundManager : BasicSoundManager
{
    /* References */
    private HumanPlayer humanPlayer;

    private void Start()
    {
        footstepTimer = 1.0f;
        humanPlayer = GetComponent<HumanPlayer>();
    }

    private void Update()
    {
        RaycastFootstepType();
        Footsteps();
        IsRunning(humanPlayer.visibleSprinting);
    }

    private void Footsteps()
    {
        if (humanPlayer.visibleMoving)
        {
            if (humanPlayer.visibleSprinting && humanPlayer.visibleSneaking)
            {
                footstepTimer -= 2f * Time.deltaTime;
            }
            else if (humanPlayer.visibleSprinting)
            {
                footstepTimer -= 3.5f * Time.deltaTime;
            }
            else if (humanPlayer.visibleSneaking)
            {
                footstepTimer -= 1.5f * Time.deltaTime;
            }
            else
            {
                footstepTimer -= 2f * Time.deltaTime;
            }
            
            if (footstepTimer <= 0.0f)
            {
                footstepTimer = 1.0f;
                PlayFootstep();
            }         
        }
    }

    private void RaycastFootstepType()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            switch (hit.transform.gameObject.tag)
            {
                case "FloorMetal":
                    SetFootstepType(1);
                    break;

                case "FloorForest":
                    SetFootstepType(2);
                    break;

                case "FloorCorrode":
                    SetFootstepType(3);
                    break;

                default:
                    SetFootstepType(0);
                    break;
            }
        }
    }
}
