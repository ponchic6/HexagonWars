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

                GameEntity attackerHex = _game.GetEntityWithId(entity.battlefield.AttackerHexagonId);
                GameEntity defenderHex = _game.GetEntityWithId(entity.battlefield.DefenderHexagonId);
                
                int attackers = attackerHex.manAmount.Value;
                int defenders = defenderHex.manAmount.Value;

                int totalAttackersLosses = (int)Math.Round(_commonStaticData.StrongCoefficientOfDefenders * defenders * defenders);
                int totalDefendersLosses = (int)Math.Round(_commonStaticData.StrongCoefficientOfDefenders * attackers * attackers);

                attackerHex.manAmount.Value -= totalAttackersLosses;
                    
                if (attackerHex.manAmount.Value < 0) 
                    attackerHex.manAmount.Value = 0;
                    
                attackerHex.ReplaceManAmount(attackerHex.manAmount.Value);
                
                defenderHex.manAmount.Value -= totalDefendersLosses;
                    
                if (defenderHex.manAmount.Value < 0) 
                    defenderHex.manAmount.Value = 0;
                    
                defenderHex.ReplaceManAmount(defenderHex.manAmount.Value);
                entity.ReplaceBattlefield(entity.battlefield.AttackerHexagonId, entity.battlefield.DefenderHexagonId);
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