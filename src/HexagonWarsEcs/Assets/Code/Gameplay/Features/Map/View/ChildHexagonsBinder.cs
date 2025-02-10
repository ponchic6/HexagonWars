using System.Collections.Generic;
using Code.Gameplay.Features.Building;
using Code.Gameplay.Features.Building.DataStructure;
using Code.Infrastructure.Services;
using Code.Infrastructure.View;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Code.Gameplay.Features.Map.View
{
    public class ChildHexagonsBinder : EntityComponentRegistrar
    {
        [SerializeField] private List<EntityBehaviour> _hexagons;
        private IIdentifierService _identifierService;

        [Inject]
        public void Construct(IIdentifierService identifierService)
        {
            _identifierService = identifierService;
        }
        
        public override void RegisterComponent()
        {
            Entity.AddMapParent(_hexagons);

            var game = Contexts.sharedInstance.game;

            foreach (EntityBehaviour behaviour in _hexagons)
            {
                if (behaviour.Entity != null) 
                    continue;
                
                GameEntity entity = game.CreateEntity();
                entity.isChildHexagon = true;
                entity.AddId(_identifierService.Next());
                entity.AddTransform(behaviour.transform);
                entity.AddCitizensAmount(new Random().Next(1, 101));
                entity.AddFood(new Random().Next(1, 101));
                entity.AddBuildingProgress(new ()
                {
                    new BuildProgressContainer
                    {
                        fullProgress = 200,
                        currentProgress = 0,
                        buildingType = BuildingsType.LivingArea,
                        buildersAmount = 0,
                        ready = false
                    }
                });
                behaviour.SetEntity(entity);
            }
        }

        public override void UnregisterComponent() =>
            Entity.RemoveMapParent();
    }
}