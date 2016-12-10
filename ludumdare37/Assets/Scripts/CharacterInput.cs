using UnityEngine;

public class CharacterInput : MonoBehaviour
{	
	[SerializeField]
	private float _movementSpeed = 1f;
	private Transform _t;
	private CharacterController _character;
	private float _currentMovementSpeed;
	private CharacterState _state;

	private float _recoilDeceleration = 5f;

	private IWeapon _weapon;

	private enum CharacterState
	{
		Free,
		Sliding
	}

	void Awake()
	{
		_t = transform;
		_character = GetComponent<CharacterController>();
		_state = CharacterState.Free;
		_currentMovementSpeed = 0f;
		_weapon = GetComponentInChildren<IWeapon>();
	}

	void Update ()
	{
		if (_state == CharacterState.Sliding)
		{
			_currentMovementSpeed = Mathf.Clamp(
				_currentMovementSpeed + _recoilDeceleration * Time.deltaTime,
				_currentMovementSpeed,
				0f);

			if (_currentMovementSpeed == 0f)
			{
				_state = CharacterState.Free;
				Debug.Log(_state);
			}
		}
		else
		{
			if (Input.GetButton("Fire1"))
			{
				var recoil = _weapon.Use();
				if (recoil != 0)
				{
					_state = CharacterState.Sliding;
					Debug.Log(_state);
					_currentMovementSpeed = -recoil;

					return;
				}
			}

			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");

			if (Input.GetKeyDown(KeyCode.R))
			{
				_weapon.Reload(2);
			}

			if (Mathf.Abs(h) > Mathf.Abs(v))
			{
				_t.rotation = Quaternion.Euler(0f, 90f * Mathf.Sign(h), 0f);
				_currentMovementSpeed = _movementSpeed;
			}
			else if (Mathf.Abs(h) < Mathf.Abs(v))
			{
				_t.rotation = Quaternion.Euler(0f, 90 * Mathf.Sign(v) - 90f, 0f);
				_currentMovementSpeed = _movementSpeed;
			}
			else
			{
				_currentMovementSpeed = 0f;
			}
		}

		_character.Move(_t.forward * Time.deltaTime * _currentMovementSpeed);
	}
}