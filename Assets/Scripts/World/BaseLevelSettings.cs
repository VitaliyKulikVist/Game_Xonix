using System;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.World.Grid;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Assets.Scripts.World {
	[Serializable]
	public class BaseLevelSettings {
		[Header("Grid Units")]
		[SerializeField] private List<GuidUnitSeaSettings> _seaGridUnits = default;
		[SerializeField] private List<GuidUnitLandSettings> _landGridUnits = default;

		#region Get/Set
		public GridUnitSea GetRandomSeaGridUnit { get => _seaGridUnits[Random.Range(0, _seaGridUnits.Count)].GridUnit; }
		public GridUnitLand GetRandomLandGridUnit { get => _landGridUnits[Random.Range(0, _landGridUnits.Count)].GridUnit; }
		#endregion

		#region Set Enum
		public void SetSeaGridUnitsEnum() {
			foreach (var sea in _seaGridUnits) {
				sea.GridUnit.SetType(sea.GridUnitSeaType);
			}
		}
		public void SetLandGridUnitsEnum() {
			foreach (var land in _landGridUnits) {
				land.GridUnit.SetType(land.GridUnitLandType);
			}
		}
		#endregion

		#region Get
		public GridUnitSea GetGridUnitSeaByType(GridUnitSeaType gridUnitSeaType) {
			return _seaGridUnits.Find(unit => unit.GridUnitSeaType == gridUnitSeaType).GridUnit;
		}

		public GridUnitLand GetGridUnitLandByType(GridUnitLandType gridUnitLandType) {
			return _landGridUnits.Find(unit => unit.GridUnitLandType == gridUnitLandType).GridUnit;
		}
		#endregion
	}
}
