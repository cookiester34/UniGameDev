using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAnimTrigger : MonoBehaviour
{
    public TowerBuilding tower;

    public void Shoot()
    {
        tower.Shoot();
    }
}
