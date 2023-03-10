using System;
using UnityEngine;

namespace Assets.Scripts.Enemies.Storage {
	[Serializable]
	public abstract class EnemyUnit<TEnums>
		where TEnums : Enum {
		[Header("Enemy parameters")]
		[SerializeField] protected TEnums _enemyType = default;
		[SerializeField] protected float _enemySpeed = 0f;
		[SerializeField] protected float _attackDistance = 0f;

		[field: Header("Enemy components")]
		[field: SerializeField] public EnemyControllerAbstract<TEnums> EnemyController { get; private set; }
		[field: SerializeField] public UnityEngine.AddressableAssets.AssetReference AssetReferencec { get; private set; }

		#region Get/Set
		public TEnums GetEnemyType { get => _enemyType; }
		public float EnemySpeed { get => _enemySpeed; }
		public float AttackDistance { get => _attackDistance; }
		#endregion

		public virtual void SetEnemyControllerType() {
			EnemyController.SetEnemyType(_enemyType);

			var convertToType = AssetReferencec as EnemyControllerAbstract<TEnums>;
			if(convertToType!=null) {
				convertToType.SetEnemyType(_enemyType);
			}
			//PrefabUtility.SavePrefabAsset(EnemyController.gameObject);
		}

		public virtual void SetEnemyMoveSpeed() {
			EnemyController.SetEnemySpeed(_enemySpeed);

			var convertToType = AssetReferencec as EnemyControllerAbstract<TEnums>;
			if (convertToType != null) {
				convertToType.SetEnemySpeed(_enemySpeed);
			}
		}

		public virtual void SetEnemyAttackDistance() {
			EnemyController.SetEnemyAttackDistance(_attackDistance);

			var convertToType = AssetReferencec as EnemyControllerAbstract<TEnums>;
			if (convertToType != null) {
				convertToType.SetEnemyAttackDistance(_attackDistance);
			}
		}
	}
}
