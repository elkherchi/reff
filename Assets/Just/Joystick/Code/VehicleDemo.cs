using UnityEngine;

namespace com.just.joystick
{
	public class VehicleDemo : MonoBehaviour
	{
		[SerializeField] private Joystick _joystickLeft;
		[SerializeField] private Joystick _joystickRight;
		[SerializeField] private Transform _vehicleTransform;
		[SerializeField] private float _maxVehicleForwardSpeed = 5f;
		[SerializeField] private float _maxVehicleBackwardSpeed = 2f;
		[SerializeField] private float _vehicleSpeedDrag = .99f;

		private float _vehicleSpeed;
		
		private void Update()
		{
			var vehicleEulerAngles = _vehicleTransform.eulerAngles;

			if (Mathf.Approximately(_joystickLeft.Angle, 90f))
				vehicleEulerAngles.y += _joystickLeft.Value;
			else
				vehicleEulerAngles.y -= _joystickLeft.Value;
			
			_vehicleTransform.eulerAngles = vehicleEulerAngles;

			if (Mathf.Approximately(_joystickRight.Value, 0f))
			{
				_vehicleSpeed *= _vehicleSpeedDrag;
			}
			else
			{
				if (Mathf.Approximately(_joystickRight.Angle, 0f))
				{
					_vehicleSpeed += _joystickRight.Value;
					if (_vehicleSpeed > _maxVehicleForwardSpeed)
						_vehicleSpeed = _maxVehicleForwardSpeed;
				}
				else
				{
					_vehicleSpeed -= _joystickRight.Value;
					if (_vehicleSpeed < -_maxVehicleBackwardSpeed)
						_vehicleSpeed = -_maxVehicleBackwardSpeed;
				}
			}
			
			var velocityX = Mathf.Sin(vehicleEulerAngles.y * Mathf.Deg2Rad);
			var velocityZ = Mathf.Cos(vehicleEulerAngles.y * Mathf.Deg2Rad);
			var velocity = new Vector3(velocityX, 0f, velocityZ) * _vehicleSpeed;
			
			_vehicleTransform.position += velocity * Time.deltaTime;
		}
	}
}
