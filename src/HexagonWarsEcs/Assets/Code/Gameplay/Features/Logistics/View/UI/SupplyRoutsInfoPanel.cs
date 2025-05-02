using System;
using System.Collections.Generic;
using System.Linq;
using Code.Infrastructure.View;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Logistics.View.UI
{
    public class SupplyRoutsInfoPanel : MonoBehaviour
    {
        [SerializeField] private SupplyRoutUiView _supplyRoutUiViewPrefab;
        [SerializeField] private RectTransform _content;
        private Dictionary<int, SupplyRoutUiView> _supplyRoutUiViews = new();
        private GameContext _game;
        private EntityBehaviour _hexEntityBehaviour;

        public EntityBehaviour HexEntityBehaviour => _hexEntityBehaviour;

        private void Awake() => 
            _game = Contexts.sharedInstance.game;
        
        public void Setup(EntityBehaviour hexEntityBehaviour)
        {
            _hexEntityBehaviour = hexEntityBehaviour;
            
            foreach (Transform contentElement in _content.transform) 
                Destroy(contentElement.gameObject);
            
            _supplyRoutUiViews.Clear();

            IGroup<GameEntity> group = _game.GetGroup(GameMatcher.SupplyRoute);

            foreach (GameEntity routEntity in group)
            {
                if (routEntity.wayIdPoints.Value.Last() == hexEntityBehaviour.Entity.id.Value) 
                    AddSupplyRout(routEntity);
            }
        }

        public void AddSupplyRout(GameEntity routEntity)
        {
            SupplyRoutUiView instance = Instantiate(_supplyRoutUiViewPrefab, _content);
            instance.Setup(routEntity);
            _supplyRoutUiViews.Add(routEntity.id.Value, instance);
        }

        public void RemoveSupplyRout(GameEntity routEntity)
        {
            Destroy(_supplyRoutUiViews[routEntity.id.Value].gameObject);
            _supplyRoutUiViews.Remove(routEntity.id.Value);
        }

        public void UpdateResourceAtStartHex()
        {
            foreach (KeyValuePair<int, SupplyRoutUiView> kvp in _supplyRoutUiViews) 
                kvp.Value.UpdateResourceAmountAtStartHex();
        }
    }
}
