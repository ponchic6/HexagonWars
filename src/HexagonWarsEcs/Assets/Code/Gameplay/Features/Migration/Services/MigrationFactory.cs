using System.Collections.Generic;
using System.Linq;
using Code.Gameplay.Common;
using Code.Gameplay.Features.Map.View;
using Code.Infrastructure.Services;
using Code.Infrastructure.View;
using UnityEngine;

namespace Code.Gameplay.Features.Migration.Services
{
    public class MigrationFactory : IMigrationFactory
    {
        private const string MOVING_ARROW_PATH = "Arrows/MigrationArrow/MigrationArrow";

        private readonly IIdentifierService _identifierService;
        private readonly GameContext _game;
        
        private EntityBehaviour _initialHex, _finishHex;
        private ManMigrationType _manMigrationType;
        private int _migrationAmount;

        public MigrationFactory(IIdentifierService identifierService)
        {
            _identifierService = identifierService;
            _game = Contexts.sharedInstance.game;
        }   
            
        public void SetInitialHex(EntityBehaviour entityBehaviour, int selectedPeople, ManMigrationType manMigrationType)
        {
            _manMigrationType = manMigrationType;
            _initialHex = entityBehaviour;
            _migrationAmount = selectedPeople;
        }

        public void SetFinishHexAndCreateMigration(EntityBehaviour entityBehaviour)
        {
            if (_initialHex == null)
            {
                _finishHex = null;
                _migrationAmount = 0;
                return;
            }
            
            _finishHex = entityBehaviour;

            List<int> shortestPath = FindShortestPath(_initialHex, _finishHex);

            if (shortestPath == null)
            {
                _initialHex = null;
                _finishHex = null;
                _migrationAmount = 0;
                return;
            }
            
            GameEntity migration = _game.CreateEntity();
            migration.AddId(_identifierService.Next());
            migration.AddWayIdPoints(shortestPath);
            migration.AddViewPath(MOVING_ARROW_PATH);
            migration.AddComplexityWay(Enumerable.Repeat(5f, shortestPath.Count - 1).ToList());
            migration.isMigrationArrow = true;

            switch (_manMigrationType)
            {
                case ManMigrationType.Citizens:
                    migration.AddCitizensMigrationAmount(_migrationAmount);
                    break;
                
                case ManMigrationType.Warriors:
                    migration.AddWarriorsMigrationAmount(_migrationAmount);
                    break;
            }

            _initialHex = null;
            _finishHex = null;
            _migrationAmount = 0;
        }
        
        private List<int> FindShortestPath(EntityBehaviour startNode, EntityBehaviour endNode)
        {
            if (startNode == null || endNode == null)
            {
                return null;
            }
    
            Queue<EntityBehaviour> queue = new Queue<EntityBehaviour>();
            queue.Enqueue(startNode);
    
            Dictionary<EntityBehaviour, EntityBehaviour> cameFrom = new Dictionary<EntityBehaviour, EntityBehaviour>();
            cameFrom[startNode] = null;
    
            while (queue.Count > 0)
            {
                EntityBehaviour current = queue.Dequeue();
        
                if (current == endNode)
                {
                    return ReconstructPath(cameFrom, endNode);
                }
        
                foreach (EntityBehaviour neighbor in current.GetComponent<NeighboringHexagons>().NeighboringHexagonsList)
                {
                    if (neighbor.Entity.isEnemyHexagon)
                    {
                        continue;
                    }

                    if (!cameFrom.ContainsKey(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        cameFrom[neighbor] = current;
                    }
                }
            }
            
            return null;
        }

        private List<int> ReconstructPath(Dictionary<EntityBehaviour, EntityBehaviour> cameFrom, EntityBehaviour endNode)
        {
            List<EntityBehaviour> path = new List<EntityBehaviour>();
            EntityBehaviour current = endNode;

            while (current != null)
            {
                path.Add(current);
                current = cameFrom[current];
            }

            path.Reverse();
            
            List<int> way = new();
            foreach (EntityBehaviour entityBehaviour in path)
            {
                way.Add(entityBehaviour.Entity.id.Value);
            }
    
            return way;
        }
    }
}