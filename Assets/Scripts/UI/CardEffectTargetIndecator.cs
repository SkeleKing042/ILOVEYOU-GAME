using ILOVEYOU.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU.UI
{
    public static class CardEffectTargetIndecator
    {
        static public void Castout(PlayerManager[] targs)
        {
            foreach(PlayerManager target in targs)
            {
                Camera cam = target.GetComponentInChildren<Camera>();
                Vector2 position = cam.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
            }
        }
    }
}
