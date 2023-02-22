using System;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Helpers;
using Assets.Scripts.Player;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Ui {
	public class GameWindow : MonoBehaviour {
		[Header("Base")]
		[SerializeField] private DependencyInjections _dependencyInjections = default;
		[SerializeField] private PlayerStorage _playerStorage = default;

		[Header("Components")]
		[SerializeField] private RectTransform _panelContainer = default;
		[SerializeField] private Transform _textContainer = default;
		[SerializeField] private TMP_Text _reactionText = default;

		[Header("Settings")]
		[SerializeField] private string _textWin = "You WIN!!";
		[SerializeField] private string _textLose = "You Lose(";

		[Header("Controll Settings")]
		[SerializeField] private DynamicJoystick _dynamicJoystick = default;

		#region Actions
		public static Action<float, float> OnDragHorizontalDeltaAction = default;
		public static Action OnEndDragHorizontalAction = default;

		public static Action<Vector3> PointerDownAction = default;
		public static Action<Vector3> PointerUpAction = default;
		public static Action<Vector3, Vector3> OnDragAction = default;
		public static Action<bool> OnHoldAction = default;
		#endregion

		#region Get\Set
		public DragState CurrencyDragState { get; private set; } = default;
		#endregion

		private void Awake() {
			_dependencyInjections.DynamicJoystick = _dynamicJoystick;
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
			ControllReactionContainer(false);
		}

		private void ReactionFinishGame(LevelResult levelResult) {
			if (levelResult == LevelResult.Win) {
				ControllReactionContainer(true);
				PrepareTextReaction(LevelResult.Win);
			}

			if (_playerStorage.ConcretePlayer.PlayerLive <= 0) {
				ControllReactionContainer(true);
				PrepareTextReaction(LevelResult.Lose);
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
