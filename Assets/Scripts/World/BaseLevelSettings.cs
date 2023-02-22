using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Assets.Scripts.World {
	[Serializable]
	public class BaseLevelSettings {
		[Header("Ground materials")]
		[SerializeField] private List<Sprite> _seaSprite = default;
		[SerializeField] private List<Sprite> _landSprite = default;

		#region Get/Set
		public Sprite GetRandomSeaSprite { get => _seaSprite[Random.Range(0, _seaSprite.Count)]; }
		public Sprite GetRandomLandSprite { get => _landSprite[Random.Range(0, _landSprite.Count)]; }
		#endregion
	}
}
