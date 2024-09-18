// using Core.Managers;
// using PoolTypes;
//
// namespace GameLogic.ConsumablesGeneration
// {
//     public class ColorRushPowerUp : Consumable
//     {
//         // [SerializeField] private PoolType consumeParticles;
//         public override void Consume()
//         {
//             // CoreManager.instance.PoolManager.GetFromPool(consumeParticles);
//             CoreManager.instance.EventManager.InvokeEvent(EventNames.ActivateColorRush,null);
//             CoreManager.instance.PoolManager.ReturnToPool(PoolType, gameObject);
//         }
//
//         public override void Update()
//         {
//             base.Update();
//         }
//     }
// }