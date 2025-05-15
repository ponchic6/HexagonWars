using System;
using Code.Infrastructure.View;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Gameplay.Features.Battle.View.UI
{
    public class BattleIndicatorController : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private EntityBehaviour _entityBehaviour;
        [SerializeField] private RectTransform _arrow;
        [SerializeField] private Image _imageArrow;
        [SerializeField] private TMP_Text _text;
        private Camera _mainCamera;
        private GameContext _game;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _game = Contexts.sharedInstance.game;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            GameEntity battleEntity = _game.GetEntityWithId(_entityBehaviour.Entity.battleIndicator.BattleId);
            
            if (battleEntity.battlefield.AttackerHexagonsId.Count == 1)
            {
                battleEntity.isDestructed = true;
                _entityBehaviour.Entity.isDestructed = true;
            }
            else
            {
                battleEntity.battlefield.AttackerHexagonsId.RemoveAll(x => x == _entityBehaviour.Entity.battleIndicator.FromHexId);
                _entityBehaviour.Entity.isDestructed = true;
            }
        }

        public void SetDirection(Transform from, Transform to)
        {
            Vector3 fromScreenPoint = _mainCamera.WorldToScreenPoint(from.position);
            Vector3 toScreenPoint = _mainCamera.WorldToScreenPoint(to.position);
            Vector3 directionToTarget = (toScreenPoint - fromScreenPoint).normalized;
            _arrow.localRotation = Quaternion.LookRotation(Vector3.forward, directionToTarget);
        }

        public void SetPosition(Transform from, Transform to) =>
            transform.position = (from.position + to.position) / 2 + Vector3.up * 0.5f;

        public void SetBattleStatus(float winIndicator)
        {
            _text.text = Math.Round(winIndicator, 2).ToString();
            
            if (winIndicator > 0.5f)
                _imageArrow.color = Color.green;
            else
                _imageArrow.color = Color.red;
        }
    }
}
