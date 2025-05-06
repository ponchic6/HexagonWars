using System.Collections.Generic;
using Code.Gameplay.Features.Warriors.View;

namespace Code.Gameplay.Features.Warriors.Services
{
    public interface ISoldiersModelFactory
    {
        public void TryCreateSoldier(int idHex);
        public void TryRemoveSoldier(int idHex);
        public void CreateAndMoveSoldierModel(GameEntity currentHex, GameEntity nextHex);
        public Dictionary<int, SoldierAnimationController> HexWithSoldiers { get; }
    }
} 