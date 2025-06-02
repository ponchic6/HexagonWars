using System.Collections.Generic;
using Code.Infrastructure.View;

namespace Code.Gameplay.Features.Migration.Services
{
    public interface IMigrationFactory
    {
        public void SetInitialHex(EntityBehaviour value, int selectedPeople);
        public GameEntity SetFinishHexAndCreateMigration(EntityBehaviour value);
        public List<GameEntity> CreateMigrationViewTrail(EntityBehaviour entityBehaviour);
        public List<int> FindShortestPath(EntityBehaviour startNode, EntityBehaviour endNode);
        public EntityBehaviour InitialHex {get; }
        public EntityBehaviour FinishHex {get; }
    }
}