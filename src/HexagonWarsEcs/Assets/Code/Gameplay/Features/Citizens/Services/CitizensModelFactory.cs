using System.Collections.Generic;
using Code.Gameplay.Features.Migration.View;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Code.Gameplay.Features.Citizens.Services
{
    public class CitizensModelFactory : ICitizensModelFactory
    {
        private const string CITIZEN_PATH = "Models/CitizenModel";

        private readonly DiContainer _diContainer;
        private readonly GameContext _game;
        private readonly Dictionary<int, CitizenAnimationController> _hexWithCitizens = new();

        public Dictionary<int, CitizenAnimationController> HexWithCitizens => _hexWithCitizens;

        public CitizensModelFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
            _game = Contexts.sharedInstance.game;
        }

        public void TryCreateCitizen(int idHex)
        {
            if (_hexWithCitizens.ContainsKey(idHex)) 
                return;

            Transform hexTransform = _game.GetEntityWithId(idHex).transform.Value;
            CitizenAnimationController citizenModel = _diContainer.InstantiatePrefabResourceForComponent<CitizenAnimationController>(CITIZEN_PATH, hexTransform);
            citizenModel.transform.localPosition = new Vector3(-0.15f, 0, 0.3f);
            citizenModel.transform.localRotation = Quaternion.Euler(90, 0, 0);
            
            _hexWithCitizens.Add(idHex, citizenModel);
        }

        public void TryRemoveCitizen(int idHex)
        {
            if (!_hexWithCitizens.ContainsKey(idHex)) 
                return;
            
            _hexWithCitizens.Remove(idHex, out CitizenAnimationController citizenModel);
            Object.Destroy(citizenModel.gameObject);
        }

        public void CreateAndMoveCitizenModel(GameEntity currentHex, GameEntity nextHex)
        {
            Vector3 finishPoint = nextHex.transform.Value.TransformPoint(new Vector3(0, 0, 0.3f));
            Transform hexTransform = _game.GetEntityWithId(currentHex.id.Value).transform.Value;
            
            CitizenAnimationController animationController =
                _diContainer.InstantiatePrefabResourceForComponent<CitizenAnimationController>(CITIZEN_PATH, hexTransform);
            animationController.transform.localPosition = new Vector3(-0.15f, 0, 0.3f);
            animationController.transform.localRotation = Quaternion.Euler(90, 0, 0);
            animationController.transform.LookAt(finishPoint);
            
            animationController.StartRun();
            animationController.transform
                .DOMove(finishPoint, 1f)
                .OnComplete(() => Object.Destroy(animationController.gameObject));
        }
    }
}