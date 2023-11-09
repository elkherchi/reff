using System;
using UnityEngine;

namespace com.just.joystick
{
	public class SpaceshipDemo : MonoBehaviour
	{
		[SerializeField] private Joystick _joystickLeft;
		[SerializeField] private Joystick _joystickRight;
		[SerializeField] private Transform _spaceshipTransform;
		[SerializeField] private float _spaceshipSteering = 2f;
		[SerializeField] private float _spaceshipSteeringFactor = 15f;
		[SerializeField] private float _spaceshipThrottle = .1f;
		[SerializeField] private float _spaceshipMaxVelocity = 5f;
		[SerializeField] private float _spaceshipVelocityDrag = .99f;

		private float _spaceshipAngle;
		private float _spaceshipTargetAngle;
		private Vector3 _spaceshipVelocity;
		private float _spaceshipEnginePower;
		
		private void Update()
		{	
			if (Mathf.Approximately(_joystickLeft.Angle, 90f))
				_spaceshipTargetAngle += _joystickLeft.Value * _spaceshipSteering;
			else
				_spaceshipTargetAngle -= _joystickLeft.Value * _spaceshipSteering;
			
			if (Mathf.Approximately(_joystickRight.Value, 0f))
			{
				_spaceshipEnginePower = 0f;
			}
			else
			{
				if (Mathf.Approximately(_joystickRight.Angle, 0f))
				{
					_spaceshipEnginePower = _joystickRight.Value * _spaceshipThrottle;
				}
				else
				{
					_spaceshipEnginePower = _joystickRight.Value * -_spaceshipThrottle;
				}
			}

			_spaceshipAngle += (_spaceshipTargetAngle - _spaceshipAngle) / _spaceshipSteeringFactor;

			var vehicleEulerAngles = _spaceshipTransform.eulerAngles;
			vehicleEulerAngles.y = _spaceshipAngle;
			
			if (Mathf.Approximately(_spaceshipEnginePower, 0f))
				_spaceshipVelocity *= _spaceshipVelocityDrag;
			else
			{
				var velocityX = Mathf.Sin(vehicleEulerAngles.y * Mathf.Deg2Rad);
				var velocityZ = Mathf.Cos(vehicleEulerAngles.y * Mathf.Deg2Rad);
				var velocity = new Vector3(velocityX, 0f, velocityZ) * _spaceshipEnginePower;
				_spaceshipVelocity += velocity;
				_spaceshipVelocity = Vector3.ClampMagnitude(_spaceshipVelocity, _spaceshipMaxVelocity);
			}
			
			_spaceshipTransform.eulerAngles = vehicleEulerAngles;
			_spaceshipTransform.position += _spaceshipVelocity * Time.deltaTime;
			
		}
	}
}
