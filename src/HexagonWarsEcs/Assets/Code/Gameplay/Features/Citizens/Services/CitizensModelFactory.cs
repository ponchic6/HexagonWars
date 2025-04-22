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
        private readonly Dictionary<int, GameObject> _hexWithCitizens = new();

        public CitizensModelFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
            _game = Contexts.sharedInstance.game;
        }
        
        public void TryCreateIdleCitizen(int idHex)
        {
            if (_hexWithCitizens.ContainsKey(idHex)) 
                return;

            Transform hexTransform = _game.GetEntityWithId(idHex).transform.Value;
            GameObject citizenModel = _diContainer.InstantiatePrefabResource(CITIZEN_PATH, hexTransform);
            citizenModel.transform.localPosition = new Vector3(0, 0, 0.3f);
            citizenModel.transform.localRotation = Quaternion.Euler(90, 0, 0);
            
            _hexWithCitizens.Add(idHex, citizenModel);
        }

        public void TryRemoveIdleCitizen(int idHex)
        {
            if (!_hexWithCitizens.ContainsKey(idHex)) 
                return;
            
            _hexWithCitizens.Remove(idHex, out GameObject citizenModel);
            Object.Destroy(citizenModel);
        }
    }
}