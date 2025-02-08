using Code.Gameplay.Features.Migration.Services;
using Code.Gameplay.Features.Migration.View.UI;
using Code.Infrastructure.View;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code.Gameplay.Features.Migration.View
{
    public class MigrationHandler : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private EntityBehaviour _entityBehaviour;
        private MigrationAllSlidersBlocker _allSlidersBlocker;
        private IMigrationFactory _migrationFactory;
        private GameEntity _entity;

        [Inject]
        public void Construct(IMigrationFactory migrationFactory)
        {
            _migrationFactory = migrationFactory;
        }

        private void Awake()
        {
            _allSlidersBlocker = GetComponentInParent<MigrationAllSlidersBlocker>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                _migrationFactory.SetFinishHexAndCreateMigration(_entityBehaviour);
                _allSlidersBlocker.isSlidersBlocked.Value = false;
            }
        }
    }
}