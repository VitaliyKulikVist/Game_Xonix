using System;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.World.Grid.AssetReference {
	[Serializable]
	public class AssetReferenceGridSea : AssetReferenceT<GridUnitSea> {
		public AssetReferenceGridSea(string guid) : base(guid) {
		}
	}
}
