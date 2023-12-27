using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UI.Event_Listeners {
    public class HoverEventListener : MonoBehaviour,
                                      IPointerEnterHandler,
                                      IPointerExitHandler,
                                      IPointerDownHandler,
                                      IPointerUpHandler {
        
        [Header("Refs")]
        [SerializeField] private Image _image;
        [SerializeField] private Transform _scaledObj;

        [Space] [Header("Settings")]
        [SerializeField] private bool _isSlider;
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _hoverColor = Color.grey;
        [SerializeField] private Color _selectedColor = Color.gray;
        [SerializeField] private float _hoverScaleMultiplier = 1.05f;
        [SerializeField] private float _clickedScaleMultiplier = 1.1f;
        
        [Space] [Header("Click Events")]
        public UnityEvent OnClickEvents = new();


        private RoundImageCorners _imageUpdater;

        public RoundImageCorners ImageUpdater { set => _imageUpdater = value; }

        public void OnPointerEnter(PointerEventData eventData) {
            SetTintColor(_hoverColor);
            SetScaleMultiplier(_hoverScaleMultiplier);
        }

        public void OnPointerExit(PointerEventData eventData) {
            if (_isSlider && _pointerDown)
                return;

            SetTintColor(_defaultColor);
            SetScaleMultiplier(1);
        }

        private bool _pointerDown;

        public void OnPointerDown(PointerEventData eventData) {
            SetTintColor(_selectedColor);
            SetScaleMultiplier(_clickedScaleMultiplier);
            _pointerDown = true;
        }

        public void OnPointerUp(PointerEventData eventData) {
            if (_pointerDown)
                OnClickEvents.Invoke();
            
            SetTintColor(_defaultColor);
            SetScaleMultiplier(1);
            _pointerDown = false;
        }

        private void SetTintColor(Color color) {
            if (_imageUpdater) {
                _imageUpdater.TintColor = color;
                _imageUpdater.Refresh();
            } else if (_image) {
                _image.color = color;
            }
        }

        private void SetScaleMultiplier(float multiplier) {
            if (_scaledObj == null)
                transform.localScale = Vector3.one * multiplier;
            else
                _scaledObj.localScale = Vector3.one * multiplier;
        }
    }
}