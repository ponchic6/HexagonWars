using Code.Infrastructure.View;
using UnityEngine;

namespace Code.Gameplay.Features.Map.View
{
    public class NeighbourSearcher : MonoBehaviour
    {
        [SerializeField] private NeighboringHexagons _neighboringHexagons;
        
        private void Update()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.75f);
            
            if (colliders.Length == 0)
                return;
            
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<EntityBehaviour>() == GetComponentInParent<EntityBehaviour>())
                    continue;
                
                _neighboringHexagons.NeighboringHexagonsList.Add(collider.GetComponent<EntityBehaviour>());
            }

            Destroy(gameObject);
        }
    }
}
