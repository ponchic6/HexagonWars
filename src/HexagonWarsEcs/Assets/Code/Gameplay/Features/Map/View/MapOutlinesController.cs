using System.Collections.Generic;
using UnityEngine;

namespace Code.Gameplay.Features.Map.View
{
    public class MapOutlinesController : MonoBehaviour
    {
        private List<GameObject> _outlines = new();
        
        public void DeactivateAllOutline() =>
            _outlines.ForEach(x => x.gameObject.SetActive(false));

        public void AddOutline(GameObject outline) =>
            _outlines.Add(outline);
    }
}