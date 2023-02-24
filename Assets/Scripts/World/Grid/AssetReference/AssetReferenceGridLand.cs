using System;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.World.Grid.AssetReference {
	[Serializable]
	public class AssetReferenceGridLand : AssetReferenceT<GridUnitLand> {
		public AssetReferenceGridLand(string guid) : base(guid) {
		}
	}
}
