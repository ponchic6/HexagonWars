using System.Collections.Generic;
using Code.Gameplay.Common;
using Code.Gameplay.Features.Battle.DataStructures;
using Code.Gameplay.Features.Map.View;
using Code.Gameplay.Features.Migration.Services;
using Code.Infrastructure.Services;
using Code.Infrastructure.StaticData;
using Code.Infrastructure.View;
using Entitas;

namespace Code.Gameplay.Features.Battle.Services
{
    public class BattleFieldFactory : IBattleFieldFactory
    {
        private const string MOVING_ARROW_PATH = "Arrows/MigrationArrow/MigrationArrow";
        
        private readonly IIdentifierService _identifierService;
        private readonly CommonStaticData _commonStaticData;
        private readonly IMigrationFactory _migrationFactory;
        private readonly GameContext _game;
        private EntityBehaviour _attackersHex, _defendersHex;
        private int _warriorsAmount;

        public BattleFieldFactory(IIdentifierService identifierService, CommonStaticData commonStaticData, IMigrationFactory migrationFactory)
        {
            _identifierService = identifierService;
            _commonStaticData = commonStaticData;
            _migrationFactory = migrationFactory;
            _game = Contexts.sharedInstance.game;
        }   
        
        public void SetAttackers(EntityBehaviour entityBehaviour, int selectedWarriors)
        {
            _attackersHex = entityBehaviour;
            _warriorsAmount = selectedWarriors;
        }

        public void TrySetDefendersAndCreateBattlefield(EntityBehaviour entityBehaviour)
        {
            if (_attackersHex == null)
            {
                _defendersHex = null;
                _warriorsAmount = 0;
                return;
            }
            
            _defendersHex = entityBehaviour;

            if (TryAddAttackersInExistBattlefield()) 
                return;

            if (IsHexagonsNotNeighbours())
            {
                CreateMigrationToBattlefield();
                return;
            }
            
            GameEntity battlefield = CreateBattlefield();
            CreateBattleArrow(battlefield);
            ResetFactoryState();
        }

        public void CreateBattlefieldFromWarriorsMigration(GameEntity migrationEntity)
        {
            GameEntity battlefield = _game.CreateEntity();
            battlefield.AddId(_identifierService.Next());
            battlefield.AddCurrentBattleCooldown(0f);
            battlefield.AddBattleCooldown(_commonStaticData.BattleCooldown);
            GameEntity defenderHex = _game.GetEntityWithId(migrationEntity.hexagonForAttack.Value);
            GameEntity attackerHex = _game.GetEntityWithId(migrationEntity.wayIdPoints.Value[0]);
            WarriorsContainer attackers = new WarriorsContainer(migrationEntity.warriorsMigrationAmount.Value, migrationEntity.wayIdPoints.Value[0]);
            WarriorsContainer defenders = new WarriorsContainer(defenderHex.warriorsAmount.Value, defenderHex.id.Value);
            battlefield.AddBattlefield(new List<WarriorsContainer>{ attackers }, defenders);
            attackerHex.warriorsAmount.Value -= migrationEntity.warriorsMigrationAmount.Value;
            defenderHex.warriorsAmount.Value = 0;
            CreateBattleArrowFromWarriorsMigration(battlefield, migrationEntity.wayIdPoints.Value[0], defenderHex.id.Value);
            ResetFactoryState();
        }

        private void CreateMigrationToBattlefield()
        {
            _migrationFactory.SetInitialHex(_attackersHex, _warriorsAmount, ManMigrationType.Warriors);
                
            EntityBehaviour neighbourHex = _migrationFactory.GetAwailableNeighbourHex(_defendersHex);

            if (neighbourHex != null)
            {
                GameEntity migration = _migrationFactory.SetFinishHexAndCreateMigration(neighbourHex);
                migration.AddHexagonForAttack(_defendersHex.Entity.id.Value);
            }
                
            ResetFactoryState();
        }

        private void CreateBattleArrowFromWarriorsMigration(GameEntity battlefield, int attackersHexId, int defendersHexId)
        {
            GameEntity battleArrow = _game.CreateEntity();
            battleArrow.AddViewPath(MOVING_ARROW_PATH);
            battleArrow.AddBattleArrow(battlefield.id.Value);
            battleArrow.AddWayIdPoints(new (){ attackersHexId, defendersHexId });
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
            battlefield.AddBattleCooldown(_commonStaticData.BattleCooldown);
            WarriorsContainer attackers = new WarriorsContainer(_warriorsAmount, _attackersHex.Entity.id.Value);
            WarriorsContainer defenders = new WarriorsContainer(_defendersHex.Entity.warriorsAmount.Value, _defendersHex.Entity.id.Value);
            battlefield.AddBattlefield(new List<WarriorsContainer>{ attackers }, defenders);
            _attackersHex.Entity.warriorsAmount.Value -= _warriorsAmount;
            _defendersHex.Entity.warriorsAmount.Value = 0;
            return battlefield;
        }

        private bool TryAddAttackersInExistBattlefield()
        {
            IGroup<GameEntity> entities = _game.GetGroup(GameMatcher.Battlefield);

            foreach (GameEntity entity in entities)
            {
                if (entity.battlefield.DefenderHexagonContainer.hexagonId == _defendersHex.Entity.id.Value)
                {
                    CreateBattleArrow(entity);
                    
                    entity.battlefield.AttackerHexagonContainers.Add(new WarriorsContainer(_warriorsAmount, _attackersHex.Entity.id.Value));
                    _attackersHex.Entity.warriorsAmount.Value -= _warriorsAmount;
                    ResetFactoryState();
                    return true;
                }
            }

            return false;
        }

        private bool IsHexagonsNotNeighbours()
        {
            if (_attackersHex.GetComponent<NeighboringHexagons>().NeighboringHexagonsList.Contains(_defendersHex))
                return false;
            
            return true;
        }

        private void ResetFactoryState()
        {
            _attackersHex = null;
            _defendersHex = null;
            _warriorsAmount = 0;
        }
    }
}