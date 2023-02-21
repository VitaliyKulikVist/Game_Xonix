using System;
using System.Collections;
using Assets.Scripts.Common;
using Assets.Scripts.Player;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

namespace Assets.Scripts {
	public class MainTimer : MonoBehaviour {
		[Header("Base")]
		[SerializeField] private PlayerStorage _playerStorageSO = default;
		[SerializeField] private GameStorageSettings _gameStorageSettingsSO = default;

		[Header("Format")]
		[SerializeField] private string _formatTime = "{0}:{1}";

		[Header("Just Controll"), Tooltip("Вивів просто для візуального контролю роботоспособності")]
		[SerializeField] private int _sec = 0;
		[SerializeField] private int _min = 0;

		public static Action<int, int> ReturnMinSecInt = default;
		public static Action<string> ReturnMinSecString = default;

		#region Validate
		private Coroutine tempCor = default;
		private bool isPaused = false;
		#endregion

		private void Update() {
			if(Input.GetKeyDown(KeyCode.Tab)) {
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
			if (_playerStorageSO.ConcretePlayer.PlayerLive > 0 &&
				_playerStorageSO.ConcretePlayer.PlayerLive <= BasePlayerConstants.MaxPlayerLife) {
				StartTimer();
			}
			else {
				StopTimer();
				ConvertIntTimeToString(0, 0);
			}
		}

		private void StartTimer() {
			if (tempCor == null) {
				tempCor = StartCoroutine(Timer());
			}
		}

		private void StopTimer() {
			if (tempCor != null) {
				StopCoroutine(tempCor);
			}
			_sec = 0;
			_min = 0;
			tempCor = null;
		}

		private IEnumerator Timer() {
			while (_playerStorageSO.ConcretePlayer.PlayerLive > 0 && !isPaused) 
				{
				_sec += 1;

				if (_sec == 60) 
				{
					_min++;
					_sec = 0;
				}

				ReturnMinSecInt?.Invoke(_min, _sec);
				ConvertIntTimeToString(_min, _sec);
				yield return new WaitForSecondsRealtime(1f);
			}

			tempCor = null;
		}
		private void ConvertIntTimeToString(int _min, int _sec) {
			string tempString = String.Format(_formatTime, _min.ToString("D2"), _sec.ToString("D2"));
			ReturnMinSecString?.Invoke(tempString);
		}
		private void FinishLevelReaction(LevelResult _levelResult) {
			if (_levelResult == LevelResult.Win) {
				StopTimer();
				ConvertIntTimeToString(0, 0);
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
