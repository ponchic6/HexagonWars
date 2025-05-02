using System.Collections.Generic;
using Entitas;

namespace Code.Gameplay.Features.Logistics.Systems
{
    public class SupplyRoutHighlightReactiveSystem : ReactiveSystem<GameEntity>
    {
        public SupplyRoutHighlightReactiveSystem(IContext<GameEntity> context) : base(context)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.HighlightedSupplyRout.AddedOrRemoved());

        protected override bool Filter(GameEntity entity)
        {
            if (entity.isSupplyRoute)
                return true;
            
            return false;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                if (entity.isHighlightedSupplyRout)
                    entity.supplyHighlighter.Value.HighlightRout();
                else
                    entity.supplyHighlighter.Value.UnhighlightRout();
            }
        }
    }
}