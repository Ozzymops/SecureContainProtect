using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SCPPlayer : SCPAI
{
    public override void OnStartClient()
    {
        base.OnStartClient();

        AIEnabled = false;

        // Testing
        scpType = SCPType.Sculpture;
    }
}
