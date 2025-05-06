using System.Collections.Generic;
using Code.Gameplay.Features.Migration.View;

namespace Code.Gameplay.Features.Citizens.Services
{
    public interface ICitizensModelFactory
    {
        public void TryCreateCitizen(int idHex);
        public void TryRemoveCitizen(int idHex);
        public void CreateAndMoveCitizenModel(GameEntity currentHex, GameEntity nextHex);
        public Dictionary<int, CitizenAnimationController> HexWithCitizens { get; }
    }
}