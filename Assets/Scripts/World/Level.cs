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

		[field: SerializeField] public GridUnitLandType LandType { get; set; } = GridUnitLandType.Land_1;
		[field: SerializeField] public GridUnitSeaType SeaType { get; set; } = GridUnitSeaType.Sea_1;

		public object Clone() {
			Level tempCloneLevel = new Level();

			tempCloneLevel.EnemiesSea = this.EnemiesSea;
			tempCloneLevel.EnemiesLand = this.EnemiesLand;
			tempCloneLevel.LandType = this.LandType;
			tempCloneLevel.SeaType = this.SeaType;

			return tempCloneLevel;
		}
	}
}
