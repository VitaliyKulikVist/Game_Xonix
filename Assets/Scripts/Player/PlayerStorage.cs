using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Player {
	[CreateAssetMenu(menuName = "ScriptableObjects/PlayerStorage", fileName = "PlayerStorage")]
	public class PlayerStorage : ScriptableObject {
		[Header("Save Settings")]
		[SerializeField] private string _playerSaveStringAtPrefs = "Game_Save_01";

		[field: Header("Concrete Player")]
		[field: SerializeField] public Player ConcretePlayer { get; set; } = null;

		public void SavePlayer() {
			if (ConcretePlayer != null) {
				string jsonPlayer = JsonUtility.ToJson(ConcretePlayer);

				PlayerPrefs.SetString(_playerSaveStringAtPrefs, jsonPlayer);
				PlayerPrefs.Save();
			}
		}

		public void LoadPlayer() {
			var playerString = PlayerPrefs.GetString(_playerSaveStringAtPrefs, string.Empty);
			if (PlayerPrefs.HasKey(_playerSaveStringAtPrefs) && playerString != string.Empty) {
				ConcretePlayer = JsonUtility.FromJson<Player>(playerString);
			}
			else {
				ConcretePlayer = new Player();

				PlayerPrefs.DeleteAll();
			}

			GameManager.LevelStartAction?.Invoke();
		}
	}
}
