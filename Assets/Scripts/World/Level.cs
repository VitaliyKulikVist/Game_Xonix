using System;
using Assets.Scripts.Common;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.World {
	[Serializable]
	public class Level : ICloneable {
		[field: Header("Enemies type at this level")]
		[field: SerializeField] public List<EnemySeaType> EnemiesSea { get; set; } = new List<EnemySeaType>();
		[field: SerializeField] public List<EnemyLandType> EnemiesLand { get; set; } = new List<EnemyLandType>();

		public object Clone() {
			Level copyLevel = new Level();
			copyLevel.EnemiesSea = this.EnemiesSea;
			copyLevel.EnemiesLand = this.EnemiesLand;

			return copyLevel;
		}
	}
}
