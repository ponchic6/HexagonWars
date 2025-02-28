using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Code.Gameplay.Features.Citizens.Services
{
    public class CitizensModelFactory : ICitizensModelFactory
    {
        private const string CITIZEN_PATH = "Models/CitizenModel";

        private readonly DiContainer _diContainer;
        private readonly GameContext _game;
        private readonly HashSet<int> _hexWithCitizens = new();

        public CitizensModelFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
            _game = Contexts.sharedInstance.game;
        }
        
        public void TryCreateIdleCitizen(int idHex)
        {
            if (_hexWithCitizens.Contains(idHex)) 
                return;

            Transform hexTransform = _game.GetEntityWithId(idHex).transform.Value;
            _hexWithCitizens.Add(idHex);
            
            _diContainer.InstantiatePrefabResource(CITIZEN_PATH, new Vector3(0, 0.3f, 0), Quaternion.Euler(0, 0, 0), hexTransform);
        }

        public void TryRemoveIdleCitizen(int idHex)
        {
            
        }
    }
}