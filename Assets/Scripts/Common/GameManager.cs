using Assets.Scripts.Player;
using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Scripts.Common {
	public class GameManager : MonoBehaviour {
		[Header("Base")]
		[SerializeField] private PlayerStorage _playerStorageSO = default;

		[Header("Settings")]
		[SerializeField] private int _targetFrameRate = 120;

		[field: Header("Just visual control")]
		[field: SerializeField] public bool IsPaused { get; private set; } = false;

		#region Global Actions
		public static Action LevelStartAction = default;
		public static Action RestartLevelAction = default;
		public static Action<LevelResult> LevelFinishAction = default;

		public static Action PausedLevelAction = default;
		public static Action PlayLevelAction = default;
		#endregion

		#region Variables
		private float _startLevelTime = default;
		#endregion

		private void Start() {
			_playerStorageSO?.LoadPlayer();
		}

		private void OnEnable() {
			LevelStartAction += StartLevel;
			LevelFinishAction += FinishLevel;

			DOTween.SetTweensCapacity(2500, 100);

			Application.targetFrameRate = _targetFrameRate;
		}

		private void OnDisable() {
			LevelStartAction -= StartLevel;
			LevelFinishAction -= FinishLevel;

			

			_playerStorageSO.SavePlayer();
		}

		private void OnDestroy() {
			_playerStorageSO.SavePlayer();
		}

		private void OnApplicationQuit() {
			_playerStorageSO.SavePlayer();
		}

		#region Pause Controll

		void OnGUI() {
			if (IsPaused)
				GUI.Label(new Rect(1000, 1000, 50, 30), "Game paused");
		}

		void OnApplicationFocus(bool hasFocus) {
			IsPaused = !hasFocus;
		}

		void OnApplicationPause(bool pauseStatus) {
			IsPaused = pauseStatus;
			if (pauseStatus) {
				_playerStorageSO.SavePlayer();
			}
		}
		#endregion

		private void StartLevel() {
			_startLevelTime = Time.time;
		}

		private void FinishLevel(LevelResult _levelResult) {
			if (_levelResult == LevelResult.Win) {
				_playerStorageSO.ConcretePlayer.AddPlayerLevel(1);
			}
			
			else if (_levelResult == LevelResult.Lose && _playerStorageSO.ConcretePlayer.PlayerLive <= 0) {
				RestartLevelAction?.Invoke();
			}
		}
	}
}


