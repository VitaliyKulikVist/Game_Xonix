using System;
using Assets.Scripts.Common;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.World {
	[Serializable]
	public class Level : ICloneable {
		[Header("Enemies type at this level")]
		[SerializeField] private List<EnemyType> _enemies = new List<EnemyType>();

		[Header("Maximum enemies on the level")]
		[SerializeField] private int _maxEnemies = 5;
		[SerializeField] private float _timeSpawnEnemies = 0.5f;

		#region Geter/Seter
		public List<EnemyType> GetLevelEnemies { get => _enemies; }

		public float GetTimeSpawnEnemies { get => _timeSpawnEnemies; }
		public int MaximumEnemies { get => _maxEnemies; }
		#endregion

		public object Clone() {
			Level copyLevel = new Level();
			copyLevel._maxEnemies = this._maxEnemies;
			copyLevel._enemies = new List<EnemyType>(this._enemies);

			return copyLevel;
		}
	}
}
