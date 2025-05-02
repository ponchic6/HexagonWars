using System.Collections.Generic;
using System.Linq;
using Code.Gameplay.Common.Services;
using Entitas;

namespace Code.Gameplay.Features.Logistics.Systems
{
    public class SupplyRoutsAddUiReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly IUIFactory _uiFactory;

        public SupplyRoutsAddUiReactiveSystem(IContext<GameEntity> context, IUIFactory uiFactory) : base(context)
        {
            _uiFactory = uiFactory;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) => 
            context.CreateCollector(GameMatcher.SupplyRoute.Added());

        protected override bool Filter(GameEntity entity)
        {
            if (_uiFactory.SupplyRoutsInfoPanel.HexEntityBehaviour.Entity.id.Value ==
                entity.wayIdPoints.Value.Last()) 
                return true;
            
            return false;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities) 
                _uiFactory.SupplyRoutsInfoPanel.AddSupplyRout(entity);
        }
    }
}