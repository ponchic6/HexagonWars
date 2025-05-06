using System.Collections.Generic;
using Code.Gameplay.Features.Warriors.View;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Code.Gameplay.Features.Warriors.Services
{
    public class SoldiersModelFactory : ISoldiersModelFactory
    {
        private const string SOLDIER_PATH = "Models/SoldierModel";

        private readonly DiContainer _diContainer;
        private readonly GameContext _game;
        private readonly Dictionary<int, SoldierAnimationController> _hexWithSoldiers = new();

        public Dictionary<int, SoldierAnimationController> HexWithSoldiers => _hexWithSoldiers;

        public SoldiersModelFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
            _game = Contexts.sharedInstance.game;
        }

        public void TryCreateSoldier(int idHex)
        {
            if (_hexWithSoldiers.ContainsKey(idHex)) 
                return;

            Transform hexTransform = _game.GetEntityWithId(idHex).transform.Value;
            SoldierAnimationController soldierModel = _diContainer.InstantiatePrefabResourceForComponent<SoldierAnimationController>(SOLDIER_PATH, hexTransform);
            soldierModel.transform.localPosition = new Vector3(0.15f, 0, 0.3f);
            soldierModel.transform.localRotation = Quaternion.Euler(90, 0, 0);
            
            _hexWithSoldiers.Add(idHex, soldierModel);
        }

        public void TryRemoveSoldier(int idHex)
        {
            if (!_hexWithSoldiers.ContainsKey(idHex)) 
                return;
            
            _hexWithSoldiers.Remove(idHex, out SoldierAnimationController soldierModel);
            Object.Destroy(soldierModel.gameObject);
        }

        public void CreateAndMoveSoldierModel(GameEntity currentHex, GameEntity nextHex)
        {
            Vector3 finishPoint = nextHex.transform.Value.TransformPoint(new Vector3(0, 0, 0.3f));
            Transform hexTransform = _game.GetEntityWithId(currentHex.id.Value).transform.Value;
            
            SoldierAnimationController animationController =
                _diContainer.InstantiatePrefabResourceForComponent<SoldierAnimationController>(SOLDIER_PATH, hexTransform);
            animationController.transform.localPosition = new Vector3(0.15f, 0, 0.3f);
            animationController.transform.localRotation = Quaternion.Euler(90, 0, 0);
            animationController.transform.LookAt(finishPoint);
            
            animationController.StartRun();
            animationController.transform
                .DOMove(finishPoint, 1f)
                .OnComplete(() => Object.Destroy(animationController.gameObject));
        }
    }
} 