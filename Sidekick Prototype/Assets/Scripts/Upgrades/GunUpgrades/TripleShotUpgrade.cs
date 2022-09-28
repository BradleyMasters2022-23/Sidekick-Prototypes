using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShotUpgrade : IUpgrade
{
    public GameObject projectile;

    public override void LoadUpgrade(PlayerControllerRB player)
    {
        PlayerGun[] t = FindObjectsOfType<PlayerGun>(true);

        foreach (PlayerGun gun in t)
        {
            gun.LoadNewProjectile(projectile);
        }
    }
}
