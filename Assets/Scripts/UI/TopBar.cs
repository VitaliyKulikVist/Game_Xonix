using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Helpers;
using Assets.Scripts.Player;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Ui {
	public class TopBar : MonoBehaviour {
		[Header("Base")]
		[SerializeField] private PlayerStorage _playerStorageSO = default;
		[SerializeField] private GameStorageSettings _gameStorageSettingsSO = default;
		[SerializeField] private RectTransform _container = default;

		[Header("Components")]
		[SerializeField] private Button _pausedButton = default;
		[SerializeField] private Image _pausedButtonimage = default;
		[SerializeField] private List<PlayPausedButtonControll> _pausedControl = default;

		[Header("Time Components")]
		[SerializeField] private TMP_Text _currencyTime = default;

		[Header("Life Components")]
		[SerializeField] private TMP_Text _currencyLife = default;

		[Header("level Components")]
		[SerializeField] private TMP_Text _currencyLevel = default;

		[Header("Progress Components")]
		[SerializeField] private Slider _sliderProgress = default;
		[SerializeField] private TMP_Text _textProgress = default;

		#region Variable 
		private ButtonType _currentButtonType = ButtonType.Pause;
		#endregion

		private void Awake() {
			PrepareButtons();
		}

		private void OnEnable() {
			Show();

			Player.Player.PlayerChangeliveAction += SetCurrencyLive;
			Player.Player.PlayerChangeLevelAction += SetCurrencyLevel;

			GameManager.LevelFinishAction += FinishGame;
			GameManager.PausedLevelAction += PausedGame;
			GameManager.LevelStartAction += StartGameReaction;

			MainTimer.ReturnHorMinSecString += PrepareTextTimer;
		}

		private void OnDisable() {

			Player.Player.PlayerChangeliveAction -= SetCurrencyLive;
			Player.Player.PlayerChangeLevelAction -= SetCurrencyLevel;

			GameManager.LevelFinishAction -= FinishGame;
			GameManager.PausedLevelAction -= PausedGame;
			GameManager.LevelStartAction -= StartGameReaction;

			MainTimer.ReturnHorMinSecString -= PrepareTextTimer;
		}

		private void Show() {
			AnimationUiWindow.AnimationWindowControll(_container, AnimationUiWindow.PositionMoveType.OX, true, _gameStorageSettingsSO.UiDuration);

			SwitchSprite(_currentButtonType);
		}

		private void Hide() {
			AnimationUiWindow.AnimationWindowControll(_container, AnimationUiWindow.PositionMoveType.OY, false, _gameStorageSettingsSO.UiDuration);
		}

		private void SetCurrencyLive() {
			if (_playerStorageSO.ConcretePlayer.PlayerLive > 0) {
				_currencyLife.text = _playerStorageSO.ConcretePlayer.PlayerLive.ToString();
			}

			else {
				_currencyLife.text = "0";
			}
		}
		private void SetCurrencyLevel() {
			_currencyLevel.text = _playerStorageSO.ConcretePlayer.PlayerLevel.ToString();
		}

		private void PrepareButtons() {
			_pausedButton.onClick.RemoveAllListeners();
			_pausedButton.onClick.AddListener(() => {

				if (_currentButtonType == ButtonType.Play) {
					_currentButtonType = ButtonType.Pause;
					GameManager.PlayLevelAction?.Invoke();
				}

				else if (_currentButtonType == ButtonType.Pause) {
					_currentButtonType = ButtonType.Play;
					GameManager.PausedLevelAction?.Invoke();
				}

				SwitchSprite(_currentButtonType);
			});
		}

		private void PrepareTextTimer(string time) {
			if (_playerStorageSO.ConcretePlayer.PlayerLive <= 0) {
				SetCurrencyTime("Dead");
			}
			else {
				SetCurrencyTime(time);
			}
		}
		private void SetCurrencyTime(string time) {
			_currencyTime.text = time;
		}

		private void FinishGame(LevelResult levelResult) {
			if (levelResult == LevelResult.Win) {
				SetCurrencyTime("Win");
			}
		}

		private void PausedGame() {
			SetCurrencyTime("Paused");
		}

		private void StartGameReaction() {
			SetCurrencyLive();
			SetCurrencyLevel();
		}

		#region Progress Controll
		private void PrepareSliderProgress(float value) {
			_sliderProgress.value = 0f;
			if (value <= 1f) {
				_sliderProgress.DOValue(value, 0.3f).OnUpdate(() => {
					PrepareTextSliderProgress(_sliderProgress.value.ToString());
				});
			}

			else {
				Debug.LogError($"Incorrected progres slider value {value}");
			}
		}
		private void PrepareTextSliderProgress(string textValue) {
			_textProgress.text = $"{textValue}%";
		}
		#endregion

		#region Button controll
		private Sprite GetSpriteByType(ButtonType buttonType) {
			return _pausedControl.Find(sel => sel.ButtonType == buttonType).Sprite;
		}
		private void SwitchSprite(ButtonType buttonType) {
			if (buttonType == ButtonType.None) {
				return;
			}

			_pausedButtonimage.sprite = GetSpriteByType(buttonType);
		}
		#endregion
	}
}
