using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HumanPlayer : BasicPlayer
{
    #region Reference
    private MusicManager musicManager;
    private HumanSoundManager humanSoundManager;
    #endregion

    #region Misc. variables
    [SerializeField] private float scanTimer;
    [SerializeField] private List<GameObject> scannedSCP = new List<GameObject>();
    [SerializeField] private List<GameObject> visibleSCP = new List<GameObject>();
    #endregion

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

    private float chaseTimer;
    [SerializeField] private float lastNearestDistance;

    public float visibleStamina;
    public float visibleMaxStamina;
    public float visibleBlinkTimer;
    public float visibleMaxBlinkTimer;

    public bool visibleMoving;
    public bool visibleSprinting;
    public bool visibleSneaking;
    #endregion

    #region Equipped items
    private bool equippedGasMask;

    public bool visibleEquippedGasMask;
    #endregion

    public override void OnStartClient()
    {
        base.OnStartClient();

        /* Set references */
        humanSoundManager = GetComponent<HumanSoundManager>();
    }

    public override void OnStartLocalPlayer()
    {
        /* Get references */
        musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();

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

        lastNearestDistance = 999;

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
        ScanForSCP();
        VisionCone();

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
            visibleSprinting = true;
            visibleSneaking = true;
            modifiedMovementSpeed = baseMovementSpeed;
        }
        else if (sprinting)
        {
            visibleSprinting = true;
            visibleSneaking = false;
            modifiedMovementSpeed = baseMovementSpeed * sprintSpeedMultiplier;
        }
        else if (sneaking)
        {
            visibleSprinting = false;
            visibleSneaking = true;
            modifiedMovementSpeed = baseMovementSpeed * sneakSpeedMultiplier;
        }
        else
        {
            visibleSprinting = false;
            visibleSneaking = false;
            modifiedMovementSpeed = baseMovementSpeed;
        }

        if (movementInput.x > 0 || movementInput.x < 0 || movementInput.y > 0 || movementInput.y < 0)
        {
            visibleMoving = true;
        }
        else
        {
            visibleMoving = false;
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

    private void ScanForSCP()
    {
        scanTimer -= 1.0f * Time.deltaTime;

        if (scanTimer <= 0.0f)
        {
            scanTimer = 10.0f;

            if (scannedSCP.Count > 0)
            {
                foreach(GameObject SCP in scannedSCP)
                {
                    if (!visibleSCP.Contains(SCP))
                    {
                        SCP.GetComponent<SCPAI>().IsVisible(this, false);
                    }
                }
            }

            scannedSCP.Clear();
            GameObject[] SCPArray = GameObject.FindGameObjectsWithTag("SCP");

            if (SCPArray.Length > 0)
            {
                foreach(GameObject SCP in SCPArray)
                {
                    scannedSCP.Add(SCP);
                }
            }
        }
    }

    private void VisionCone()
    {
        visibleSCP.Clear();

        if (scannedSCP.Count > 0)
        {
            foreach (GameObject SCP in scannedSCP)
            {
                if (Vector3.Distance(transform.position, SCP.transform.position) <= 60)
                {
                    Vector3 positionViaCamera = Camera.main.WorldToViewportPoint(SCP.transform.position);
                    bool isVisible = (positionViaCamera.z > 0 && positionViaCamera.x > 0 && positionViaCamera.x < 1 && positionViaCamera.y > 0 && positionViaCamera.y < 1);

                    if (isVisible)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, (SCP.transform.position - transform.position), out hit, 30))
                        {
                            if (hit.transform.gameObject.tag == "SCP")
                            {
                                if (blinkTimer != 0.0f)
                                {
                                    // if not blinking
                                    if (SCP.GetComponent<SCPAI>().visibleSCPType == 4)
                                    {
                                        musicManager.PlayShyHorrorClip();
                                    }
                                    Debug.DrawRay(transform.position, (hit.point - transform.position), Color.green);
                                    visibleSCP.Add(SCP);
                                }
                                else
                                {
                                    if (SCP.GetComponent<SCPAI>().visibleSCPType != 4)
                                    {
                                        musicManager.ResetHorrorCooldown();
                                    }                              
                                    Debug.DrawRay(transform.position, (hit.point - transform.position), Color.yellow);
                                }
                            }
                            else
                            {
                                Debug.DrawRay(transform.position, (hit.point - transform.position), Color.red);
                            }
                        }
                    }
                }
            }
        }

        foreach (GameObject SCP in scannedSCP)
        {
            lastNearestDistance = GetNearestSCPDistance();
            musicManager.ChangeChaseDistance(lastNearestDistance);

            int sequenceIndex = 0;

            if (SCP.GetComponent<SCPAI>().visibleSCPType >= 0 && SCP.GetComponent<SCPAI>().visibleSCPType <= 4)
            {
                sequenceIndex = SCP.GetComponent<SCPAI>().visibleSCPType;
            }
            else
            {
                sequenceIndex = 0;
            }

            if (visibleSCP.Contains(SCP))
            {
                SCP.GetComponent<SCPAI>().IsVisible(this, true);
                chaseTimer = 30.0f;
                musicManager.ActivateSequence(sequenceIndex);
            }
            else
            {
                chaseTimer -= 1.0f * Time.deltaTime;
                SCP.GetComponent<SCPAI>().IsVisible(this, false);
            }
        }
    }

    private float GetNearestSCPDistance()
    {
        if (visibleSCP.Count > 0)
        {
            float nearestDistance = 999;

            foreach (GameObject SCP in visibleSCP)
            {
                float distance = Vector3.Distance(transform.position, SCP.transform.position);

                if (nearestDistance == 999 || nearestDistance > distance)
                {
                    nearestDistance = distance;
                }
            }

            if (nearestDistance < 10)
            {
                musicManager.PlayHorrorClip();
            }

            return nearestDistance;
        }

        if (chaseTimer <= 0.0f)
        {
            lastNearestDistance = 999;
            return -99;
        }

        return lastNearestDistance;
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

        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            chaseTimer = 0.0f;
        }
    }
}
