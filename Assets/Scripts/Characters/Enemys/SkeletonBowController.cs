using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBowController : EnemyController
{
    public Transform arrowPos;

    public GameObject arrowPrefab;

    public void Shoot()
    {
        Instantiate(arrowPrefab, arrowPos.position, arrowPos.rotation);
    }
}
