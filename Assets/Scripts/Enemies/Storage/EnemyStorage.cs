using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Enemies.Storage {

	[CreateAssetMenu(menuName = "ScriptableObjects/EnemyStorage", fileName = "EnemyStorage")]
	public class EnemyStorage : ScriptableObject {

		[Header("Enemys")]
		[SerializeField] private List<EnemySeaUnit> _enemySeaUnits = new List<EnemySeaUnit>();
		[SerializeField] private List<EnemyLandUnit> _enemyLandUnits = new List<EnemyLandUnit>();

#if UNITY_EDITOR
		[Header("OnValidate")]
		[SerializeField] private bool _SetEnemyControllerType = false;
		[SerializeField] private bool _SetEnemyControllerSpeed = false;
		[SerializeField] private bool _SetEnemyAttackDistance = false;
#endif

		#region Get/Set
		public List<EnemySeaUnit> GetEnemySeaUnits { get => _enemySeaUnits; }
		public List<EnemyLandUnit> GetEnemyLandUnits { get => _enemyLandUnits; }
		#endregion

		public EnemySeaUnit GetSeaEnemyByType(EnemySeaType _enemyType) {
			return _enemySeaUnits.Find(someUnit => someUnit.GetEnemyType == _enemyType);
		}
		public EnemyLandUnit GetLandEnemyByType(EnemyLandType _enemyType) {
			return _enemyLandUnits.Find(someUnit => someUnit.GetEnemyType == _enemyType);
		}
		public UnityEngine.AddressableAssets.AssetReference GetAssetReferenceSeaUnitByType(EnemySeaType enemySeaType) {
			return _enemySeaUnits.Find(someUnit => someUnit.GetEnemyType == enemySeaType).AssetReferencec;
		}
		public UnityEngine.AddressableAssets.AssetReference GetAssetReferenceLandUnitByType(EnemyLandType enemyLandType) {
			return _enemyLandUnits.Find(someUnit => someUnit.GetEnemyType == enemyLandType).AssetReferencec;
		}


#if UNITY_EDITOR
		private void OnValidate() {
			if (_SetEnemyControllerType) {
				_enemySeaUnits.ForEach(en => en.SetEnemyControllerType());
				_enemyLandUnits.ForEach(en => en.SetEnemyControllerType());
			}

			if (_SetEnemyControllerSpeed) {
				_enemySeaUnits.ForEach(en => en.SetEnemyMoveSpeed());
				_enemyLandUnits.ForEach(en => en.SetEnemyMoveSpeed());
			}
			if (_SetEnemyAttackDistance) {
				_enemySeaUnits.ForEach(en => en.SetEnemyAttackDistance());
				_enemyLandUnits.ForEach(en => en.SetEnemyAttackDistance());
			}
		}
#endif

	}
}
