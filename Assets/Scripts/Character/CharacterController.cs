using Assets.Scripts.Common;
using Assets.Scripts.Common.Helpers;
using Assets.Scripts.Enemies;
using Assets.Scripts.World.Grid;
using UnityEngine;

namespace Assets.Scripts.Character {
	public class CharacterController : MonoBehaviour, IValidateHalper {
		[Header("Base")]
		[SerializeField] private DependencyInjections _dependencyInjections = default;

		[Header("Components")]
		[SerializeField] private RectTransform _container = default;

		[Header("Settings move")]
		[SerializeField] private float _speedMoveCharacter = 50f;
		[SerializeField] private float _speedMovePoint = 5f;
		[SerializeField] private float _distanceMove = .05f;

		[Header("Settings rotation")]
		[SerializeField] private float _rotationModif = 90f;
		[SerializeField] private float _rotationSpeed = 50f;
		[SerializeField] private float _distanceDontControllRotation = 1f;

		[SerializeField] private RectTransform _movePoint = default;

		[field: Header("On Validate Settings")]
		[field: SerializeField] public bool IsValidate { get; set; }

		#region Variable
		private bool _canMove = false;
		private DynamicJoystick _dynamicJoystick = default;

		private Vector3 _tempVectorDirection = default;
		private float _tempAngleDirection = 0f;
		private Quaternion tempAngleAxisDirection = default;
		private GridManager _gridManager = null;
		#endregion

		private void Awake() {
			ResetPlayer();
			ResetRotation();
			ResetMovePoint();
			PrepareCamera();
			_dependencyInjections.PlayerPosition = _container;
		}
		private void Start() {
			_dynamicJoystick = _dependencyInjections.DynamicJoystick;
			_gridManager = _dependencyInjections.GridManager;
		}
		private void OnEnable() {
			GameManager.LevelStartAction += ReactionStartGame;
			GameManager.LevelFinishAction += ReactionFinishGame;
			GameManager.PausedLevelAction += ReactionPaused;
			GameManager.PlayLevelAction += ReactionPlay;
		}
		private void OnDisable() {
			GameManager.LevelStartAction -= ReactionStartGame;
			GameManager.LevelFinishAction -= ReactionFinishGame;
			GameManager.PausedLevelAction -= ReactionPaused;
			GameManager.PlayLevelAction -= ReactionPlay;
		}
		private void FixedUpdate() {
			if (_canMove) {
				//Vector3 direction = Vector3.up * _dynamicJoystick.Vertical + Vector3.right * _dynamicJoystick.Horizontal;

				_container.position = Vector3.MoveTowards(_container.position, _movePoint.position, _speedMoveCharacter * Time.fixedDeltaTime);

				var dist = Vector3.Distance(_container.position, _movePoint.position);

				if (dist <= _distanceMove) {

					_movePoint.position += new Vector3(_dynamicJoystick.Horizontal * _speedMovePoint, _dynamicJoystick.Vertical * _speedMovePoint, 0f);

					//if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f) {
					//	_movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal") * _speedMovePoint, 0f, 0f);
					//}

					//if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f) {
					//	_movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical") * _speedMovePoint, 0f);
					//}
				}

				if (dist >= _distanceDontControllRotation) {
					_tempVectorDirection = _movePoint.position - _container.position;
					_tempAngleDirection = Mathf.Atan2(_tempVectorDirection.y, _tempVectorDirection.x) * Mathf.Rad2Deg - _rotationModif;
					tempAngleAxisDirection = Quaternion.AngleAxis(_tempAngleDirection, Vector3.forward);

					_container.rotation = Quaternion.Slerp(_container.rotation, tempAngleAxisDirection, Time.fixedDeltaTime * _rotationSpeed);
				}
			}
		}

		#region Resets
		private void ResetPlayer() {
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;
			transform.localScale = Vector3.one;

			_container.position = Vector3.zero;
			_container.rotation = Quaternion.identity;
			_container.localScale = Vector3.one;
		}

		private void ResetMovePoint() {
			_movePoint.position = new Vector3(0f, -19f, 0f);
		}

		private void ResetRotation() {
			_tempVectorDirection = Vector3.zero;
			_tempAngleDirection = 0f;
			tempAngleAxisDirection = Quaternion.identity;
		}

		#endregion

		#region Reaction to Action
		private void ReactionStartGame() {
			PreparePlayerPosition();

			_canMove = true;
		}

		private void ReactionFinishGame(LevelResult levelResult) {
			_canMove = false;
		}
		private void ReactionPaused() {
			_canMove = false;
		}

		private void ReactionPlay() {
			_canMove = true;
		}
		#endregion

		#region hit reaction
		public void HitLandUnit(GridUnit<GridUnitLandType> gridUnit) {
			Debug.Log($"Character hit to land\t{gridUnit.gameObject.name}");

		}

		public void HitSeaUnit(GridUnit<GridUnitSeaType> gridUnit) {
			Debug.Log($"Character hit to Sea\t{gridUnit.gameObject.name}");

		}

		public void HitEnemt<TEnum>(EnemyControllerAbstract<TEnum> enemy)
			where TEnum : System.Enum {
			Debug.Log($"Character hit to Enemy\t{enemy.gameObject.name}");
		}
		#endregion

		private void PrepareCamera() {
			var canvas = gameObject.GetComponent<Canvas>();

			if (canvas != null
				&& canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace
				&& canvas.worldCamera == null) {
				canvas.worldCamera = Camera.main;
			}
		}
		private void PreparePlayerPosition() {
			ResetPlayer();
			transform.position = GetRandomLandPosition();
		}

		private Vector3 GetRandomLandPosition() {
			var ss = _gridManager.GetListAllLandPosition();
			if (ss != null && ss.Count > 0) {

				return ss[Random.Range(0, ss.Count)];
			}

			else {
				Debug.LogError("Can`t take position spawn from player");

				return Vector3.zero;
			}
		}

		public void OnValidate() {
			if (IsValidate) {
				PrepareCamera();
			}
		}
	}
}
