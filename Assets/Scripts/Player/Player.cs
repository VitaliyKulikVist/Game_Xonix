using Assets.Scripts.Common;
using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Scripts.Player {
	[Serializable]
	public class Player {
		[Header("Player's Property")]
		[SerializeField] private int _playerLive = BasePlayerConstants.MaxPlayerLife;
		[SerializeField] private int _playerLevel = BasePlayerConstants.MinPlayerLevel;

		#region Actions
		public static Action PlayerChangeliveAction = default;
		public static Action PlayerChangeLevelAction = default;
		#endregion

		#region Variable
		private Tween _liveTween = default;
		#endregion

		#region Geters/Seters
		public int PlayerLive { get => _playerLive; }
		public int PlayerLevel { get => _playerLevel; }
		#endregion

		public Player() {
			_playerLive = BasePlayerConstants.MaxPlayerLife;
			_playerLevel = BasePlayerConstants.MinPlayerLevel;
		}

		public void Changelive(int live) {
			if (_playerLive <= 0) {
				return;
			}

			else {
				if (_playerLive > 0) {
					_liveTween?.Complete();
				}
				_liveTween = DOTween.To(() =>
				_playerLive, (tempCoins) =>
				_playerLive = tempCoins, _playerLive + live, .5f)
					.SetEase(Ease.Flash)
					.OnUpdate(() => {
						PlayerChangeliveAction?.Invoke();
					}).OnComplete(() => {
						if (_playerLive <= 0) {
							_playerLive = 0;

							GameManager.LevelFinishAction?.Invoke(LevelResult.Lose);

							return;
						}
					});
			}
		}

		public void ResetPlayerLive() {
			_playerLive = BasePlayerConstants.MaxPlayerLife;
		}

		public void AddPlayerLevel(int count) {
			_playerLevel += count;

			PlayerChangeLevelAction?.Invoke();
		}
	}
}
