using System.Collections;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Helpers;
using Assets.Scripts.Enemies;
using Assets.Scripts.Ui;
using Assets.Scripts.World.Grid;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Character {
	public class PlayerController : MonoBehaviour, IValidateHalper {
		[Header("Base")]
		[SerializeField] private DependencyInjections _dependencyInjections = default;

		[Header("Components")]
		[SerializeField] private RectTransform _container = default;

		[Header("Settings Spawn")]
		[SerializeField] private Vector3 _startSpawnPoint = new Vector3(-500f, 30f, 0f);

		[Header("Settings move")]
		[SerializeField] private LayerMask _layerStopsMovement = default;
		[SerializeField] private float _speedMovePlayer = 0.3f;
		[SerializeField] private float _speedMovePoint = 0.3f;

		[Header("Settings rotation")]
		[SerializeField] private float _rotationModif = 90f;
		[SerializeField] private float _rotationSpeed = 50f;
		[SerializeField] private float _distanceDontControllRotation = 1f;

		[SerializeField] private RectTransform _movePoint = default;

		[field: Header("On Validate Settings")]
		[field: SerializeField] public bool IsValidate { get; set; }

		#region Variable
		private bool _canMove = false;

		private Vector3 _tempVectorDirection = default;
		private float _tempAngleDirection = 0f;
		private Quaternion tempAngleAxisDirection = default;
		private GridManager _gridManager = null;
		private Coroutine _tempMoveCorotine = null;
		private bool _onHold = false;
		#endregion

		private void Awake() {
			ResetPlayer();
			ResetRotation();
			ResetMovePoint();
			PrepareCamera();
			_dependencyInjections.PlayerPosition = _container;
		}
		private void OnEnable() {
			_gridManager = _dependencyInjections.GridManager;

			GameManager.LevelStartAction += ReactionStartGame;
			GameManager.LevelFinishAction += ReactionFinishGame;
			GameManager.PausedLevelAction += ReactionPaused;
			GameManager.PlayLevelAction += ReactionPlay;

			SwipeController.GoodSwipeAction += ChackSwipe;
			SwipeController.OnHoldActionAction += OnGold;
		}
		private void OnDisable() {
			GameManager.LevelStartAction -= ReactionStartGame;
			GameManager.LevelFinishAction -= ReactionFinishGame;
			GameManager.PausedLevelAction -= ReactionPaused;
			GameManager.PlayLevelAction -= ReactionPlay;

			SwipeController.GoodSwipeAction -= ChackSwipe;
			SwipeController.OnHoldActionAction -= OnGold;
		}
		private void OnDestroy() {
			ResetDoTween();
		}

		private void FixedUpdate() {
			if (_canMove) {
				RotationCharacter();
			}
		}

		#region Character Controll
		private void ChackSwipe(TypeSwipe typeSwipe) {
			if (!_canMove) {
				return;
			}

			if (_tempMoveCorotine == null) {
				_tempMoveCorotine = StartCoroutine(Move(typeSwipe));
			}
		}
		private IEnumerator Move(TypeSwipe typeSwipe) {
			while (_onHold) {

				Vector3 moveTo = GetMositionMove(typeSwipe, _gridManager.GetWidthAndHeigth);

				var _tempColider = Physics2D.OverlapCircle(moveTo, 3f, _layerStopsMovement);

				if(_tempColider !=null) {
					Debug.Log(_tempColider.gameObject.transform.position);
				}
				
				if (!_tempColider) {
					_movePoint.DOComplete();
					_movePoint.DOMove(moveTo, _speedMovePoint).SetEase(Ease.Flash).OnComplete(() => {

						_container.DOComplete();
						_container.DOMove(_movePoint.position, _speedMovePlayer).SetEase(Ease.Flash);
					});
				}
				yield return new WaitForSeconds(_speedMovePoint + _speedMovePlayer);
			}

			if (!_onHold) {
				_movePoint.DOComplete();
				_container.DOComplete();
				_tempMoveCorotine = null;
			}
		}

		private void OnGold(bool isHold) {
			_onHold = isHold;
		}

		private Vector3 GetMositionMove(TypeSwipe typeSwipe, float intervel = 50f) {
			Vector3 _point = _movePoint.position;
			switch (typeSwipe) {
				case TypeSwipe.None:
					return _point;
				case TypeSwipe.Click:
					return _point;
				case TypeSwipe.LeftSwipe:
					return new Vector3(_point.x - intervel, _point.y, 0f);
				case TypeSwipe.RightSwipe:
					return new Vector3(_point.x + intervel, _point.y, 0f);
				case TypeSwipe.UpSwipe:
					return new Vector3(_point.x, _point.y + intervel, 0f);
				case TypeSwipe.DownSwipe:
					return new Vector3(_point.x, _point.y - intervel, 0f);
				default:
					return _point;
			}
		}

		private void RotationCharacter() {
			var dist = Vector3.Distance(_container.position, _movePoint.position);
			if (dist >= _distanceDontControllRotation) {
				_tempVectorDirection = _movePoint.position - _container.position;
				_tempAngleDirection = Mathf.Atan2(_tempVectorDirection.y, _tempVectorDirection.x) * Mathf.Rad2Deg - _rotationModif;
				tempAngleAxisDirection = Quaternion.AngleAxis(_tempAngleDirection, Vector3.forward);

				_container.rotation = Quaternion.Slerp(_container.rotation, tempAngleAxisDirection, Time.fixedDeltaTime * _rotationSpeed);
			}
		}
		#endregion


		#region Resets
		private void ResetPlayer() {
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;
			transform.localScale = Vector3.one;

			_container.position = Vector3.zero;
			_container.rotation = Quaternion.identity;
			_container.localScale = Vector3.one;
		}

		private void ResetDoTween() {
			transform?.DOKill();
			_container?.DOKill();
			_movePoint?.DOKill();
		}

		private void ResetMovePoint() {
			_movePoint.localPosition = new Vector3(0f, -19f, 0f);
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
			//Debug.Log($"Character hit to land\t{gridUnit.gameObject.name}");

		}

		public void HitSeaUnit(GridUnit<GridUnitSeaType> gridUnit) {
			//Debug.Log($"Character hit to Sea\t{gridUnit.gameObject.name}");

		}

		public void HitEnemt<TEnum>(EnemyControllerAbstract<TEnum> enemy)
			where TEnum : System.Enum {
			//Debug.Log($"Character hit to Enemy\t{enemy.gameObject.name}");
		}
		#endregion

		#region Prepare
		private void PrepareCamera() {
			var canvas = gameObject.GetComponent<Canvas>();

			if (canvas != null
				&& canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace
				&& canvas.worldCamera == null) {
				canvas.worldCamera = Camera.main;
			}

			else {
				Debug.LogError("Can`t take canvas");
			}
		}

		private void PreparePlayerPosition() {
			ResetPlayer();

			_container.position = _startSpawnPoint;
			_movePoint.position = _movePoint.localPosition + _startSpawnPoint;
		}

		#endregion

		private Vector3 GetRandomSeaPosition() {
			var ss = _gridManager.GetListAllSeaPosition();
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
