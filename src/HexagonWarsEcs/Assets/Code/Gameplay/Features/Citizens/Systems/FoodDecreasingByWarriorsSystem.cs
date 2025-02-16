using Code.Gameplay.Features.Battle.DataStructures;
using Code.Infrastructure.StaticData;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Citizens.Systems
{
    public class FoodDecreasingByWarriorsSystem : IExecuteSystem
    {
        private readonly CommonStaticData _commonStaticData;
        private readonly GameContext _game;
        private readonly IGroup<GameEntity> _hexagons;
        private readonly IGroup<GameEntity> _battlefields;

        public FoodDecreasingByWarriorsSystem(CommonStaticData commonStaticData)
        {
            _commonStaticData = commonStaticData;
            _game = Contexts.sharedInstance.game;

            _hexagons = _game.GetGroup(GameMatcher.AllOf(GameMatcher.WarriorsAmount, GameMatcher.FoodAmount));
            _battlefields = _game.GetGroup(GameMatcher.Battlefield);
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _battlefields)
            {
                DecreaseFoodByAttackers(entity);
                DecreaseFoodByDefenders(entity);
            }
            
            foreach (GameEntity entity in _hexagons)
            {
                if (entity.foodAmount.Value <= 0)
                    entity.ReplaceFoodAmount(0);
                else
                    entity.ReplaceFoodAmount(entity.foodAmount.Value -
                                             entity.warriorsAmount.Value * Time.deltaTime * _commonStaticData.FoodPerSecondByWarriors);
            }
        }

        private void DecreaseFoodByAttackers(GameEntity entity)
        {
            foreach (WarriorsContainer warriorsContainer in entity.battlefield.AttackerHexagonContainers)
            {
                GameEntity attackerEntity = _game.GetEntityWithId(warriorsContainer.hexagonId);
                
                if (attackerEntity.foodAmount.Value <= 0)
                    attackerEntity.ReplaceFoodAmount(0);
                else
                    attackerEntity.ReplaceFoodAmount(attackerEntity.foodAmount.Value - 
                                                     warriorsContainer.warriorsCount * Time.deltaTime * _commonStaticData.FoodPerSecondByWarriors);
            }
        }

        private void DecreaseFoodByDefenders(GameEntity entity)
        {
            GameEntity defenderEntity = _game.GetEntityWithId(entity.battlefield.DefenderHexagonContainer.hexagonId);
            int defendersAmount = entity.battlefield.DefenderHexagonContainer.warriorsCount;

            if (defenderEntity.foodAmount.Value <= 0)
                defenderEntity.ReplaceFoodAmount(0);
            else
                defenderEntity.ReplaceFoodAmount(defenderEntity.foodAmount.Value - 
                                                 defendersAmount * Time.deltaTime * _commonStaticData.FoodPerSecondByWarriors);
        }
    }
}