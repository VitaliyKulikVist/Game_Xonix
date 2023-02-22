using System;
using Assets.Scripts.Common;
using Assets.Scripts.Enemies.Storage;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Enemies {
	public class Enemy : MonoBehaviour {

		[Header("Base")]
		[SerializeField] protected DependencyInjections _dependencyInjections = default;
		[SerializeField] protected EnemyStorage enemyStorageSO = default;

		[Header("Enemy settings Just Visual View, Don't touched")]
		[SerializeField] protected EnemyType _enemyType = EnemyType.None;
		[SerializeField] protected float _enemySpeed = default;
		[SerializeField] protected float _attackDistance = 3f;

		[Header("Enemy Components")]
		[SerializeField] protected RectTransform enemyContainer = default;

		#region Variables
		protected Vector3 target = default;
		#endregion

		#region Get\Set
		public bool IsKilled { get; set; } = false;
		public EnemyType EnemyType { get => _enemyType; }
		public float EnemySpeed { get => _enemySpeed; }
		public bool IsFree { get; set; } = false;
		#endregion

		#region Action
		public static Action<EnemyControllerAbstract> EnemyKilledAction = default;
		public static Action<EnemyControllerAbstract> EnemyRichPlayerAction = default;
		public static Action KillEnemies = default;
		#endregion

		public void ResetEnemy() {
			transform.DOKill();
			enemyContainer.DOKill();

			transform.position= Vector3.zero;
			transform.rotation= Quaternion.identity;
			transform.localScale= Vector3.one;

			enemyContainer.position= Vector3.zero;
			enemyContainer.rotation= Quaternion.identity;
			enemyContainer.localScale= Vector3.one;
		}

		public void SetEnemyType(EnemyType enemyType) {
			_enemyType = enemyType;
		}
		public void SetEnemySpeed(float enemyspeed) {
			_enemySpeed = enemyspeed;
		}
	}
}

