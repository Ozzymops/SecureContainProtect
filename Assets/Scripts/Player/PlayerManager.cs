using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    [Header("References")]
    private PlayerStatus statusScript;
    private PlayerUI uiScript;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        uiScript.GetElements(GameObject.Find("Canvas").GetComponent<Canvas>());
    }

    public void Awake()
    {
        statusScript = GetComponent<PlayerStatus>();
        uiScript = GetComponent<PlayerUI>();
    }

    public void Update()
    {
        if (!isLocalPlayer) { return; }

        if (statusScript.sharedVariables.Length > 0) { uiScript.UpdateElements(statusScript.sharedVariables); }
        if (statusScript.sharedBools.Length > 0) { uiScript.Blink(statusScript.sharedBools[0]); }
        
        // Debug
        if (Input.GetKeyDown(KeyCode.O)) // damage 10
        {
            Cmd_TakeDamage(5, 0);
        }

        if (Input.GetKeyDown(KeyCode.P)) // heal 10
        {
            Cmd_Heal(10);
        }
    }

    public void Cmd_TakeDamage(float damage, int type)
    {
        statusScript.TakeDamage(damage);
        // soundscript damage type
        // effectscript damage type
    }

    public void Cmd_Heal(float heal)
    {
        statusScript.Heal(heal);
        // soundscript heal
    }
}
