using System.Collections.Generic;
using Code.Gameplay.Features.Battle.DataStructures;
using Code.Infrastructure.Services;
using Code.Infrastructure.View;
using Entitas;

namespace Code.Gameplay.Features.Battle.Services
{
    public class BattleFieldFactory : IBattleFieldFactory
    {
        private const string MOVING_ARROW_PATH = "Arrows/MigrationArrow/MigrationArrow";
        
        private readonly IIdentifierService _identifierService;
        private EntityBehaviour _attackersHex, _defendersHex;
        private GameContext _game;
        private int _warriorsAmount;

        public BattleFieldFactory(IIdentifierService identifierService)
        {
            _identifierService = identifierService;
            _game = Contexts.sharedInstance.game;
        }   
        
        public void SetAttackers(EntityBehaviour entityBehaviour, int selectedWarriors)
        {
            _attackersHex = entityBehaviour;
            _warriorsAmount = selectedWarriors;
        }

        public void SetDefendersAndCreateBattlefield(EntityBehaviour entityBehaviour)
        {
            _defendersHex = entityBehaviour;

            IGroup<GameEntity> entities = _game.GetGroup(GameMatcher.Battlefield);
            
            foreach (GameEntity entity in entities)
            {
                if (entity.battlefield.DefenderHexagonContainer.hexagonId == _defendersHex.Entity.id.Value)
                {
                    CreateBattleArrow(entity);
                    
                    entity.battlefield.AttackerHexagonContainers.Add(new WarriorsContainer(_warriorsAmount, _attackersHex.Entity.id.Value));
                    _attackersHex.Entity.warriorsAmount.Value -= _warriorsAmount;
                    _attackersHex = null;
                    _defendersHex = null;
                    _warriorsAmount = 0;
                    return;
                }
            }

            GameEntity battlefield = CreateBattlefield();
            CreateBattleArrow(battlefield);

            _attackersHex = null;
            _defendersHex = null;
            _warriorsAmount = 0;
        }

        private void CreateBattleArrow(GameEntity battlefield)
        {
            GameEntity battleArrow = _game.CreateEntity();
            battleArrow.AddViewPath(MOVING_ARROW_PATH);
            battleArrow.AddBattleArrow(battlefield.id.Value);
            battleArrow.AddWayIdPoints(new (){ _attackersHex.Entity.id.Value, _defendersHex.Entity.id.Value });
        }

        private GameEntity CreateBattlefield()
        {
            GameEntity battlefield = _game.CreateEntity();
            battlefield.AddId(_identifierService.Next());
            battlefield.AddCurrentBattleCooldown(0f);
            battlefield.AddBattleCooldown(2f);
            WarriorsContainer attackers = new WarriorsContainer(_warriorsAmount, _attackersHex.Entity.id.Value);
            WarriorsContainer defenders = new WarriorsContainer(_defendersHex.Entity.warriorsAmount.Value, _defendersHex.Entity.id.Value);
            battlefield.AddBattlefield(new List<WarriorsContainer>{ attackers }, defenders);
            _attackersHex.Entity.warriorsAmount.Value -= _warriorsAmount;
            _defendersHex.Entity.warriorsAmount.Value = 0;
            return battlefield;
        }
    }
}