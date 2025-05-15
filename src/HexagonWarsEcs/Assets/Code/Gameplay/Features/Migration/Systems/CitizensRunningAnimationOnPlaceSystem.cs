using Code.Gameplay.Features.Citizens.Services;
using Code.Gameplay.Features.Migration.View;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Migration.Systems
{
    public class CitizensRunningAnimationOnPlaceSystem : IExecuteSystem
    {
        private readonly ICitizensModelFactory _citizensModelFactory;
        private readonly IGroup<GameEntity> _entities;
        private readonly GameContext _game;

        public CitizensRunningAnimationOnPlaceSystem(ICitizensModelFactory citizensModelFactory)
        {
            _citizensModelFactory = citizensModelFactory;
            
            _game = Contexts.sharedInstance.game;
            _entities = _game.GetGroup(GameMatcher.AllOf(GameMatcher.MigrationComplexityWay, GameMatcher.CitizensMigrationAmount));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                _citizensModelFactory.HexWithCitizens.TryGetValue(entity.wayIdPoints.Value[0], out CitizenAnimationController animationController);

                if (animationController == null)
                    continue;
                
                if (entity.migrationComplexityWay.Value.Count > 0 && entity.migrationComplexityWay.Value[0] < 0)
                {
                    animationController.StartIdle();
                    animationController.transform.localRotation = Quaternion.Euler(90, 0, 0);
                    continue;
                }

                if (!animationController.Animator.GetCurrentAnimatorStateInfo(0).IsName("Run") &&
                    entity.migrationComplexityWay.Value.Count > 0)
                {
                    GameEntity nextHex = _game.GetEntityWithId(entity.wayIdPoints.Value[1]);
                    Vector3 finishPoint = nextHex.transform.Value.TransformPoint(new Vector3(0, 0, 0.3f));
                    animationController.transform.LookAt(finishPoint);
                    animationController.StartRun();
                }
            }
        }
    }
}