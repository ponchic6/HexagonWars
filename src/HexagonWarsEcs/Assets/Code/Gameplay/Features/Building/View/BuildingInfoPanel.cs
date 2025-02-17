using System;
using System.Collections.Generic;
using System.Linq;
using Code.Gameplay.Common.View.UI;
using Code.Gameplay.Features.Building.DataStructure;
using Code.Infrastructure.View;
using TMPro;
using UnityEngine;

namespace Code.Gameplay.Features.Building.View
{
    public class BuildingInfoPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _citizensAmount;
        [SerializeField] private TMP_Text _foodAmount;
        [SerializeField] private TMP_Text _ammoAmount;
        [SerializeField] private RectTransform _content;
        [SerializeField] private SlidersGroup _slidersGroup;
        [SerializeField] private BuildingButton _buildingButtonPrefab;

        private EntityBehaviour _entityBehaviour;
        private Dictionary<BuildProgressContainer, BuildingButton> _buildingButtons = new();

        public EntityBehaviour EntityBehaviour => _entityBehaviour;

        private void Update()
        {
            _citizensAmount.text = _entityBehaviour.Entity.citizensAmount.Value.ToString();
            _foodAmount.text = _entityBehaviour.Entity.foodAmount.Value.ToString("F1");
            _ammoAmount.text = _entityBehaviour.Entity.ammoAmount.Value.ToString("F1");
        }

        public void Setup(EntityBehaviour entityBehaviour)
        {
            foreach (var kvp in _buildingButtons)
            {
                Destroy(kvp.Value.gameObject);
            }

            _slidersGroup.ClearSliders();
            _buildingButtons.Clear();
            _entityBehaviour = entityBehaviour;
        }

        public void UpdateBuildersStates(GameEntity entity)
        {
            foreach (BuildProgressContainer buildProgress in entity.buildingProgress.Value)
            {
                if (_buildingButtons.ContainsKey(buildProgress) && buildProgress.currentProgress >= buildProgress.fullProgress)
                    DeleteBuildingButton(buildProgress);
                    
                if (!_buildingButtons.ContainsKey(buildProgress) && buildProgress.currentProgress < buildProgress.fullProgress)
                    CreateBuildingButton(buildProgress);
                    
                if (_buildingButtons.ContainsKey(buildProgress) && buildProgress.currentProgress < buildProgress.fullProgress)
                    UpdateBuildingButton(buildProgress);
            }
        }
        
        private void CreateBuildingButton(BuildProgressContainer buildProgress)
        {
            var buildingButton = Instantiate(_buildingButtonPrefab, _content);
            buildingButton.Setup(buildProgress, _entityBehaviour);
            _buildingButtons.Add(buildProgress, buildingButton);
            _slidersGroup.Add(buildingButton.Slider);
            
            buildingButton.OnCancelButton += _ => ResetBuildingProgress(buildProgress, buildingButton);
            buildingButton.OnSliderValueChanged += _=> UpdateBuildersAmount();
        }

        private void DeleteBuildingButton(BuildProgressContainer buildProgress)
        {
            _slidersGroup.Remove(_buildingButtons[buildProgress].Slider);
            Destroy(_buildingButtons[buildProgress].gameObject);
            _buildingButtons.Remove(buildProgress);
        }

        private void UpdateBuildingButton(BuildProgressContainer buildProgress)
        {
            _buildingButtons[buildProgress].UpdateUI(buildProgress);
        }

        private void UpdateBuildersAmount()
        {
            var totalAmount = _buildingButtons.Keys.Sum(x => x.buildersAmount) +
                              _entityBehaviour.Entity.citizensAmount.Value;
                
            foreach (var containerButtonPair in _buildingButtons)
            {
                containerButtonPair.Key.buildersAmount = (int)Math.Round(totalAmount * containerButtonPair.Value.Slider.value);
            }

            int restCitizens = 0;

            foreach (var containerButtonPair in _buildingButtons)
            {
                restCitizens -= containerButtonPair.Key.buildersAmount;
            }

            restCitizens += totalAmount;
                
            _entityBehaviour.Entity.ReplaceCitizensAmount(restCitizens);
        }

        private void ResetBuildingProgress(BuildProgressContainer buildProgress, BuildingButton buildingButton)
        {
            int citizensAmount = _entityBehaviour.Entity.citizensAmount.Value;
            int buildersAmount = buildProgress.buildersAmount;
            int totalAmount = citizensAmount + buildersAmount;
                
            _entityBehaviour.Entity.ReplaceCitizensAmount(totalAmount);

            buildProgress.buildersAmount = 0;
            buildProgress.currentProgress = 0;
            buildingButton.Slider.value = 0;
        }
    }
}