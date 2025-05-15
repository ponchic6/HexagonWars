using Code.Gameplay.Features.Battle.View.UI;
using Code.Infrastructure.View;
using UnityEngine;

namespace Code.Gameplay.Features.Battle.Registrars
{
    public class BattleIndicatorControllerRegistrar : EntityComponentRegistrar
    {
        [SerializeField] private BattleIndicatorController _battleIndicator;
        
        public override void RegisterComponent() =>
            Entity.AddBattleIndicatorController(_battleIndicator);

        public override void UnregisterComponent() =>
            Entity.RemoveBattleIndicatorController();
    }
}