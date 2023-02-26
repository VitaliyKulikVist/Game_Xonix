using System;
using System.Collections;
using Assets.Scripts.Common;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts {
	public class MainTimer : MonoBehaviour {
		[Header("Base")]
		[SerializeField] private PlayerStorage _playerStorageSO = default;

		[Header("Format")]
		[SerializeField] private string _formatTime = "{0}:{1}";
		[SerializeField] private string _formatTimeAfterHours = "{0}:{1}:{2}";

		[Header("Just Controll"), Tooltip("Вивів просто для візуального контролю роботоспособності")]
		[SerializeField] private int _sec = 0;
		[SerializeField] private int _min = 0;
		[SerializeField] private int _hor = 0;

		public static Action<int, int, int> ReturnMinSecHorInt = default;
		public static Action<string> ReturnMinSecString = default;

		#region Validate
		private Coroutine tempCor = default;
		private bool isPaused = false;
		#endregion

		private void Update() {
			if (Input.GetKeyDown(KeyCode.Tab)) {
				GameManager.LevelFinishAction?.Invoke(LevelResult.Win);
			}

			if (Input.GetKeyDown(KeyCode.CapsLock)) {
				_playerStorageSO.ConcretePlayer.Changelive(-1);
			}
		}

		private void OnEnable() {
			GameManager.LevelStartAction += StartLevelReaction;
			GameManager.LevelFinishAction += FinishLevelReaction;

			GameManager.PausedLevelAction += PausedGameReaction;
			GameManager.PlayLevelAction += PlayGameReaction;
		}

		private void OnDisable() {
			GameManager.LevelStartAction -= StartLevelReaction;
			GameManager.LevelFinishAction -= FinishLevelReaction;

			GameManager.PausedLevelAction -= PausedGameReaction;
			GameManager.PlayLevelAction -= PlayGameReaction;
		}

		private void StartLevelReaction() {
			ConvertIntTimeToString(0, 0, 0);
			StartTimer();
		}

		private void StartTimer() {
			if (tempCor != null) {
				StopCoroutine(tempCor);
			}

			tempCor = StartCoroutine(Timer());
		}

		private void StopTimer() {
			if (tempCor != null) {
				StopCoroutine(tempCor);
			}
			_sec = 0;
			_min = 0;
			_hor = 0;
			tempCor = null;
		}

		private IEnumerator Timer() {
			while (_playerStorageSO.ConcretePlayer.PlayerLive > 0 && !isPaused) {
				_sec += 1;

				if (_sec == 60) {
					_min++;
					_sec = 0;
				}

				if (_min == 60) {
					_hor++;
					_min = 0;
					_sec = 0;
				}

				ReturnMinSecHorInt?.Invoke(_min, _sec, _hor);
				ConvertIntTimeToString(_min, _sec, _hor);
				yield return new WaitForSecondsRealtime(1f);
			}

			tempCor = null;
		}
		private void ConvertIntTimeToString(int _min, int _sec, int _hor) {
			string tempString;
			if (_hor <= 0) {
					tempString = String.Format(_formatTime, _min.ToString("D2"), _sec.ToString("D2"));
			}

			else {
				tempString = String.Format(_formatTimeAfterHours, _min.ToString("D2"), _sec.ToString("D2"), _hor.ToString("D2"));
			}

			ReturnMinSecString?.Invoke(tempString);
		}
		private void FinishLevelReaction(LevelResult _levelResult) {
			if (_levelResult == LevelResult.Win) {
				StopTimer();
				ConvertIntTimeToString(0, 0, 0);
			}
		}

		private void PausedGameReaction() {
			isPaused = true;
		}

		private void PlayGameReaction() {
			isPaused = false;

			StartTimer();
		}
	}
}
