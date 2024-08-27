using UnityEngine;

namespace Core.PlayerRelated
{
    public class Player : MonoBehaviour
    {

        public Shooter Shooter => shooter;
        public PlayerMovement PlayerMovement => playerMovement;
        
        [SerializeField] private Shooter shooter;
        [SerializeField] private PlayerMovement playerMovement;
        
        
    }
}