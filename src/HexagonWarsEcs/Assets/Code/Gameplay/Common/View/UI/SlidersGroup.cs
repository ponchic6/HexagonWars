using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Gameplay.Common.View.UI
{
    public class SlidersGroup : MonoBehaviour
    {
        private List<Slider> _sliders = new();
        
        public void Add(Slider slider)
        {
            _sliders.Add(slider);
            slider.onValueChanged.AsObservable().Subscribe(_ => OnSliderChanged(slider)).AddTo(this);
        }

        public void Remove(Slider slider) =>
            _sliders.Remove(slider);

        public void ClearSliders() =>
            _sliders.Clear();

        private void OnSliderChanged(Slider changedSlider)
        {
            float total = _sliders.Sum(s => s.value);

            if (total > 1f)
            {
                float excess = total - 1f;
                DistributeExcess(changedSlider, excess);
            }
        }

        private void DistributeExcess(Slider changedSlider, float excess)
        {
            var adjustableSliders = _sliders.Where(s => s != changedSlider && s.value > 0).ToList();

            if (adjustableSliders.Count == 0) return;

            foreach (var slider in adjustableSliders)
            {
                float reduction = excess / adjustableSliders.Count;
                slider.value = Mathf.Max(0, slider.value - reduction);
            }
        }
    }
}