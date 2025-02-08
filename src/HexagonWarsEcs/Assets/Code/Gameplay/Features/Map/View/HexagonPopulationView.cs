using Code.Infrastructure.View;
using TMPro;
using UnityEngine;

namespace Code.Gameplay.Features.Map.View
{
    public class HexagonPopulationView : MonoBehaviour
    {
        [SerializeField] private EntityBehaviour _entityBehaviour;
        [SerializeField] private TMP_Text _text;

        private void Update()
        {
            if (!_entityBehaviour.Entity.hasCitizensAmount) 
                return;
            
            _text.text = _entityBehaviour.Entity.citizensAmount.Value.ToString();
        }
    }
}