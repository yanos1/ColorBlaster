using UnityEngine;

namespace Core.PlayerRelated
{
    public class ColorBlock : MonoBehaviour
    {
        public Color color;

        void Start()
        {
            // Apply the color to the GameObject's material on start
            GetComponent<Renderer>().material.color = color;
        }
        
        
    }
}