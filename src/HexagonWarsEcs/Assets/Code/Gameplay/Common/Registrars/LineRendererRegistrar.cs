using Code.Infrastructure.View;
using UnityEngine;

namespace Code.Gameplay.Common.Registrars
{
    public class LineRendererRegistrar : EntityComponentRegistrar
    {
        [SerializeField] private LineRenderer _lineRenderer;

        public override void RegisterComponent() =>
            Entity.AddLineRenderer(_lineRenderer);

        public override void UnregisterComponent() =>
            Entity.RemoveLineRenderer();
    }
}