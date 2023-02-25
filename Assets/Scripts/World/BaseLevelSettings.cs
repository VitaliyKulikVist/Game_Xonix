using System;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.World.Grid;
using Assets.Scripts.World.Grid.AssetReference;
using UnityEngine;

namespace Assets.Scripts.World {
	[Serializable]
	public class BaseLevelSettings {
		[Header("Grid Units")]
		[SerializeField] private List<GuidUnitSeaSettings> _seaGridUnits = default;
		[SerializeField] private List<GuidUnitLandSettings> _landGridUnits = default;

		#region Set Enum
		public void SetSeaGridUnitsEnum() {
			foreach (var sea in _seaGridUnits) {
				var gridUnitSea = sea.GridUnit;
				gridUnitSea.SetType(sea.GridUnitSeaType);
				//PrefabUtility.SavePrefabAsset(gridUnitSea.gameObject);
			}
		}
		public void SetLandGridUnitsEnum() {
			foreach (var land in _landGridUnits) {
				var gridUnitLand = land.GridUnit;
				gridUnitLand.SetType(land.GridUnitLandType);
				//PrefabUtility.SavePrefabAsset(gridUnitLand.gameObject);
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

		public AssetReferenceGridSea GetAssetReferenceSeaByType(GridUnitSeaType gridUnitSeaType) {
			return _seaGridUnits.Find(unit => unit.GridUnitSeaType == gridUnitSeaType).AssetReference;
		}
		public AssetReferenceGridLand GetAssetReferenceLandByType(GridUnitLandType gridUnitLandType) {
			return _landGridUnits.Find(unit => unit.GridUnitLandType == gridUnitLandType).AssetReference;
		}
		#endregion
	}
}
