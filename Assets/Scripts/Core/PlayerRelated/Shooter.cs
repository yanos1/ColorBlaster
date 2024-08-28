using System.Collections;
using System.Collections.Generic;
using Core.Managers;
using Core.PlayerRelated;
using ObstacleGeneration;
using UnityEngine;

public class Shooter : MonoBehaviour
{

    [SerializeField] private Transform shootingPosition;
    // Start is called before the first frame update
    private PoolType bullet = PoolType.Bullet;
    private Color lastShotColor;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        GameObject bulletPrefab = CoreManager.instance.PoolManager.GetFromPool(bullet);
        bulletPrefab.transform.position = shootingPosition.position;
    }
}
