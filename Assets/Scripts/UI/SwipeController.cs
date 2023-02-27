using System;
using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Ui {

	public class SwipeController : MonoBehaviour {

		[Header("Swipe component")]
		[SerializeField] private DependencyInjections _dependencyInjections = default;
		[SerializeField] private EventTrigger _eventTriggerSwipe = default;

		[Header("Sensitive Settings Swipe")]
		[SerializeField] private float _sensitySwipeHorizontal = 0.15f;
		[SerializeField] private float _sensitySwipeVertical = 0.15f;
		[SerializeField] private float _currencySwipeLengthToGood = 0.6f;
		
		#region Action
		public static Action<TypeSwipe> GoodSwipeAction = default;
		public static Action<bool> OnHoldActionAction = default;
		#endregion

		#region Get\Set
		public float GetSwipeLengthToGood { get => _currencySwipeLengthToGood; }
		#endregion

		#region Variable
		private float _inputStartTime = default;
		private Vector3 _inputStartPosition = default;
		private Vector3 _startPointSwipe = default;
		private Vector3 _endPointSwipe = default;
		private bool _startGame = false;
		private Vector3 _enterPos = default;
		private float _tempEnvirementX = 0f;
		private float _tempEnvirementY = 0f;
		private float _tempEnvirement1 = 0f;
		private float _tempEnvirement2 = 0f;
		private DragState _currencyDragState = default;
		private float _currencySwipeLength = 0f;
		private TypeSwipe _currentTypeEnvirement = default;
		#endregion

		private void Awake() {
			_dependencyInjections.SwipeController = this;
			PrepareSwipe();
			_startGame = false;
		}
		private void OnEnable() {
			GameManager.LevelStartAction += ReactiononStartGame;
			GameManager.LevelFinishAction += ReactionFinishGame;
		}

		private void OnDisable() {
			GameManager.LevelStartAction += ReactiononStartGame;
			GameManager.LevelFinishAction += ReactionFinishGame;
		}

		private void Update() {
			if (_startGame) {
				if (Input.GetMouseButtonDown(0)) {
					_inputStartTime = Time.time;
					_currencyDragState = DragState.Wait;
					_inputStartPosition = Input.mousePosition;
				}

				if (Input.GetMouseButton(0)) {
					if (_currencyDragState == DragState.Wait) {
						if (Vector3.Distance(_inputStartPosition, Input.mousePosition) < 5f && Time.time - _inputStartTime > .1f) {
							_currencyDragState = DragState.Canceled;
						}
						if (Vector3.Distance(_inputStartPosition, Input.mousePosition) > 5f && Time.time - _inputStartTime < .1f) {
							_currencyDragState = DragState.Continue;
						}
					}
				}

				if (Input.GetMouseButtonUp(0)) {
					_currencyDragState = DragState.Canceled;
				}
			}
		}
		private void PrepareSwipe() {
			_eventTriggerSwipe.triggers.Clear();

			EventTrigger.Entry down = new EventTrigger.Entry {
				eventID = EventTriggerType.PointerDown
			};
			down.callback.AddListener((data) => {
				_startPointSwipe = Input.mousePosition;
				PointerDown(_startPointSwipe);
				OnHoldActionAction?.Invoke(true);
			});

			EventTrigger.Entry up = new EventTrigger.Entry {
				eventID = EventTriggerType.PointerUp
			};
			up.callback.AddListener((data) => {
				_endPointSwipe = Input.mousePosition;
				PointerUpEnvirement(_endPointSwipe);
				OnHoldActionAction?.Invoke(false);
			});

			EventTrigger.Entry drag = new EventTrigger.Entry {
				eventID = EventTriggerType.Drag
			};
			drag.callback.AddListener((data) => {
				PointerUpEnvirement(Input.mousePosition);
			});

			_eventTriggerSwipe.triggers.Add(down);
			_eventTriggerSwipe.triggers.Add(up);
			_eventTriggerSwipe.triggers.Add(drag);
		}

		#region Player Controll
		public void PointerDown(Vector3 startPosition) {
			_enterPos = startPosition;
			_currencySwipeLength = 0;
		}

		public void PointerUpEnvirement(Vector3 endPosition) {
			_currencySwipeLength = Mathf.InverseLerp(5f, Screen.width * .5f, Vector2.Distance(_startPointSwipe, endPosition));

			if (_currencySwipeLength > _currencySwipeLengthToGood) {
				GoodSwipeAction?.Invoke(GetSwipeDirection(endPosition));
			}
		}

		private TypeSwipe GetSwipeDirection(Vector3 endPosition) {
			_tempEnvirementX = (_enterPos.x - endPosition.x) / Screen.width;
			_tempEnvirementY = (_enterPos.y - endPosition.y) / Screen.height;
			_tempEnvirement1 = Mathf.Abs(_tempEnvirementX) - Mathf.Abs(_tempEnvirementY);
			_tempEnvirement2 = Mathf.Abs(_tempEnvirementY) - Mathf.Abs(_tempEnvirementX);
			if (_tempEnvirement1 >= _sensitySwipeHorizontal) {
				if (_tempEnvirementX > 0) {
					_currentTypeEnvirement = TypeSwipe.LeftSwipe;
					return TypeSwipe.LeftSwipe;
				}
				else {
					_currentTypeEnvirement = TypeSwipe.RightSwipe;
					return TypeSwipe.RightSwipe;
				}

			}
			else if (_tempEnvirement2 >= _sensitySwipeVertical) {
				if (_tempEnvirementY > 0) {
					_currentTypeEnvirement = TypeSwipe.DownSwipe;
					return TypeSwipe.DownSwipe;
				}
				else {
					_currentTypeEnvirement = TypeSwipe.UpSwipe;
					return TypeSwipe.UpSwipe;
				}
			}
			else {
				_currentTypeEnvirement = TypeSwipe.Click;
				return TypeSwipe.Click;
			}
		}
		#endregion

		#region Reaction
		private void ReactiononStartGame() {
			_startGame = true;
		}

		private void ReactionFinishGame(LevelResult levelResult) {
			_startGame = false;
		}
		#endregion
	}
}
