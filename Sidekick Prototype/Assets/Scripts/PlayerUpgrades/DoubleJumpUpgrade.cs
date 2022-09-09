using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpUpgrade : IUpgrade
{
    public override void LoadUpgrade(PlayerControllerRB player)
    {
        player.maxJumps += 1;
    }
}
