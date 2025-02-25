using Code.Gameplay.Features.Battle.Services;
using Entitas;

namespace Code.Gameplay.Features.Battle.Systems
{
    public class BattlefieldFromWarriorsMigrationSystem : IExecuteSystem
    {
        private readonly IBattleFieldFactory _battleFieldFactory;
        private readonly GameContext _game;
        private readonly IGroup<GameEntity> _entities;
        
        public BattlefieldFromWarriorsMigrationSystem(IBattleFieldFactory battleFieldFactory)
        {
            _battleFieldFactory = battleFieldFactory;
            _game = Contexts.sharedInstance.game;
            
            _entities = _game.GetGroup(GameMatcher.AllOf(
                GameMatcher.ComplexityWay,
                GameMatcher.WayIdPoints,
                GameMatcher.WarriorsMigrationAmount,
                GameMatcher.HexagonForAttack,
                GameMatcher.Destructed));
        }

        public void Execute()
        {
            foreach (GameEntity entity in _entities) 
                _battleFieldFactory.CreateBattlefieldFromWarriorsMigration(entity);
        }
    }
}