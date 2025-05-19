using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Gameplay.Features.Map.View
{
    public class CommonMigrationToggleGroup : MonoBehaviour
    {
        private List<Toggle> _toggles = new();
        
        public void AddToggle(Toggle toggle) =>
            _toggles.Add(toggle);

        public void AllTogglesOff() =>
            _toggles.ForEach(x => x.isOn = false);
        
        public void AllTogglesOffExceptOne(Toggle toggle1, Toggle toggle2)
        {
            _toggles.ForEach(x =>
            {
                if (x != toggle1 && x != toggle2) 
                    x.isOn = false;
            });
        }
    }
}