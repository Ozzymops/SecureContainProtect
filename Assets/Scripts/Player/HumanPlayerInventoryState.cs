using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayerInventoryState
{
    virtual public HumanPlayerInventoryState handleInput(HumanPlayer player)
    {
        return this;
    }
}

public class HumanPlayerNothingInventoryState : HumanPlayerInventoryState
{
    override public HumanPlayerInventoryState handleInput(HumanPlayer player)
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            return new HumanPlayerGasMaskInventoryState();
        }

        return this;
    }
}

public class HumanPlayerGasMaskInventoryState : HumanPlayerInventoryState
{
    override public HumanPlayerInventoryState handleInput(HumanPlayer player)
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            return new HumanPlayerNothingInventoryState();
        }

        return this;
    }
}