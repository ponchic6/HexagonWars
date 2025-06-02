using System.Collections.Generic;
using Code.Gameplay.Features.Building.DataStructure;
using Code.Infrastructure.View;
using TMPro;
using UnityEngine;
using Zenject;

namespace Code.Gameplay.Features.Building.View
{
    public class BuildingInfoPanel : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private TMP_Text _manAmount;
        [SerializeField] private TMP_Text _foodAmount;
        [SerializeField] private TMP_Text _ammoAmount;
        [SerializeField] private BuildingButton _buildingButtonPrefab;
        private Dictionary<BuildProgressContainer, BuildingButton> _buildingButtons = new();
        private EntityBehaviour _hexEntityBehaviour;
        private DiContainer _diContainer;

        public EntityBehaviour HexEntityBehaviour => _hexEntityBehaviour;
        public Dictionary<BuildProgressContainer, BuildingButton> BuildingButtons => _buildingButtons;

        [Inject]
        public void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        private void Update()
        {
            _manAmount.text = _hexEntityBehaviour.Entity.manAmount.Value.ToString();
            _foodAmount.text = _hexEntityBehaviour.Entity.foodAmount.Value.ToString("F1");
            _ammoAmount.text = _hexEntityBehaviour.Entity.ammoAmount.Value.ToString("F1");
        }

        public void Setup(EntityBehaviour hexEntityBehaviour)
        {
            _hexEntityBehaviour = hexEntityBehaviour;
            
            foreach (var kvp in _buildingButtons) 
                Destroy(kvp.Value.gameObject);
            
            _buildingButtons.Clear();
            UpdateBuildingProgress();
        }

        public void UpdateBuildingProgress()
        {
            foreach (BuildProgressContainer buildProgress in _hexEntityBehaviour.Entity.buildingProgress.Value)
            {
                if (!_buildingButtons.ContainsKey(buildProgress) && buildProgress.currentProgress < buildProgress.fullProgress)
                {
                    CreateBuildingButton(buildProgress);
                    continue;
                }

                if (!_buildingButtons.ContainsKey(buildProgress) && buildProgress.currentProgress >= buildProgress.fullProgress)
                {
                    CreateBuildingButton(buildProgress);
                    SetBuildedStatus(buildProgress);
                    continue;
                }

                if (_buildingButtons.ContainsKey(buildProgress) && buildProgress.currentProgress >= buildProgress.fullProgress)
                {
                    SetBuildedStatus(buildProgress);
                    continue;
                }

                if (_buildingButtons.ContainsKey(buildProgress) && buildProgress.currentProgress < buildProgress.fullProgress)
                {
                    UpdateBuildingButton(buildProgress);
                    continue;
                }
            }
        }
        
        private void CreateBuildingButton(BuildProgressContainer buildProgress)
        {
            BuildingButton buildingButton = _diContainer.InstantiatePrefabForComponent<BuildingButton>(_buildingButtonPrefab, _content);
            buildingButton.Setup(buildProgress, _hexEntityBehaviour);
            _buildingButtons.Add(buildProgress, buildingButton);
        }

        private void SetBuildedStatus(BuildProgressContainer buildProgress) => 
            _buildingButtons[buildProgress].SetBuildedStatus();

        private void UpdateBuildingButton(BuildProgressContainer buildProgress) =>
            _buildingButtons[buildProgress].UpdateUI(buildProgress);
    }
}