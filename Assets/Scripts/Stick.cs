using System;
using Joystick.UGUI;
using UnityEngine;

namespace Joystick
{
    [RequireComponent(typeof(RectTransform))]
    public class Stick : MonoBehaviour
    {
        private static readonly Vector2 OriginOffset = new Vector2(0.5f, 0.5f);
        
        [SerializeField] private JoystickInput _joystick;

        private RectTransform _rectTransform;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            _joystick.OnDragMove += OnJoystickDragMove;
            _joystick.OnDragEnd += OnJoystickDragEnd;
        }
        
        private void OnDisable()
        {
            _joystick.OnDragMove -= OnJoystickDragMove;
            _joystick.OnDragEnd -= OnJoystickDragEnd;
        }

        private void OnJoystickDragMove(Vector2 pos)
        {
            _rectTransform.anchorMin = pos * 0.5f + OriginOffset;
            _rectTransform.anchorMax = pos * 0.5f + OriginOffset;
        }
        
        private void OnJoystickDragEnd(Vector2 pos)
        {
            var target = OriginOffset;
            _rectTransform.anchorMin = target;
            _rectTransform.anchorMax = target;
        }
    }
}