using System;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Assets.Scripts.Enemies.Storage {
	[Serializable]
	public class EnemyUnit {
		[Header("Enemy parameters")]
		[SerializeField] private EnemyType _enemyType = EnemyType.None;

		[Header("Enemy components")]
		[SerializeField] private List<EnemyControllerSettings> _enemyControllers = new List<EnemyControllerSettings>();

		#region Get/Set
		public EnemyType GetEnemyType { get => _enemyType; }
		public EnemyControllerAbstract GetEnemyController { get => _enemyControllers[Random.Range(0, _enemyControllers.Count)].EnemyController; }
		#endregion

		public void SetEnemyControllerType() {
			_enemyControllers.ForEach(all => all.EnemyController.SetEnemyType(_enemyType));
		}

		public void SetEnemyControllerSpeed() {

			foreach (var settings in _enemyControllers) {
				settings.EnemyController.SetEnemySpeed(settings.EnemySpeed);
			}
		}
	}

	[Serializable]
	public class EnemyControllerSettings {
		[field:SerializeField] public EnemyControllerAbstract EnemyController { get;private set; }
		[field: SerializeField] public float EnemySpeed { get; private set; }
	}
}
