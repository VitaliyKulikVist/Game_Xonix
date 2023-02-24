using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Enemies.Trigger {
	public abstract class EnemyTriger<TEnum> : MonoBehaviour
		where TEnum : System.Enum {

		#region Variables
		protected EnemyControllerAbstract<TEnum> _enemyController = default;
		#endregion

		protected virtual void OnEnable() {
			_enemyController = GetComponentInParent<EnemyControllerAbstract<TEnum>>();
		}

		protected virtual void OnTriggerEnter(Collider other) {
			if (isActiveAndEnabled && other.gameObject.tag.Equals(Tags.Character)) {
				_enemyController.ReactionOnCharacter(other);
			}
		}

		public virtual void Damage(Collider collider) {
			_enemyController.GetDamage();
		}
	}
}
