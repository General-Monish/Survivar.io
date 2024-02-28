using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knife : WeaponController
{
    // Start is called before the first frame update
   protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        if (playerController.isWalking)
        {
            base.Attack();
            GameObject spawnKnife = Instantiate(weaponData.prefabs);
            spawnKnife.transform.position = transform.position;
            spawnKnife.GetComponent<KnifeBehaviour>().DirChecker(playerController.moveInput);
        }
    }
}
