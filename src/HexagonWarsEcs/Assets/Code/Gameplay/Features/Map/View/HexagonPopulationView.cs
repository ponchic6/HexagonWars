using Code.Infrastructure.View;
using TMPro;
using UnityEngine;

namespace Code.Gameplay.Features.Map.View
{
    public class HexagonPopulationView : MonoBehaviour
    {
        [SerializeField] private EntityBehaviour _entityBehaviour;
        [SerializeField] private TMP_Text _citizens;
        [SerializeField] private TMP_Text _warriors;

        private void Update()
        {
            if (_entityBehaviour.Entity.hasCitizensAmount) 
                _citizens.text = _entityBehaviour.Entity.citizensAmount.Value.ToString();
            
            if (_entityBehaviour.Entity.hasWarriorsAmount) 
                _warriors.text = _entityBehaviour.Entity.warriorsAmount.Value.ToString();
        }
    }
}