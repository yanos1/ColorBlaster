﻿using System;
using Core.Managers;
using GameLogic.ObstacleGeneration;
using GameLogic.StyleRelated;
using ScriptableObjects;
using UnityEngine;

namespace GameLogic.PlayerRelated
{
    public class ColorBlock : StyleableObject
    {
        public Vector3 StartingPosition => startingPosition;
        public Vector3 EndGamePosition => endGamePosition;
        public Quaternion StartingRotation => startingRotation;
        
        [SerializeField] private Vector3 endGamePosition;
        
        private Vector3 startingPosition;
        private Quaternion startingRotation;

        public override void Awake()
        {
            base.Awake();
            startingPosition = transform.position;
            startingRotation = transform.rotation;
            CoreManager.instance.StyleManager.AddStyleableObject(this);
            ApplyStyle();
        }

        private void OnDestroy()
        {
            CoreManager.instance.StyleManager.RemoveStyleableObject(this);
        }
        


        public override Style ApplyStyle()
        {
            print("style applied for color block !");
            Style currentStyle = base.ApplyStyle();
            return currentStyle;
        }
        



        public override void ChangeStyle()
        {
            ApplyStyle();
        }
    }
}