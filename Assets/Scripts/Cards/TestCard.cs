using ILOVEYOU.Cards;
using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCard : DisruptCard
{
    public override void ExecuteEvents(PlayerManager caller)
    {
        base.ExecuteEvents(caller);

        Debug.Log("Effect executed");
    }
}
