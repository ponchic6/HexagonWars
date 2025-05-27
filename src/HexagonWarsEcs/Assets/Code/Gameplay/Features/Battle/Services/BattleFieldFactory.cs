using System.Collections.Generic;
using Code.Gameplay.Common;
using Code.Gameplay.Features.Map.View;
using Code.Gameplay.Features.Migration.Services;
using Code.Infrastructure.Services;
using Code.Infrastructure.StaticData;
using Code.Infrastructure.View;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Battle.Services
{
    public class BattleFieldFactory : IBattleFieldFactory
    {
        private const string BATTLE_INDICATOR_PATH = "Hexagons/UI/BattleIndicator";
        
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
            
            if (IsHexagonsNotNeighbours())
            {
                CreateMigrationToBattlefield();
                return;
            }

            if (TryAddAttackersInExistBattlefield())
                return;
            
            CreateBattlefield(_attackersHex.Entity, _defendersHex.Entity);
            ResetFactoryState();
        }

        public void CreateBattlefieldFromWarriorsMigration(GameEntity migrationEntity)
        {
            GameEntity defenderHex = _game.GetEntityWithId(migrationEntity.hexagonForAttack.Value);
            GameEntity attackerHex = _game.GetEntityWithId(migrationEntity.wayIdPoints.Value[0]);
            CreateBattlefield(attackerHex, defenderHex);
            ResetFactoryState();
        }

        public GameEntity CreateBattlefield(GameEntity attackerHex, GameEntity defenderHex)
        {
            GameEntity battlefield = _game.CreateEntity();
            battlefield.AddId(_identifierService.Next());
            battlefield.AddCurrentBattleCooldown(0f);
            battlefield.AddBattleCooldown(_commonStaticData.BattleCooldown);
            battlefield.AddBattlefield(new List<int>{ attackerHex.id.Value }, defenderHex.id.Value);
            CreateBattleIndicator(attackerHex, defenderHex, battlefield);
            return battlefield;
        }

        private void CreateMigrationToBattlefield()
        {
            _migrationFactory.SetInitialHex(_attackersHex, _warriorsAmount, ManMigrationType.Warriors);
            EntityBehaviour neighbourHex = null;
            
            List<EntityBehaviour> neighboringHexagonsList = _defendersHex.GetComponent<NeighboringHexagons>().NeighboringHexagonsList;
            
            foreach (EntityBehaviour entity in neighboringHexagonsList)
            {
                List<int> findShortestPath = _migrationFactory.FindShortestPath(_attackersHex, entity);
                
                if (findShortestPath != null)
                    neighbourHex = entity;
            }

            if (neighbourHex != null)
            {
                GameEntity migration = _migrationFactory.SetFinishHexAndCreateMigration(neighbourHex);
                migration.AddHexagonForAttack(_defendersHex.Entity.id.Value);
            }
                
            ResetFactoryState();
        }
        
        private void CreateBattleIndicator(GameEntity fromHex, GameEntity toHex, GameEntity battlefieldEntity)
        {
            GameEntity battleIndicator = _game.CreateEntity();
            battleIndicator.AddId(_identifierService.Next());
            battleIndicator.AddBattleIndicator(fromHex.id.Value, toHex.id.Value, 0, battlefieldEntity.id.Value);
            battleIndicator.AddViewPath(BATTLE_INDICATOR_PATH);
        }

        private bool TryAddAttackersInExistBattlefield()
        {
            IGroup<GameEntity> entities = _game.GetGroup(GameMatcher.Battlefield);

            foreach (GameEntity battlefieldEntity in entities)
            {
                if (battlefieldEntity.battlefield.DefenderHexagonId != _defendersHex.Entity.id.Value)
                    continue;
                
                battlefieldEntity.battlefield.AttackerHexagonsId.Add(_attackersHex.Entity.id.Value);
                battlefieldEntity.ReplaceBattlefield(battlefieldEntity.battlefield.AttackerHexagonsId, battlefieldEntity.battlefield.DefenderHexagonId);
                CreateBattleIndicator(_attackersHex.Entity, _defendersHex.Entity, battlefieldEntity);
                ResetFactoryState();
                return true;
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