using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Gameplay.Features.PopUp.View
{
    public class ResourcePopUpConfigurator : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _icon;
        [SerializeField] private float duration;
        [SerializeField] private float moveDistance;
        [SerializeField] private float _horizontalRandomOffset;
        [SerializeField] private AnimationCurve _curve;

        public void Setup(Sprite sprite, float count, Vector3 hexPosition)
        {
            _text.text = count.ToString();
            _icon.sprite = sprite;
            _canvasGroup.alpha = 1f;
            transform.position = hexPosition + Vector3.up * 0.3f +
                                 Vector3.forward * Random.Range(-_horizontalRandomOffset, _horizontalRandomOffset) +
                                 Vector3.right * Random.Range(-_horizontalRandomOffset, _horizontalRandomOffset);
            Vector3 targetPosition = new Vector3(transform.localPosition.x,
                transform.localPosition.y + moveDistance,
                transform.localPosition.z);
            transform.DOLocalMoveY(targetPosition.y, duration);
            _canvasGroup.DOFade(0f, duration).OnComplete(() => Destroy(gameObject)).SetEase(_curve);
        }

    }
}