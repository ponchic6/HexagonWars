using System;
using Code.Gameplay.Features.Logistics.Services;
using Code.Infrastructure.View;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.Gameplay.Features.Logistics.View.UI
{
    public class SupplyRoutInfoPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _couriersCountTmp;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _destroySupplyButton;
        private Action _onDestroyRoute;
        private EntityBehaviour _entityBehaviour;
        private GameContext _game;
        
        private void Awake()
        {
            _inputField.onSubmit.AsObservable().Subscribe(OnSubmit).AddTo(this);
            _destroySupplyButton.onClick.AsObservable().Subscribe(_ => DestroySupplyRoute()).AddTo(this);
            _game = Contexts.sharedInstance.game;
        }

        private void Update()
        {
            _couriersCountTmp.text = _entityBehaviour.Entity.couriersAmount.Value.ToString();
        }

        public void Setup(EntityBehaviour entityBehaviour)
        {
            _entityBehaviour = entityBehaviour;
        }

        private void OnSubmit(string input)
        {
            GameEntity entity = _entityBehaviour.Entity;
            GameEntity startHexEntity = _game.GetEntityWithId(entity.wayIdPoints.Value[0]);

            if (!int.TryParse(input, out int result)) 
                return;
            
            if (startHexEntity.citizensAmount.Value < result) 
                return;
            
            _inputField.text = string.Empty;
            startHexEntity.ReplaceCitizensAmount(startHexEntity.citizensAmount.Value - result);
            entity.ReplaceCouriersAmount(result);
        }

        private void DestroySupplyRoute()
        {
            GameEntity entity = _entityBehaviour.Entity;
            GameEntity startHexEntity = _game.GetEntityWithId(entity.wayIdPoints.Value[0]);
            
            startHexEntity.ReplaceCitizensAmount(startHexEntity.citizensAmount.Value + entity.couriersAmount.Value);
            _entityBehaviour.Entity.isDestructed = true;
            gameObject.SetActive(false);
        }
    }
}
