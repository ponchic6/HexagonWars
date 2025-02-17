using System.Collections.Generic;
using Code.Gameplay.Features.Battle.DataStructures;
using Code.Gameplay.Features.PopUp.DataStructures;
using Code.Infrastructure.StaticData;
using Entitas;

namespace Code.Gameplay.Features.Battle.Systems
{
    public class WarriorLossesPopUpSystem : IExecuteSystem
    {
        private readonly CommonStaticData _commonStaticData;
        private readonly IGroup<GameEntity> _entities;
        private readonly GameContext _game;

        public WarriorLossesPopUpSystem(CommonStaticData commonStaticData)
        {
            _commonStaticData = commonStaticData;
            
            _game = Contexts.sharedInstance.game;

            _entities = _game.GetGroup(GameMatcher.AllOf(GameMatcher.CurrentBattleCooldown, GameMatcher.BattleCooldown, GameMatcher.Battlefield));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.isDestructed)
                {
                    ClearWarriorPopUpEvents(entity);
                    continue;
                }
                
                if (entity.currentBattleCooldown.Value == 0) 
                    UpdateWarriorPopUpEvents(entity);
            }
        }

        private void UpdateWarriorPopUpEvents(GameEntity entity)
        {
            GameEntity defenderEntity = _game.GetEntityWithId(entity.battlefield.DefenderHexagonContainer.hexagonId);
            List<AmountPopUpContainer> defenderPopUpContainer = defenderEntity.amountPopUpEvents.Value;
            defenderPopUpContainer.RemoveAll(x => x.sprite == _commonStaticData.ManSprite);
            defenderPopUpContainer.Add(new AmountPopUpContainer(_commonStaticData.ManSprite, entity.battlefield.DefenderHexagonContainer.warriorsCount, defenderEntity.transform.Value));

            foreach (WarriorsContainer warriorsContainer in entity.battlefield.AttackerHexagonContainers)
            {
                GameEntity attackerEntity = _game.GetEntityWithId(warriorsContainer.hexagonId);
                List<AmountPopUpContainer> attackerPopUpContainer = attackerEntity.amountPopUpEvents.Value;
                attackerPopUpContainer.RemoveAll(x => x.sprite == _commonStaticData.ManSprite);
                attackerPopUpContainer.Add(new AmountPopUpContainer(_commonStaticData.ManSprite, warriorsContainer.warriorsCount, attackerEntity.transform.Value));
            }
        }

        private void ClearWarriorPopUpEvents(GameEntity entity)
        {
            GameEntity defenderEntity = _game.GetEntityWithId(entity.battlefield.DefenderHexagonContainer.hexagonId);
            List<AmountPopUpContainer> defenderPopUpContainer = defenderEntity.amountPopUpEvents.Value;
            defenderPopUpContainer.RemoveAll(x => x.sprite == _commonStaticData.ManSprite);
                    
            foreach (WarriorsContainer warriorsContainer in entity.battlefield.AttackerHexagonContainers)
            {
                GameEntity attackerEntity = _game.GetEntityWithId(warriorsContainer.hexagonId);
                List<AmountPopUpContainer> attackerPopUpContainer = attackerEntity.amountPopUpEvents.Value;
                attackerPopUpContainer.RemoveAll(x => x.sprite == _commonStaticData.ManSprite);
            }
        }
    }
}