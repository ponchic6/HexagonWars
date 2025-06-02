using Code.Gameplay.Features.Men;
using Code.Gameplay.Features.Men.Systems;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Code.Gameplay.Features.Citizens.Services
{
    public class ManModelFactory : IManModelFactory
    {
        private const string CITIZEN_PATH = "Models/CitizenModel";

        private readonly DiContainer _diContainer;
        private readonly GameContext _game;

        public ManModelFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
            _game = Contexts.sharedInstance.game;
        }

        public void TryCreateCitizen(int idHex)
        {
            GameEntity hexEntity = _game.GetEntityWithId(idHex);
            
            if (hexEntity.hasManAnimation) 
                return;

            Transform hexTransform = hexEntity.transform.Value;
            ManAnimationController citizenModel = _diContainer.InstantiatePrefabResourceForComponent<ManAnimationController>(CITIZEN_PATH, hexTransform);
            citizenModel.transform.localPosition = new Vector3(-0.15f, 0, 0.3f);
            citizenModel.transform.localRotation = Quaternion.Euler(90, 0, 0);
            hexEntity.AddManAnimation(ManAnimationType.Idle);
        }

        public void TryRemoveCitizen(int idHex)
        {
            GameEntity hexEntity = _game.GetEntityWithId(idHex);
            
            if (!hexEntity.hasManAnimation) 
                return;

            ManAnimationController controller = hexEntity.transform.Value.GetComponentInChildren<ManAnimationController>();
            Object.Destroy(controller.gameObject);
            hexEntity.RemoveManAnimation();
        }

        public void CreateAndMoveCitizenModel(GameEntity currentHex, GameEntity nextHex)
        {
            Vector3 finishPoint = nextHex.transform.Value.TransformPoint(new Vector3(0, 0, 0.3f));
            Transform hexTransform = _game.GetEntityWithId(currentHex.id.Value).transform.Value;
            
            ManAnimationController animationController =
                _diContainer.InstantiatePrefabResourceForComponent<ManAnimationController>(CITIZEN_PATH, hexTransform);
            animationController.transform.localPosition = new Vector3(-0.15f, 0, 0.3f);
            animationController.transform.localRotation = Quaternion.Euler(90, 0, 0);
            animationController.transform.LookAt(finishPoint);
            
            animationController.StartRun();
            animationController
                .transform
                .DOMove(finishPoint, 1f)
                .OnComplete(() => Object.Destroy(animationController.gameObject));
        }
    }
}