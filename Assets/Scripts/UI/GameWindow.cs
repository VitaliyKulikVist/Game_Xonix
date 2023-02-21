using System;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Helpers;
using Assets.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Ui {
	public class GameWindow : MonoBehaviour {
		[Header("Base")]
		[SerializeField] private PlayerStorage _playerStorage = default;

		[Header("Components")]
		[SerializeField] private RectTransform _panelContainer = default;
		[SerializeField] private Transform _textContainer = default;
		[SerializeField] private TMP_Text _reactionText = default;

		[Header("Settings")]
		[SerializeField] private float _mobileSensivityMultiplier = .01f;
		[SerializeField] private string _textWin = "You WIN!!";
		[SerializeField] private string _textLose = "You Lose(";

		[Header("Swipe Settings")]
		[SerializeField] private EventTrigger _eventTriggerSwipe = default;

		#region Actions
		public static Action<float, float> OnDragHorizontalDeltaAction = default;
		public static Action OnEndDragHorizontalAction = default;

		public static Action<Vector3> PointerDownAction = default;
		public static Action<Vector3> PointerUpAction = default;
		public static Action<Vector3, Vector3> OnDragAction = default;
		public static Action<bool> OnHoldAction = default;
		#endregion

		#region Validate
		private float _mouseDragDelta = default;
		private float _inputStartTime = default;
		private float _inputStartHeight = default;
		private Vector3 _inputStartPosition = default;

		private Vector3 _startPointSwipe = default;
		private Vector3 _endPointSwipe = default;

		private bool canMove = false;
		#endregion

		#region Get\Set
		public DragState CurrencyDragState { get; private set; } = default;
		#endregion

		private void Awake() {
			PrepareSwipe();
		}

		private void OnEnable() {
			GameManager.LevelStartAction += ReactiononStartGame;
			GameManager.LevelFinishAction += ReactionFinishGame;
		}
		private void OnDisable() {
			GameManager.LevelStartAction -= ReactiononStartGame;
			GameManager.LevelFinishAction -= ReactionFinishGame;
		}

		private void ReactiononStartGame() {
			ControllReactionContainer(false, () => {
				canMove = true;
			});
		}

		private void ReactionFinishGame(LevelResult levelResult) {
			if (levelResult == LevelResult.Win) {
				canMove = false;
				ControllReactionContainer(true);
				PrepareTextReaction(LevelResult.Win);
			}

			if (_playerStorage.ConcretePlayer.PlayerLive <= 0) {
				ControllReactionContainer(true);
				PrepareTextReaction(LevelResult.Lose);
			}
		}

		private void PrepareSwipe() {
			_eventTriggerSwipe.triggers.Clear();
			EventTrigger.Entry down = new EventTrigger.Entry {
				eventID = EventTriggerType.PointerDown
			};
			down.callback.AddListener((data) => {
				_startPointSwipe = Input.mousePosition;
				PointerDownAction?.Invoke(_startPointSwipe);
				OnHoldAction?.Invoke(true);
			});
			EventTrigger.Entry up = new EventTrigger.Entry {
				eventID = EventTriggerType.PointerUp
			};
			up.callback.AddListener((data) => {
				_endPointSwipe = Input.mousePosition;
				PointerUpAction?.Invoke(_endPointSwipe);
				OnHoldAction?.Invoke(false);
			});
			EventTrigger.Entry drag = new EventTrigger.Entry {
				eventID = EventTriggerType.Drag
			};
			drag.callback.AddListener((data) => {
				OnDragAction?.Invoke(_startPointSwipe, Input.mousePosition);
			});

			_eventTriggerSwipe.triggers.Add(down);
			_eventTriggerSwipe.triggers.Add(up);
			_eventTriggerSwipe.triggers.Add(drag);
		}


		private void Update() {
			if (canMove) {
#if UNITY_EDITOR

				if (Input.GetMouseButton(0)) {
					_mouseDragDelta = Input.GetAxis("Mouse X");
				}
#else
            if (Input.touchCount > 0)
            {
                mouseDragDelta = Input.GetTouch(0).deltaPosition.x * _mobileSensivityMultiplier;
            }     
#endif

				if (Input.GetMouseButtonDown(0)) {
					_inputStartTime = Time.time;
					CurrencyDragState = DragState.Wait;
					_inputStartHeight = Input.mousePosition.y;
					_inputStartPosition = Input.mousePosition;
				}

				if (Input.GetMouseButton(0)) {
					if (CurrencyDragState == DragState.Wait) {
						if (Vector3.Distance(_inputStartPosition, Input.mousePosition) < 5f && Time.time - _inputStartTime > .1f) {
							CurrencyDragState = DragState.Canceled;
						}
						if (Vector3.Distance(_inputStartPosition, Input.mousePosition) > 5f && Time.time - _inputStartTime < .1f) {
							CurrencyDragState = DragState.Continue;
						}
					}
				}

				if (Input.GetMouseButtonUp(0)) {
					_mouseDragDelta = 0;
					CurrencyDragState = DragState.Canceled;
					OnEndDragHorizontalAction?.Invoke();
				}


				if (CurrencyDragState == DragState.Continue) {
					OnDragHorizontalDeltaAction?.Invoke(_mouseDragDelta, _inputStartHeight);
				}
			}
		}
		#region Controll
		private void Hide() {
			AnimationUiWindow.AnimationWindowControll(_panelContainer, AnimationUiWindow.PositionMoveType.OY, false, 0.3f);
		}

		private void Show() {
			AnimationUiWindow.AnimationWindowControll(_panelContainer, AnimationUiWindow.PositionMoveType.OY, true, 0.3f);
			ControllReactionContainer(false);
		}

		private void ControllReactionContainer(bool switcher, Action callBack = null!) {
			AnimationUiWindow.AnimationWindowControll(_textContainer, switcher, 0.3f, callBack);
		}

		private void PrepareTextReaction(LevelResult levelResult) {

			switch (levelResult) {
				case LevelResult.None:
					_reactionText.text = "Empty";
					_reactionText.color = Color.black;
					break;
				case LevelResult.Win:
					_reactionText.text = _textWin;
					_reactionText.color = Color.green;
					break;
				case LevelResult.Lose:
					_reactionText.text = _textLose;
					_reactionText.color = Color.red;
					break;
				default:
					_reactionText.text = "Default text";
					_reactionText.color = Color.black;
					break;
			}

		}
		#endregion
	}
}
