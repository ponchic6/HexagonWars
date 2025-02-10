using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Logic.Common
{
    public class PointerHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<PointerEventData> OnPointerDownEvent;
        public event Action<PointerEventData> OnPointerUpEvent;
        public event Action<PointerEventData> OnPointerEnterEvent;
        public event Action<PointerEventData> OnPointerExitEvent;

        public void OnPointerDown(PointerEventData eventData) =>
            OnPointerDownEvent?.Invoke(eventData);

        public void OnPointerUp(PointerEventData eventData) =>
            OnPointerUpEvent?.Invoke(eventData);

        public void OnPointerEnter(PointerEventData eventData) =>
            OnPointerEnterEvent?.Invoke(eventData);

        public void OnPointerExit(PointerEventData eventData) =>
            OnPointerExitEvent?.Invoke(eventData);
    }
}