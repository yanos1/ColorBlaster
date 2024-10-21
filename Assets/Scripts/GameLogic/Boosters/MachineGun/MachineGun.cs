using Core.Managers;
using GameLogic.ObstacleGeneration;
using UnityEngine;

namespace GameLogic.PlayerRelated
{
    public  class MachineGun : MonoBehaviour
    {


        private void OnTriggerEnter2D(Collider2D col)
        {
            ObstaclePart obstaclePart = col.GetComponent<ObstaclePart>();
            if (obstaclePart is not null)
            {
                CoreManager.instance.EventManager.InvokeEvent(EventNames.KillPlayer,null);
            }
        }
    }
}