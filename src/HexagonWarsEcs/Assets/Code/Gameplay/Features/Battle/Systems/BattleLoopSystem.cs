using System;
using System.Collections.Generic;
using System.Linq;
using Code.Infrastructure.StaticData;
using Entitas;

namespace Code.Gameplay.Features.Battle.Systems
{
    public class BattleLoopSystem : IExecuteSystem
    {
        private readonly CommonStaticData _commonStaticData;
        private readonly IGroup<GameEntity> _entities;
        private readonly GameContext _game;

        public BattleLoopSystem(CommonStaticData commonStaticData)
        {
            _commonStaticData = commonStaticData;
            _game = Contexts.sharedInstance.game;

            _entities = _game.GetGroup(GameMatcher.AllOf(GameMatcher.Battlefield));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.currentBattleCooldown.Value > 0) 
                    continue;

                int attackers = entity
                    .battlefield
                    .AttackerHexagonsId
                    .Sum(x => _game.GetEntityWithId(x).warriorsAmount.Value);
                
                int defenders = _game
                    .GetEntityWithId(entity.battlefield.DefenderHexagonId)
                    .warriorsAmount
                    .Value;

                List<int> attackersList = entity
                        .battlefield
                        .AttackerHexagonsId
                        .Select(x => _game.GetEntityWithId(x).warriorsAmount.Value)
                        .ToList();
                
                int totalAttackersLosses = (int)Math.Round(_commonStaticData.StrongCoefficientOfDefenders * defenders * defenders);

                List<int> distributeLosses = DistributeLosses(attackersList, totalAttackersLosses);

                for (var i = 0; i < entity.battlefield.AttackerHexagonsId.Count; i++)
                {
                    GameEntity attackerHex = _game.GetEntityWithId(entity.battlefield.AttackerHexagonsId[i]);
                    
                    attackerHex.warriorsAmount.Value -= distributeLosses[i];
                    
                    if (attackerHex.warriorsAmount.Value < 0) 
                        attackerHex.warriorsAmount.Value = 0;
                    
                    attackerHex.ReplaceWarriorsAmount(attackerHex.warriorsAmount.Value);
                }

                GameEntity defenderHex = _game.GetEntityWithId(entity.battlefield.DefenderHexagonId);
                
                defenderHex.warriorsAmount.Value -= (int)Math.Round(_commonStaticData.StrongCoefficientOfAttackers * attackers * attackers);

                if (defenderHex.warriorsAmount.Value < 0)
                    defenderHex.warriorsAmount.Value = 0;
                
                entity.ReplaceBattlefield(entity.battlefield.AttackerHexagonsId, entity.battlefield.DefenderHexagonId);
            }
        }
        
        private List<int> DistributeLosses(List<int> units, int totalLosses)
        {
            int totalUnits = units.Sum();
            double lossRatio = (double)totalLosses / totalUnits;
            List<int> losses = new List<int>(new int[units.Count]);
            int calculatedLosses = 0;
            
            for (int i = 0; i < units.Count; i++)
            {
                losses[i] = (int)Math.Round(units[i] * lossRatio);
                calculatedLosses += losses[i];
            }
            
            int difference = calculatedLosses - totalLosses;
            while (difference != 0)
            {
                int index = losses.IndexOf(losses.Max()); 
                losses[index] -= Math.Sign(difference);
                difference -= Math.Sign(difference);
            }

            return losses;        
        }
    }
}