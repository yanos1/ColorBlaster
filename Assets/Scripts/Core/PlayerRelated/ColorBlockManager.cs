using Core.Managers;
using UnityEngine;

namespace Core.PlayerRelated
{
    // this manager styles the circle of colors aorund the plyer based on the current style
    public class ColorBlockManager : MonoBehaviour
    {
        [SerializeField] private ColorBlock[] blocks;
        [SerializeField] float rotationSpeed; // Speed of rotation
        
        private Player player;

        private void Start()
        {
            Color[] styleColors = CoreManager.instance.StyleManager.GetStyle().ColorPalette;
            player = CoreManager.instance.Player;

            if (blocks.Length != styleColors.Length)
            {
                Debug.Log("Mismatch between style colors and color blocks");
            }

            for (int i = 0; i < styleColors.Length; ++i)
            {
                blocks[i].SetColor(styleColors[i]);
            }
        }


        private void Update()
        {
            RotateAroundPlayer();
        }

        private void RotateAroundPlayer()
        {
            foreach (var colorBlock in blocks)
            {
                if (colorBlock != null)
                {
                    colorBlock.transform.RotateAround(player.transform.position, Vector3.back,
                        rotationSpeed * Time.deltaTime);
                }
            }
        }
    }
}