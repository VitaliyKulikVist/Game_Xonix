using System;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Ui {
	public class GameWindow : MonoBehaviour {

		[Header("Components")]
		[SerializeField] private RectTransform _panelContainer = default;

		[Header("Reaction Components and Settings")]
		[SerializeField] private Transform _reactionContainer = default;
		[SerializeField] private TMP_Text _reactionText = default;
		[SerializeField] private Button _reactionButton = default;
		[SerializeField] private Image _reactionButtonImage = default;
		[SerializeField] private TMP_Text _reactionButtonText = default;
		[SerializeField] private string _textButtonWining = "Next";
		[SerializeField] private string _texButtontLosed = "Restart";
		[SerializeField] private Color _colorButtonAfterWin = Color.green;
		[SerializeField] private Color _colorButtonAfterLose = Color.red;
		[SerializeField] private string _textWin = "You WIN!!";
		[SerializeField] private string _textLose = "You Lose(";

		private void Awake() {
			PrepareButton();
		}

		private void OnEnable() {
			GameManager.LevelStartAction += ReactiononStartGame;
			GameManager.LevelFinishAction += ReactionFinishGame;
		}
		private void OnDisable() {
			GameManager.LevelStartAction -= ReactiononStartGame;
			GameManager.LevelFinishAction -= ReactionFinishGame;
		}
		private void PrepareButton() {

			_reactionButton.onClick.RemoveAllListeners();
			_reactionButton.onClick.AddListener(() => {
				GameManager.LevelStartAction?.Invoke();
			});
		}



		#region Reaction
		private void ReactiononStartGame() {
			ControllReactionContainer(false);
		}

		private void ReactionFinishGame(LevelResult levelResult) {
			ControllReactionContainer(true);
			PrepareReaction(levelResult);
		}
		#endregion

		private void ChangeColorButton(bool _win) {
			_reactionButtonImage.color = _win ? _colorButtonAfterWin : _colorButtonAfterLose;
		}
		private void ChangeTextButton(bool _win) {
			_reactionButtonText.text = _win ? _textButtonWining : _texButtontLosed;
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
			AnimationUiWindow.AnimationWindowControll(_reactionContainer, switcher, 0.3f, callBack);
		}

		private void PrepareReaction(LevelResult levelResult) {
			switch (levelResult) {
				case LevelResult.None:
					_reactionText.text = "Empty";
					_reactionText.color = Color.black;
					break;
				case LevelResult.Win:
					_reactionText.text = _textWin;
					_reactionText.color = Color.green;
					ChangeColorButton(true);
					ChangeTextButton(true);
					break;
				case LevelResult.Lose:
					_reactionText.text = _textLose;
					_reactionText.color = Color.red;
					ChangeColorButton(false);
					ChangeTextButton(false);
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
