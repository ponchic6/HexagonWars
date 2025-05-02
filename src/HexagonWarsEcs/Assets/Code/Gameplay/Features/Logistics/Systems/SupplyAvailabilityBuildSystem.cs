using Entitas;

namespace Code.Gameplay.Features.Logistics.Systems
{
    public class SupplyAvailabilityBuildSystem : IExecuteSystem
    {
        private readonly GameContext _game;
        private readonly IGroup<GameEntity> _unavailableSupplyEntity;
        private readonly IGroup<GameEntity> _availableSupplyEntity;

        public SupplyAvailabilityBuildSystem()
        {
            _game = Contexts.sharedInstance.game;

            _availableSupplyEntity = _game.GetGroup(GameMatcher.PlayerHexagon);
            _unavailableSupplyEntity = _game.GetGroup(GameMatcher.EnemyHexagon);
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _availableSupplyEntity) 
                entity.isAvailabilityForSupplyRout = true;
            
            foreach (GameEntity entity in _unavailableSupplyEntity) 
                entity.isAvailabilityForSupplyRout = false;
        }
    }
}