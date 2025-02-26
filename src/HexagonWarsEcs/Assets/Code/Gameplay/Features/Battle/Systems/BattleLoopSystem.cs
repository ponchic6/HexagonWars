﻿using System;
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

        public BattleLoopSystem(CommonStaticData commonStaticData)
        {
            _commonStaticData = commonStaticData;
            GameContext game = Contexts.sharedInstance.game;

            _entities = game.GetGroup(GameMatcher.AllOf(GameMatcher.Battlefield, GameMatcher.CurrentBattleCooldown, GameMatcher.BattleCooldown));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.currentBattleCooldown.Value > 0) 
                    continue;

                int attackers = entity.battlefield.AttackerHexagonContainers.Sum(x => x.warriorsCount);
                int defenders = entity.battlefield.DefenderHexagonContainer.warriorsCount;

                List<int> attackersList = entity.battlefield.AttackerHexagonContainers.Select(x => x.warriorsCount).ToList();
                int totalAttackersLosses = (int)Math.Round(_commonStaticData.StrongCoefficientOfDefenders * defenders * defenders);

                List<int> distributeLosses = DistributeLosses(attackersList, totalAttackersLosses);

                for (var i = 0; i < entity.battlefield.AttackerHexagonContainers.Count; i++)
                {
                    var warriorsContainer = entity.battlefield.AttackerHexagonContainers[i];
                    warriorsContainer.warriorsCount -= distributeLosses[i];
                    
                    if (warriorsContainer.warriorsCount < 0) 
                        warriorsContainer.warriorsCount = 0;
                }
                
                entity.battlefield.DefenderHexagonContainer.warriorsCount -= (int)Math.Round(_commonStaticData.StrongCoefficientOfAttackers * attackers * attackers);
                
                if (entity.battlefield.DefenderHexagonContainer.warriorsCount < 0)
                    entity.battlefield.DefenderHexagonContainer.warriorsCount = 0;
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