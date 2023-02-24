using System;
using Assets.Scripts.Common;
using Assets.Scripts.World.Grid;
using Assets.Scripts.World.Grid.AssetReference;
using UnityEngine;

namespace Assets.Scripts.World {
	[Serializable]
	public class GuidUnitSeaSettings {
		[field: SerializeField] public GridUnitSeaType GridUnitSeaType { get; set; }
		[field: SerializeField] public AssetReferenceGridSea AssetReference { get; set; }
		[field: SerializeField] public GridUnitSea GridUnit { get; set; }
	}
}
