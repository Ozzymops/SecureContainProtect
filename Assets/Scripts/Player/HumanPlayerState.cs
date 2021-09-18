using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayerState
{
    virtual public HumanPlayerState handleInput(HumanPlayer player)
    {
        return this;
    }
}

public class HumanPlayerIdleState : HumanPlayerState
{
    override public HumanPlayerState handleInput(HumanPlayer player)
    {
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (movement.x != 0.0f || movement.y != 0.0f)
        {
            return new HumanPlayerMovingState();
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.GetStamina()[0] > 10.0f)
        {
            return new HumanPlayerJumpingState();
        }

        return this;
    }
}

public class HumanPlayerMovingState : HumanPlayerState
{
    override public HumanPlayerState handleInput(HumanPlayer player)
    {
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.DoMovement(movement);

        if (Input.GetKeyDown(KeyCode.Space) && player.GetStamina()[0] > 10.0f)
        {
            return new HumanPlayerJumpingState();
        }

        return this;
    }
}

public class HumanPlayerJumpingState : HumanPlayerState
{
    override public HumanPlayerState handleInput(HumanPlayer player)
    {
        player.ModifyStamina(-10);

        float[] stamina = player.GetStamina();
        Debug.Log(stamina[0] + " | " + stamina[1]);

        return new HumanPlayerIdleState();
    }
}