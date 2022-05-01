using Joystick.UGUI;
using UnityEngine;
using UnityEngine.UI;

namespace Joystick
{
    public class JoystickListener : MonoBehaviour
    {
        [SerializeField] private JoystickInput _joystick;
        [SerializeField] private Text _text;

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
            _text.text = pos.ToString();
        }
        
        private void OnJoystickDragEnd(Vector2 pos)
        {
            _text.text = string.Empty;
        } 

    }
}