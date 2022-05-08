using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Joystick.UGUI
{
    [RequireComponent(typeof(RectTransform))]
    public class JoystickInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private Canvas _canvas;
        private RectTransform _rectTransform;
        private bool _isCaptured;

        public event Action<Vector2> OnDragBegin, OnDragMove, OnDragEnd;

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (!_isCaptured) return;
            OnDragMove?.Invoke(GetPosition());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isCaptured = false;
            OnDragEnd?.Invoke(GetPosition());
        }

        private void OnPointerDownInternal(Vector2 screenPosition)
        {
            var pos = ScreenPixelToLocalNormalized(screenPosition);
            if (pos.magnitude > 1f) return;

            _isCaptured = true;
            OnDragBegin?.Invoke(pos);
        }

        private Vector2 GetPosition()
        {
            var localNormalized = ScreenPixelToLocalNormalized(GetInputPosition());
            var insideCircle = Vector2.ClampMagnitude(localNormalized, 1f);
            return insideCircle;
        }

        private Vector2 ScreenPixelToLocalNormalized(Vector2 position)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform,
                position,
                _canvas.worldCamera,
                out var localPosition);

            var rect = _rectTransform.rect;
            var unitSize = rect.size * 0.5f;
            
            return (localPosition - rect.center) / unitSize;
        }

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        private int _capturedTouchIndex;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _capturedTouchIndex = Input.touchCount - 1;
            OnPointerDownInternal(eventData.position);
        }
        
        private Vector2 GetInputPosition()
        {
            return Input.GetTouch(_capturedTouchIndex).position;
        }
#else
        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownInternal(eventData.position);
        }

        private Vector2 GetInputPosition()
        {
            return Input.mousePosition;
        }
#endif
    }
}