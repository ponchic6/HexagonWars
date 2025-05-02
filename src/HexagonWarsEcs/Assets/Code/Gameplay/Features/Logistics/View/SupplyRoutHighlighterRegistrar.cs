using Code.Infrastructure.View;
using UnityEngine;

namespace Code.Gameplay.Features.Logistics.View
{
    public class SupplyRoutHighlighterRegistrar : EntityComponentRegistrar
    {
        [SerializeField] private SupplyRoutHighlighter _highlighter;
        
        public override void RegisterComponent() =>
            Entity.AddSupplyHighlighter(_highlighter);

        public override void UnregisterComponent() =>
            Entity.RemoveSupplyHighlighter();
    }
}