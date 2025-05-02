using System.Collections.Generic;
using Code.Gameplay.Common.Services;
using Entitas;

namespace Code.Gameplay.Features.Logistics.Systems
{
    public class SupplyRoutsRemoveUiReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly IUIFactory _uiFactory;

        public SupplyRoutsRemoveUiReactiveSystem(IContext<GameEntity> context, IUIFactory uiFactory) : base(context)
        {
            _uiFactory = uiFactory;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) => 
            context.CreateCollector(GameMatcher.Destructed.Added());

        protected override bool Filter(GameEntity entity)
        {
            if (entity.isSupplyRoute) 
                return true;
            
            return false;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities) 
                _uiFactory.SupplyRoutsInfoPanel.RemoveSupplyRout(entity);
        }
    }
}