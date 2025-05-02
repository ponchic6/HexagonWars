using System;
using Code.Gameplay.Features.Logistics.DataStructure;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Gameplay.Features.Logistics.View.UI
{
    public class SupplyRoutUiView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_InputField _couriersInputField;
        [SerializeField] private Button _plus;
        [SerializeField] private Button _minus;
        [SerializeField] private Button _deleteButton;
        [SerializeField] private TMP_Dropdown _resourcesDropdown;
        [SerializeField] private TMP_Text _resourceAmountAtStartHex;
        private GameContext _gameContext;
        private GameEntity _routEntity;
        private LogisticResources _dropDownValue;

        private void Awake() =>
            _gameContext = Contexts.sharedInstance.game;

        public void Setup(GameEntity routEntity)
        {
            _plus.onClick.AsObservable().Subscribe(_ => OnPlusButton(routEntity)).AddTo(this);
            _minus.onClick.AsObservable().Subscribe(_ => OnMinusButton(routEntity)).AddTo(this);
            _deleteButton.onClick.AsObservable().Subscribe(_ => OnDeleteButton(routEntity)).AddTo(this);

            _couriersInputField.text = routEntity.couriersProgressList.Value.Count.ToString();

            _resourcesDropdown.onValueChanged.AsObservable().Subscribe(i => OnValueChanged(i, routEntity)).AddTo(this);
            _resourcesDropdown.options.Add(new TMP_Dropdown.OptionData(nameof(LogisticResources.Food)));
            _resourcesDropdown.options.Add(new TMP_Dropdown.OptionData(nameof(LogisticResources.Ammo)));
            
            _routEntity = routEntity;
        }
        
        public void OnPointerEnter(PointerEventData eventData) =>
            _routEntity.isHighlightedSupplyRout = true;

        public void OnPointerExit(PointerEventData eventData) => 
            _routEntity.isHighlightedSupplyRout = false;

        public void UpdateResourceAmountAtStartHex()
        {
            GameEntity startHex = _gameContext.GetEntityWithId(_routEntity.wayIdPoints.Value[0]);

            switch (_dropDownValue)
            {
                case LogisticResources.Food:
                    _resourceAmountAtStartHex.text = startHex.foodAmount.Value.ToString(); 
                    break;
                
                case LogisticResources.Ammo:
                    _resourceAmountAtStartHex.text = startHex.ammoAmount.Value.ToString();
                    break;
            }
        }

        private void OnValueChanged(int i, GameEntity routEntity)
        {
            TMP_Dropdown.OptionData optionData = _resourcesDropdown.options[i];
            Enum.TryParse(optionData.text, out LogisticResources resource);
            _dropDownValue = resource;
            
            foreach (CurrentCourierProgress courierProgress in routEntity.couriersProgressList.Value) 
                courierProgress.logisticResources = resource;
        }

        private void OnPlusButton(GameEntity routEntity)
        {
            GameEntity startHexEntity = _gameContext.GetEntityWithId(routEntity.wayIdPoints.Value[0]);
            
            if (startHexEntity.citizensAmount.Value <= 0)
                return;
            
            startHexEntity.citizensAmount.Value--;
            var value = _resourcesDropdown.value;
            TMP_Dropdown.OptionData optionData = _resourcesDropdown.options[value];
            Enum.TryParse(optionData.text, out LogisticResources resource);
            routEntity.couriersProgressList.Value.Add(new CurrentCourierProgress(resource));
            _couriersInputField.text = routEntity.couriersProgressList.Value.Count.ToString();
        }

        private void OnMinusButton(GameEntity routEntity)
        {
            GameEntity startHexEntity = _gameContext.GetEntityWithId(routEntity.wayIdPoints.Value[0]);
            
            if (routEntity.couriersProgressList.Value.Count == 0)
                return;
            
            startHexEntity.citizensAmount.Value++;
            routEntity.couriersProgressList.Value.RemoveAt(0);
            _couriersInputField.text = routEntity.couriersProgressList.Value.Count.ToString();
        }

        private void OnDeleteButton(GameEntity routEntity)
        {
            GameEntity startHexEntity = _gameContext.GetEntityWithId(routEntity.wayIdPoints.Value[0]);
            startHexEntity.citizensAmount.Value += routEntity.couriersProgressList.Value.Count;
            routEntity.isDestructed = true;
        }
    }
}