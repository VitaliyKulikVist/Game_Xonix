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
		public static Action<LevelResult> LevelFinishAction = default;

		public static Action PausedLevelAction = default;
		public static Action PlayLevelAction = default;
		#endregion

		private void Start() {
			_playerStorageSO?.LoadPlayer();
		}

		private void OnEnable() {
			LevelFinishAction += FinishLevel;

			DOTween.SetTweensCapacity(2500, 100);

			Application.targetFrameRate = _targetFrameRate;
		}

		private void OnDisable() {
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

		private void FinishLevel(LevelResult _levelResult) {
			if (_levelResult == LevelResult.Win) {
				_playerStorageSO.ConcretePlayer.AddPlayerLevel(1);
			}
		}
	}
}


