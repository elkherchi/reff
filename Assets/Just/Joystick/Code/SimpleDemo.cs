using UnityEngine;
using UnityEngine.UI;

namespace com.just.joystick
{
	public class SimpleDemo : MonoBehaviour
	{
		[SerializeField] private Joystick[] _joysticks;
		[SerializeField] private Text _text;
		[SerializeField] private Text _subText;

		private void Start()
		{
			foreach (var joystick in _joysticks)
			{
				joystick.OnStartDrag += JoystickOnStartDrag;
				joystick.OnStopDrag += JoystickOnStopDrag;
				joystick.OnUpdate += JoystickOnUpdate;
				joystick.OnPressure += JoystickOnPressure;
				joystick.OnPressureTap += JoystickOnPressureTap;
			}

			var joy = _joysticks[0];
			Debug.Log("Initialised: " + joy.Initialised + ", disabled: " + joy.Disabled + ", hidden: " + joy.Hidden + ", active: " + joy.Active);
			joy.Hide();
			Debug.Log("Initialised: " + joy.Initialised + ", disabled: " + joy.Disabled + ", hidden: " + joy.Hidden + ", active: " + joy.Active);
			joy.Disable();
			Debug.Log("Initialised: " + joy.Initialised + ", disabled: " + joy.Disabled + ", hidden: " + joy.Hidden + ", active: " + joy.Active);
			joy.Enable();
			Debug.Log("Initialised: " + joy.Initialised + ", disabled: " + joy.Disabled + ", hidden: " + joy.Hidden + ", active: " + joy.Active);
			joy.Show();
			Debug.Log("Initialised: " + joy.Initialised + ", disabled: " + joy.Disabled + ", hidden: " + joy.Hidden + ", active: " + joy.Active);
		}
		
		private void JoystickOnStartDrag(Joystick joystick)
		{
			var logText = "JoystickOnStartDrag: " + joystick.gameObject.name;
			_text.text = logText;
			Debug.LogWarning(logText);
		}
		
		private void JoystickOnStopDrag(Joystick joystick)
		{
			var logText = "JoystickOnStopDrag: " + joystick.gameObject.name;
			_text.text = logText;
			Debug.LogWarning(logText);
			_subText.text = string.Empty;
		}

		private void JoystickOnUpdate(Joystick joystick, float angle, float value)
		{
			var logText = "JoystickOnUpdate: " + joystick.gameObject.name + ", angle: " + angle + ", value: " + value;
			_subText.text = logText;
			Debug.LogWarning(logText);
		}
		
		private void JoystickOnPressure(Joystick joystick, int fingerId, Vector2 position, float pressure, Vector2 deltaPosition, float deltaTime)
		{
			var logText = "JoystickOnPressure: " + joystick.gameObject.name;
			_text.text = logText;
			Debug.LogWarning(logText);
		}	
		
		private void JoystickOnPressureTap(Joystick joystick, int fingerId, Vector2 position, float pressure, Vector2 deltaPosition, float deltaTime)
		{
			var logText = "JoystickOnPressureTap: " + joystick.gameObject.name;
			_text.text = logText;
			Debug.LogWarning(logText);
		}	
	}
}
