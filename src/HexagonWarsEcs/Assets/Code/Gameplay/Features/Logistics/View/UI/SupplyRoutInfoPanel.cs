using System;
using System.Collections.Generic;
using System.Linq;
using Code.Gameplay.Features.Logistics.DataStructure;
using Code.Infrastructure.View;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Gameplay.Features.Logistics.View.UI
{
    public class SupplyRoutInfoPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _couriersCountTmp;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _destroySupplyButton;
        [SerializeField] private TMP_Dropdown _dropdown;
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
            _couriersCountTmp.text = _entityBehaviour.Entity.couriersProgressList.Value.Count.ToString();
        }

        public void Setup(EntityBehaviour entityBehaviour)
        {
            _entityBehaviour = entityBehaviour;
            _dropdown.ClearOptions();
            _dropdown.AddOptions(new List<string>{LogisticResources.Food.ToString(), LogisticResources.Ammo.ToString()});
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
            Enum.TryParse(_dropdown.options[_dropdown.value].text, out LogisticResources logisticResources);
            entity.couriersProgressList.Value.AddRange(Enumerable.Repeat(new CurrentCourierProgress(), result).Select(_ => new CurrentCourierProgress(logisticResources)));
        }

        private void DestroySupplyRoute()
        {
            GameEntity entity = _entityBehaviour.Entity;
            GameEntity startHexEntity = _game.GetEntityWithId(entity.wayIdPoints.Value[0]);
            
            startHexEntity.ReplaceCitizensAmount(startHexEntity.citizensAmount.Value + entity.couriersProgressList.Value.Count);
            _entityBehaviour.Entity.isDestructed = true;
            gameObject.SetActive(false);
        }
    }
}
