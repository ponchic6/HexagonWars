using Code.Infrastructure.View;
using UnityEngine;

namespace Code.Gameplay.Common.Registrars
{
    public class RendererRegistrar : EntityComponentRegistrar
    {
        [SerializeField] private Renderer _renderer;

        public override void RegisterComponent() =>
            Entity.AddRenderer(_renderer);

        public override void UnregisterComponent() =>
            Entity.RemoveRenderer();
    }
}