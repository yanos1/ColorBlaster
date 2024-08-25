using Core.Managers;
using UnityEngine;

namespace Core.PlayerRelated
{
    public class ColorBlockRotator : MonoBehaviour
    {
        private Player player;
        private ColorBlock[] _colorBlocks;
        public float rotationSpeed = 20f; // Speed of rotation

        private void Start()
        {
            player = CoreManager.instance.Player;
            _colorBlocks = CoreManager.instance.ColorBlockManager.ColorBlocks;
        }

        private void RotateAroundPlayer()
        {
            foreach (var colorBlock in _colorBlocks)
            {
                if (colorBlock != null)
                {
                    colorBlock.transform.RotateAround(player.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
                }
            }
        }

        private void Update()
        {
            RotateAroundPlayer();
        }
    }
}