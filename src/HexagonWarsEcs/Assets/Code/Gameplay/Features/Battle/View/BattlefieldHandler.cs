using Code.Gameplay.Features.Battle.Services;
using Code.Gameplay.Features.Migration.Services;
using Code.Gameplay.Features.Migration.View.UI;
using Code.Infrastructure.View;
using Logic.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code.Gameplay.Features.Battle.View
{
    public class BattlefieldHandler : MonoBehaviour
    {
        [SerializeField] private EntityBehaviour _entityBehaviour;
        [SerializeField] private PointerHandler _pointerHandler;
        private MigrationAllSlidersBlocker _allSlidersBlocker;
        private IBattleFieldFactory _battleFieldFactory;

        [Inject]
        public void Construct(IBattleFieldFactory battleFieldFactory)
        {
            _battleFieldFactory = battleFieldFactory;
        }

        private void Awake()
        {
            _allSlidersBlocker = GetComponentInParent<MigrationAllSlidersBlocker>();
            _pointerHandler.OnPointerDownEvent += OnPointerDown;
        }

        private void OnDisable()
        {
            _pointerHandler.OnPointerDownEvent -= OnPointerDown;
        }

        private void OnPointerDown(PointerEventData eventData)
        {
            if (!_entityBehaviour.Entity.isEnemyHexagon)
                return;
            
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                _battleFieldFactory.SetDefendersAndCreateBattlefield(_entityBehaviour);
                _allSlidersBlocker.isSlidersBlocked.Value = false;
            }
        }
    }
}