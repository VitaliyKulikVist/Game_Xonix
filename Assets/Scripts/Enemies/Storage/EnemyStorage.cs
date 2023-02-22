using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Enemies.Storage {

	[CreateAssetMenu(menuName = "ScriptableObjects/EnemyStorage", fileName = "EnemyStorage")]
	public class EnemyStorage : ScriptableObject {

		[Header("Enemys")]
		[SerializeField] private List<EnemyUnit> _enemyUnits = new List<EnemyUnit>();

#if UNITY_EDITOR
		[Header("OnValidate")]
		[SerializeField] private bool _SetEnemyControllerType = false;
		[SerializeField] private bool _SetEnemyControllerSpeed = false;
#endif

		#region Get/Set
		public List<EnemyUnit> GetEnemyUnits { get => _enemyUnits; }
		#endregion

		public EnemyUnit GetEnemyByType(EnemyType _enemyType) {
			return _enemyUnits.Find(someUnit => someUnit.GetEnemyType == _enemyType);
		}

#if UNITY_EDITOR
		private void OnValidate() {
			if (_SetEnemyControllerType) {
				_enemyUnits.ForEach(en => en.SetEnemyControllerType());
			}

			if(_SetEnemyControllerSpeed) {
				_enemyUnits.ForEach(en => en.SetEnemyControllerSpeed());
			}
		}
#endif

	}
}
