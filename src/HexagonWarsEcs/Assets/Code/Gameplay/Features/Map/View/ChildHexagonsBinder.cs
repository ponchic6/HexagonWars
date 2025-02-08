using System.Collections.Generic;
using Code.Infrastructure.Services;
using Code.Infrastructure.View;
using UnityEngine;
using Zenject;

namespace Code.Gameplay.Features.Map.View
{
    public class ChildHexagonsBinder : EntityComponentRegistrar
    {
        [SerializeField] private List<EntityBehaviour> _hexagons;
        private IIdentifierService _identifierService;

        public IReadOnlyList<EntityBehaviour> ChildHexagons => _hexagons;

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
                behaviour.SetEntity(entity);
            }
        }

        public override void UnregisterComponent() =>
            Entity.RemoveMapParent();
    }
}