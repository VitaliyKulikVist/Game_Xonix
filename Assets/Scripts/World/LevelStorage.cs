using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.World {
	[CreateAssetMenu(menuName = "ScriptableObjects/LevelStorage", fileName = "LevelStorage")]
	public class LevelStorage : ScriptableObject {
		[Header("Level Settings")]
		[SerializeField] private BaseLevelSettings _baseLevelSettings = default;

		[Header("Levels Storage")]
		[SerializeField] private List<Level> _gameLevels = new List<Level>();

		#region Get/Set
		public BaseLevelSettings BaseLevelSettings { get => _baseLevelSettings; }
		#endregion

		public Level GetNextLevel(int _currentLVL) {
			return (Level)_gameLevels[_currentLVL % _gameLevels.Count].Clone();
		}
	}
}
