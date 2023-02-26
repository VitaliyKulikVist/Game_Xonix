using System.Collections.Generic;
using Assets.Scripts.Common.Helpers;
using UnityEngine;

namespace Assets.Scripts.World {
	[CreateAssetMenu(menuName = "ScriptableObjects/LevelStorage", fileName = "LevelStorage")]
	public class LevelStorage : ScriptableObject, IValidateHalper {
		[Header("Level Settings")]
		[SerializeField, Range(1, 86399)] private int _levelDurationAtSecond = 60;
		[SerializeField] private BaseLevelSettings _baseLevelSettings = default;

		[Header("Levels Storage")]
		[SerializeField] private List<Level> _gameLevels = new List<Level>();

		[field: Header("OnValidate"), Tooltip("Set enum by Grid Unit")]
		[field: SerializeField] public bool IsValidate { get; set; }

		#region Get/Set
		public BaseLevelSettings BaseLevelSettings { get => _baseLevelSettings; }
		public int LevelDuration { get => _levelDurationAtSecond; }
		#endregion

		public Level GetNextLevel(int _currentLVL) {
			return (Level)_gameLevels[_currentLVL % _gameLevels.Count].Clone();
		}

		public void OnValidate() {
			if (IsValidate) {
				_baseLevelSettings.SetLandGridUnitsEnum();
				_baseLevelSettings.SetSeaGridUnitsEnum();
			}
		}
	}
}
