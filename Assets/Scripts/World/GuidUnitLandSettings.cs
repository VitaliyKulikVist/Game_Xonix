using System;
using Assets.Scripts.Common;
using Assets.Scripts.World.Grid;
using Assets.Scripts.World.Grid.AssetReference;
using UnityEngine;

namespace Assets.Scripts.World {
	[Serializable]
	public class GuidUnitLandSettings {
		[field: SerializeField] public GridUnitLandType GridUnitLandType { get; set; }
		[field: SerializeField] public AssetReferenceGridLand AssetReference { get; set; }
		[field:SerializeField]public GridUnitLand GridUnit { get; set; }
	}
}
