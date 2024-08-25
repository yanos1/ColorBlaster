using System;
using Core.PlayerRelated;
using UnityEngine;

namespace Core.Managers
{
    public class ColorBlockManager : MonoBehaviour
    {
        [SerializeField] private ColorBlock[] blocks;

        public ColorBlock[] ColorBlocks => blocks;


        private void Awake()
        {
            blocks = new ColorBlock[4];
        }

        private void Start()
        {
            Color[] styleColors = CoreManager.instance.StyleManager.GetStyle().ColorPalette;

            if (blocks.Length != styleColors.Length)
            {
                Debug.Log("Mismatch between style colors and color blocks");
            }

            for (int i = 0; i < styleColors.Length; ++i)
            {
                blocks[i].color = styleColors[i];
            }
        }
    }
}