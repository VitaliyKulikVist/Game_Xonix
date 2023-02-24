using System;
using Assets.Scripts.Common;
using Assets.Scripts.Enemies.Storage;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Enemies {
	public class Enemy<TEnum> : MonoBehaviour
		where TEnum : System.Enum {

		[Header("Base")]
		[SerializeField] protected DependencyInjections _dependencyInjections = default;
		[SerializeField] protected EnemyStorage enemyStorageSO = default;

		[Header("Enemy settings Just Visual View, Don't touched")]
		[SerializeField] protected TEnum _enemyType = default;
		[SerializeField] protected float _enemySpeed = default;
		[SerializeField] protected float _attackDistance = 3f;

		[Header("Enemy Components")]
		[SerializeField] protected RectTransform enemyContainer = default;

		#region Variables
		protected Vector3 target = default;
		#endregion

		#region Get\Set
		public bool IsKilled { get; set; } = false;
		public TEnum EnemyType { get => _enemyType; }
		public float EnemySpeed { get => _enemySpeed; }
		public bool IsFree { get; set; } = false;
		#endregion

		#region Action
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

		public void SetEnemyType(TEnum enemyType) {
			_enemyType = enemyType;
		}
		public void SetEnemySpeed(float enemySpeed) {
			_enemySpeed = enemySpeed;
		}

		public void SetEnemyAttackDistance(float attackDistance) {
			_attackDistance = attackDistance;
		}
	}
}

