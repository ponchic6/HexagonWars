using System;
using UnityEngine;

namespace Code.Gameplay.Features.PopUp.DataStructures
{
    [Serializable]
    public class AmountPopUpContainer
    {
        public Sprite sprite;
        public float count;
        public Transform hexTransform;

        public AmountPopUpContainer(Sprite sprite, float count, Transform hexTransform)
        {
            this.sprite = sprite;
            this.count = count;
            this.hexTransform = hexTransform;
        }
    }
}