using System.Collections.Generic;
using Code.Infrastructure.View;
using UnityEngine;

namespace Code.Gameplay.Features.Map.View
{
    public class NeighbourSearcher : MonoBehaviour
    {
        [SerializeField] private NeighboringHexagons _neighboringHexagons;
        [SerializeField] private EntityBehaviour _entityBehaviour;
        
        private void Update()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.75f);
            
            if (colliders.Length == 0)
                return;
            
            _entityBehaviour.Entity.AddNeighboringHexagons(new List<int>());
            
            foreach (Collider collider in colliders)
            {
                EntityBehaviour neighbourEntityBehaviour = collider.GetComponent<EntityBehaviour>();
                
                if (neighbourEntityBehaviour == GetComponentInParent<EntityBehaviour>())
                    continue;
                
                _neighboringHexagons.NeighboringHexagonsList.Add(neighbourEntityBehaviour);
                _entityBehaviour.Entity.neighboringHexagons.Value.Add(neighbourEntityBehaviour.Entity.id.Value);
            }

            Destroy(gameObject);
        }
    }
}
