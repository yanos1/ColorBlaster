using System;
using System.Collections;
using Core.Managers;
using GameLogic.PlayerRelated;
using UnityEngine;

namespace GameLogic.Boosters.Gunners
{
    public class ChangingColorBlock : ColorBlock
    {

        private void OnTriggerEnter2D(Collider2D other)
        {
            ColorBlock block = other.GetComponent<ColorBlock>();
            print($"hit {other.gameObject.name}");
            if (block is not null)
            {
                print($"set color of chaningcoloblock to {block.GetColor().ToString()}");
                SetColor(block.GetColor());
            }
        }
    }
}
