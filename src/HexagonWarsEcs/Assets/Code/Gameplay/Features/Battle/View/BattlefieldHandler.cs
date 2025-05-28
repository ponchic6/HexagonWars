using Code.Gameplay.Common.Services;
using Code.Gameplay.Common.View;
using Code.Gameplay.Features.Battle.Services;
using Code.Infrastructure.View;
using Entitas;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code.Gameplay.Features.Battle.View
{
    public class BattlefieldHandler : MonoBehaviour
    {
        [SerializeField] private EntityBehaviour _entityBehaviour;
        [SerializeField] private PointerHandler _pointerHandler;
        private IBattleFieldFactory _battleFieldFactory;
        private GameContext _game;

        [Inject]
        public void Construct(IBattleFieldFactory battleFieldFactory, IUIFactory uiFactory)
        {
            _battleFieldFactory = battleFieldFactory;
            _game = Contexts.sharedInstance.game;
        }

        private void Awake() =>
            _pointerHandler.OnPointerDownEvent += OnPointerDown;

        private void OnDisable() =>
            _pointerHandler.OnPointerDownEvent -= OnPointerDown;
        
        private void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right) 
                return;
            
            IGroup<GameEntity> toggles = _game.GetGroup(GameMatcher.AnyOf(GameMatcher.CitizenToggleEnabling, GameMatcher.SoldiersToggleEnabling));
            
            foreach (GameEntity entity in toggles.GetEntities())
            {
                entity.isCitizenToggleEnabling = false;
                entity.isSoldiersToggleEnabling = false;
            }
            
            _battleFieldFactory.TrySetDefendersAndCreateBattlefield(_entityBehaviour);
        }
    }
}