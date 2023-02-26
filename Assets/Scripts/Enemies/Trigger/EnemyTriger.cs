using System;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Enemies.Trigger {
	public abstract class EnemyTriger<TEnum> : MonoBehaviour
		where TEnum : Enum {

		[Header("Debug")]
		[SerializeField] protected bool _debug = false;

		#region Variables
		protected EnemyControllerAbstract<TEnum> _enemyController = default;
		#endregion

		protected virtual void OnEnable() {
			_enemyController = GetComponentInParent<EnemyControllerAbstract<TEnum>>();
		}

		protected virtual void OnTriggerEnter2D(Collider2D other) {

			Debug.Log($"<color=yellow>Enemy trigger</color> {_enemyController.EnemyType} Hit {other.gameObject.name}");

			if (isActiveAndEnabled && other.gameObject.tag.Equals(Tags.Character)) {
				_enemyController.ReactionOnCharacter(other);

				if(_debug) {
					Debug.Log($"<color=yellow>Enemy trigger Player</color> {_enemyController.EnemyType} Hit {other.gameObject.name}");
				}
			}
		}

		public virtual void Damage(Collider2D collider) {
			_enemyController.GetDamage();
		}
	}
}
