﻿using Code.Infrastructure.View;
using UnityEngine;

namespace Code.Infrastructure.StaticData
{
    [CreateAssetMenu(fileName = "CommonStaticData", menuName = "StaticData/CommonStaticData")]
    public class CommonStaticData : ScriptableObject
    {
        public EntityBehaviour Hexagon;
        public Sprite Man;
    }
}