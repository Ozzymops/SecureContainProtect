using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SCPAI : BasicPlayer
{
    protected bool AIEnabled;
    [SerializeField] protected bool isVisibleByPlayer;
    [SerializeField] protected List<HumanPlayer> visibleByThesePlayers = new List<HumanPlayer>();

    protected enum SCPType { Sculpture, PlagueDoctor, OldMan, ForestGuardian, ShyGuy, TalkingHound, TeddyBear, YarnBall, Zombie }
    [SerializeField] protected SCPType scpType;
    public int visibleSCPType;

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    public void Update()
    {
        ServerUpdate();
        Health();

        visibleSCPType = (int)scpType;
    }

    public void IsVisible(HumanPlayer humanPlayer, bool visible)
    {
        if (visible)
        {
            if (!visibleByThesePlayers.Contains(humanPlayer))
            {
                visibleByThesePlayers.Add(humanPlayer);
            }           
        }
        else
        {
            visibleByThesePlayers.Remove(humanPlayer);
        }
    }
}
