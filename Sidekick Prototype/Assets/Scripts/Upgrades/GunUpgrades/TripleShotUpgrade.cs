using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShotUpgrade : IUpgrade
{
    public GameObject projectile;
    public GameObject projectileHitscan;

    public override void LoadUpgrade(PlayerControllerRB player)
    {
        PlayerGun[] t = FindObjectsOfType<PlayerGun>(true);

        foreach (PlayerGun gun in t)
        {
            if(gun.CompareTag("Hitscan"))
            {
                gun.LoadNewProjectile(projectileHitscan);
            }
            else
                gun.LoadNewProjectile(projectile);
        }
    }
}
