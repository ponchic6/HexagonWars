using Code.Infrastructure.StaticData;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Production.Systems
{
    public class MineSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly CommonStaticData _commonStaticData;

        public MineSystem(CommonStaticData commonStaticData)
        {
            _commonStaticData = commonStaticData;
            GameContext game = Contexts.sharedInstance.game;

            _entities = game.GetGroup(GameMatcher.Mine);
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                switch (entity.mine.OreType)
                {
                    case OreType.Iron:
                        float newIronValue = entity.ironAmount.Value + entity.mine.Miners * Time.deltaTime * _commonStaticData.IronPerformancePerSecond;
                        entity.ReplaceIronAmount(newIronValue);
                        break;
                    case OreType.Coal:
                        float newCoalValue = entity.coalAmount.Value + entity.mine.Miners * Time.deltaTime * _commonStaticData.CoalPerformancePerSecond;
                        entity.ReplaceCoalAmount(newCoalValue);
                        break;
                }
            }
        }
    }
}