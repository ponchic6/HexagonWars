using UnityEngine;

namespace Code.Infrastructure.View
{
    public abstract class EntityComponentRegistrar : MonoBehaviour
    {
        [SerializeField] private EntityBehaviour _entityBehaviour;

        protected GameEntity Entity => _entityBehaviour.Entity;

        public abstract void RegisterComponent();
        public abstract void UnregisterComponent();
    }
}