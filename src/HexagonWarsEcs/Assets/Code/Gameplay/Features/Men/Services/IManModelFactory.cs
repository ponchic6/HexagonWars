using System.Collections.Generic;
using Code.Gameplay.Features.Men.Systems;

namespace Code.Gameplay.Features.Citizens.Services
{
    public interface IManModelFactory
    {
        public void TryCreateCitizen(int idHex);
        public void TryRemoveCitizen(int idHex);
        public void CreateAndMoveCitizenModel(GameEntity currentHex, GameEntity nextHex);
    }
}