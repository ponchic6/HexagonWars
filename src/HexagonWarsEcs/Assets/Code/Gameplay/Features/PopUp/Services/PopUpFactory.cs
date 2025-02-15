using Code.Gameplay.Features.PopUp.View;
using UnityEngine;
using Zenject;

namespace Code.Gameplay.Features.PopUp.Services
{
    public class PopUpFactory : IPopUpFactory
    {
        private const string POPUP_PATH = "Hexagons/UI/ResourcePopUp";
        
        private readonly DiContainer _diContainer;

        public PopUpFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        public void CreatePopUp(Sprite sprite, float count, Transform transform)
        {
            ResourcePopUpConfigurator resourcePopUpConfigurator =
                _diContainer.InstantiatePrefabResourceForComponent<ResourcePopUpConfigurator>(POPUP_PATH, transform.GetComponentInChildren<Canvas>().transform);
            resourcePopUpConfigurator.Setup(sprite, count, transform.position);
        }
    }
}