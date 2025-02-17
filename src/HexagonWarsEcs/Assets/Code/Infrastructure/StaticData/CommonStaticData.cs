using Code.Gameplay.Features.Logistics.DataStructure;
using Code.Infrastructure.View;
using UnityEngine;

namespace Code.Infrastructure.StaticData
{
    [CreateAssetMenu(fileName = "CommonStaticData", menuName = "StaticData/CommonStaticData")]
    public class CommonStaticData : ScriptableObject
    {
        public EntityBehaviour Hexagon;
        public Sprite ManSprite;
        public Sprite FoodIcon;
        public float BattleCooldown;
        public float BattleArrowsVerticalOffset;
        public float StrongCoefficientOfDefenders;
        public float StrongCoefficientOfAttackers;
        public float FoodPerSecondByBuilders;
        public float FoodPerSecondByWarriors;
        public float FoodPerSecondByCitizens;
        [Range(0, 1)] public float CoefficientBuildersDeathByHungerInAct;
        [Range(0, 1)] public float CoefficientCitizensDeathByHungerInAct;
        [Range(0, 1)] public float CoefficientWarriorsDeathByHungerInAct;
        public float HungerCooldown;
        public float FoodPerformancePerSecond;
        public float WarriorTrainingTime;
        public CourierCapacity[] CourierCapacity;
        public float AmountPopUpCooldown;
    }
}