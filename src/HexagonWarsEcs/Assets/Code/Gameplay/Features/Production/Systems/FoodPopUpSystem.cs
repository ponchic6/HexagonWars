using Code.Gameplay.Features.PopUp.DataStructures;
using Code.Infrastructure.StaticData;
using Entitas;

namespace Code.Gameplay.Features.Production.Systems
{
    public class FoodPopUpSystem : IExecuteSystem
    {
        private readonly CommonStaticData _commonStaticData;
        private readonly GameContext _game;
        private readonly IGroup<GameEntity> _entities;

        public FoodPopUpSystem(CommonStaticData commonStaticData)
        {
            _commonStaticData = commonStaticData;
            _game = Contexts.sharedInstance.game;

            _entities = _game.GetGroup(GameMatcher.AllOf(GameMatcher.AmountPopUpEvents, GameMatcher.AmountPopUpCooldown, GameMatcher.FoodFarm));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.foodFarm.Workers == 0)
                {
                    entity.amountPopUpEvents.Value.RemoveAll(x => x.sprite == _commonStaticData.FoodIcon);
                    continue;
                }

                if (entity.amountPopUpCooldown.RemainingTime == 0)
                {
                    entity.amountPopUpEvents.Value.RemoveAll(x => x.sprite == _commonStaticData.FoodIcon);
                    entity.amountPopUpEvents.Value.Add(new AmountPopUpContainer(_commonStaticData.FoodIcon, entity.foodAmount.Value, entity.transform.Value));
                }
            }
        }
    }
}