using UnityEngine;
using Zenject;

namespace Code.Infrastructure.View
{
    public class EntityViewFactory : IEntityViewFactory
    {
        private DiContainer _diContainer;

        public EntityViewFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        public EntityBehaviour CreateViewForEntityFromPath(GameEntity gameEntity)
        {
            var view = _diContainer.InstantiatePrefabResource(gameEntity.viewPath.Value);
            var entityBehaviour = view.GetComponent<EntityBehaviour>();
            entityBehaviour.SetEntity(gameEntity);
            return entityBehaviour;
        }
        
        public EntityBehaviour CreateViewForEntityFromPrefab(GameEntity gameEntity)
        {
            GameObject view;

            if (gameEntity.hasViewPrefabWithParent)
                view = _diContainer.InstantiatePrefab(gameEntity.viewPrefabWithParent.Value, gameEntity.viewPrefabWithParent.Parent.transform);
            else
                view = _diContainer.InstantiatePrefab(gameEntity.viewPrefab.Value);
            
            var entityBehaviour = view.GetComponent<EntityBehaviour>();
            entityBehaviour.SetEntity(gameEntity);
            return entityBehaviour;
        }
    }
}