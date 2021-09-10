using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MusicManager : NetworkBehaviour
{
    [SerializeField] private AudioSource mainSource;
    [SerializeField] private AudioSource fadeSource;
    [SerializeField] private AudioSource horrorSource;
    [SerializeField] private AudioClip[] musicList;
    [SerializeField] private AudioClip[] horrorList;
    private float maxVolume;

    /* Chase music */
    private enum ChaseMusicState { Intro, Loop, LoopNearby, LoopDeath, Outro, Done };
    private ChaseMusicState chaseMusicState;
    private ChaseMusicState previousChaseMusicState;
    private int[] chaseMusicSelection;
    [SerializeField] private float chaseDistance;
    private bool doingSequence;
    [SerializeField] private float clipTimer;

    public override void OnStartClient()
    {
        base.OnStartClient();

        /* Populate music list */
        // todo

        mainSource.volume = 0.0f;
        fadeSource.volume = 0.0f;
        maxVolume = 0.1f;
        chaseDistance = 50;

        PlayClip(0);
    }

    private void Update()
    {
        /* Should only be executed on clients */
        if (!isClient) { return; }

        Fading();
        PlayChaseSequence(chaseDistance);
        
        // DebugInput();
    }

    private void PlayClip(int index)
    {
        if (mainSource.clip != musicList[index])
        {
            fadeSource.clip = mainSource.clip;
            fadeSource.volume = mainSource.volume;
            fadeSource.time = mainSource.time;

            mainSource.clip = musicList[index];
            mainSource.volume = 0.0f;
            mainSource.time = 0.0f;

            fadeSource.Play();
            mainSource.Play();
        }
    }

    private void PlayHorrorClip()
    {
        horrorSource.clip = horrorList[Random.Range(0, horrorList.Length)];
        horrorSource.Play();
    }

    private void PlayShyHorrorClip()
    {
        horrorSource.clip = horrorList[horrorList.Length - 1];
        horrorSource.Play();
    }

    private void Fading()
    {
        if (fadeSource.volume > 0.0f)
        {
            fadeSource.volume -= 0.25f * Time.deltaTime;
        }

        if (mainSource.volume < maxVolume)
        {
            mainSource.volume += 0.25f * Time.deltaTime;
        }

        if (clipTimer > 0.0f)
        {
            clipTimer -= 1.0f * Time.deltaTime;
        }
    }

    private void DebugInput()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayClip(0);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayClip(1);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayHorrorClip();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            PlayShyHorrorClip();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ActivateSequence(0);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            ActivateSequence(1);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            ActivateSequence(2);
        }

        if (Input.GetKeyDown(KeyCode.Comma))
        {
            ActivateSequence(3);
        }

        if (Input.GetKeyDown(KeyCode.Period))
        {
            ActivateSequence(4);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            ChangeChaseDistance(50);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            ChangeChaseDistance(25);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ChangeChaseDistance(5);
        }

        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            ChangeChaseDistance(-99);
        }
    }

    /* Events */
    public void ActivateSequence(int type)
    {
        if (!doingSequence)
        {
            chaseMusicState = ChaseMusicState.Intro;
            doingSequence = true;

            switch (type)
            {
                case 0: // the Sculpture
                    chaseMusicSelection = new int[] { 4, 5, 6, 7, 8 };
                    break;

                case 1: // the Plague Doctor
                    chaseMusicSelection = new int[] { 9, 10, 11, 12 };
                    break;

                case 2: // the Old Man
                    chaseMusicSelection = new int[] { 13, 14, 15, 16 };
                    break;

                case 3: // the Forest Guardian
                    chaseMusicSelection = new int[] { 18, 19, 20 };
                    break;

                case 4: // the Shy Guy
                    chaseMusicSelection = new int[] { 22, 23, 24 };
                    break;

                default:
                    break;
            }

            Debug.Log("Starting sequence with chase state Intro!");
        }
        else
        {
            // interrupt intro if close enough
            if (chaseMusicState == ChaseMusicState.Intro)
            {
                if (chaseDistance < 10)
                {
                    clipTimer = 0.1f;
                    Debug.Log("Interrupted intro!");
                }
            }

            // interrupt outro
            if (chaseMusicState == ChaseMusicState.Outro)
            {
                switch (type)
                {
                    case 0: // the Sculpture
                        chaseMusicSelection = new int[] { 4, 5, 6, 7, 8 };
                        break;

                    case 1: // the Plague Doctor
                        chaseMusicSelection = new int[] { 9, 10, 11, 12 };
                        break;

                    case 2: // the Old Man
                        chaseMusicSelection = new int[] { 13, 14, 15, 16 };
                        break;

                    case 3: // the Forest Guardian
                        chaseMusicSelection = new int[] { 18, 19, 20 };
                        break;

                    case 4: // the Shy Guy
                        chaseMusicSelection = new int[] { 22, 23, 24 };
                        break;

                    default:
                        break;
                }

                chaseMusicState = ChaseMusicState.Loop;
                clipTimer = 0.0f;
                Debug.Log("Interrupted outro!");
            }
        }
    }

    public void ChangeChaseDistance(float distance)
    {
        chaseDistance = distance;
    }

    private void PlayChaseSequence(float distance)
    {
        if (doingSequence)
        {
            if (chaseMusicState == ChaseMusicState.Intro)
            {
                if (chaseMusicState == previousChaseMusicState)
                {
                    if (clipTimer <= 0.0f)
                    {
                        Debug.Log("Playing intro!");

                        PlayClip(chaseMusicSelection[0]);
                        clipTimer = musicList[chaseMusicSelection[0]].length;
                    }                
                }

                if (clipTimer <= 0.1f)
                {
                    clipTimer = 0.0f;
                    chaseMusicState = ChaseMusicState.Loop;
                }
            }

            if (chaseMusicState == ChaseMusicState.Loop || chaseMusicState == ChaseMusicState.LoopNearby || chaseMusicState == ChaseMusicState.LoopDeath)
            {
                if (distance < 10 && chaseMusicSelection.Length >= 5)
                {
                    chaseMusicState = ChaseMusicState.LoopDeath;
                }
                else if (distance < 15 && chaseMusicSelection.Length > 3)
                {
                    chaseMusicState = ChaseMusicState.LoopNearby;
                }
                else
                {
                    chaseMusicState = ChaseMusicState.Loop;
                }

                if (distance == -99)
                {
                    chaseMusicState = ChaseMusicState.Outro;
                }

                if (chaseMusicState != previousChaseMusicState)
                {
                    switch (chaseMusicState)
                    {
                        case ChaseMusicState.Loop:
                            PlayClip(chaseMusicSelection[1]);
                            break;

                        case ChaseMusicState.LoopNearby:
                            PlayClip(chaseMusicSelection[2]);
                            break;

                        case ChaseMusicState.LoopDeath:
                            PlayClip(chaseMusicSelection[3]);
                            break;

                        default:
                            break;
                    }

                    previousChaseMusicState = chaseMusicState;
                    Debug.Log("Changing loop state to " + chaseMusicState.ToString());
                }
            }

            if (chaseMusicState == ChaseMusicState.Outro)
            {
                if (chaseMusicState == previousChaseMusicState)
                {
                    if (clipTimer <= 0.0f)
                    {
                        Debug.Log("Playing outro!");

                        PlayClip(chaseMusicSelection[chaseMusicSelection.Length - 1]);
                        clipTimer = musicList[chaseMusicSelection[chaseMusicSelection.Length - 1]].length;
                    }
                }

                if (clipTimer <= 1.0f)
                {
                    clipTimer = 0.0f;
                    chaseMusicState = ChaseMusicState.Done;
                }
            }

            if (chaseMusicState == ChaseMusicState.Done)
            {
                doingSequence = false;
                // return to previous
                Debug.Log("Returning to non-chase music!");
                PlayClip(0);
                chaseMusicState = ChaseMusicState.Intro;
            }
        }
    }
}
