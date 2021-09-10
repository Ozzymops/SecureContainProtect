using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BasicSoundManager : NetworkBehaviour
{
    [SerializeField] protected AudioSource footstepSource;
    [SerializeField] protected AudioClip[] footstepsWalkGeneric;
    [SerializeField] protected AudioClip[] footstepsWalkMetal;
    [SerializeField] protected AudioClip[] footstepsWalkForest;
    [SerializeField] protected AudioClip[] footstepsWalkCorrode;
    [SerializeField] protected AudioClip[] footstepsRunGeneric;
    [SerializeField] protected AudioClip[] footstepsRunMetal;

    protected enum FootstepType { Generic, Metal, Forest, Corrode };
    protected FootstepType footstepType;
    protected bool footstepRunning;
    protected float footstepTimer;

    private void Update()
    {
        
    }

    public void PlayFootstep()
    {
        if (footstepRunning)
        {
            switch (footstepType)
            {
                case FootstepType.Generic:
                    footstepSource.clip = footstepsRunGeneric[Random.Range(0, footstepsRunGeneric.Length)];
                    break;

                case FootstepType.Metal:
                    footstepSource.clip = footstepsRunMetal[Random.Range(0, footstepsRunMetal.Length)];
                    break;

                case FootstepType.Forest:
                    footstepSource.clip = footstepsWalkForest[Random.Range(0, footstepsWalkForest.Length)];
                    break;

                case FootstepType.Corrode:
                    footstepSource.clip = footstepsWalkCorrode[Random.Range(0, footstepsWalkCorrode.Length)];
                    break;

                default:
                    break;
            }
        }
        else
        {
            switch (footstepType)
            {
                case FootstepType.Generic:
                    footstepSource.clip = footstepsWalkGeneric[Random.Range(0, footstepsWalkGeneric.Length)];
                    break;

                case FootstepType.Metal:
                    footstepSource.clip = footstepsWalkMetal[Random.Range(0, footstepsWalkMetal.Length)];
                    break;

                case FootstepType.Forest:
                    footstepSource.clip = footstepsWalkForest[Random.Range(0, footstepsWalkForest.Length)];
                    break;

                case FootstepType.Corrode:
                    footstepSource.clip = footstepsWalkCorrode[Random.Range(0, footstepsWalkCorrode.Length)];
                    break;

                default:
                    break;
            }
        }

        footstepSource.Play();
    }

    public void SetFootstepType(int type)
    {
        switch (type)
        {
            case 0:
                footstepType = FootstepType.Generic;
                break;

            case 1:
                footstepType = FootstepType.Metal;
                break;

            case 2:
                footstepType = FootstepType.Forest;
                break;

            case 3:
                footstepType = FootstepType.Corrode;
                break;

            default:
                footstepType = FootstepType.Generic;
                break;
        }
    }

    public void IsRunning(bool running)
    {
        footstepRunning = running;
    }
}
