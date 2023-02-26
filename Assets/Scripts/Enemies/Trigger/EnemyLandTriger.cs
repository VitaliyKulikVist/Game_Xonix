using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Enemies.Trigger {
	public class EnemyLandTriger : EnemyTriger<EnemyLandType> {

		protected override void OnEnable() {
			base.OnEnable();

		}

		protected override void OnTriggerEnter2D(Collider2D other) {
			base.OnTriggerEnter2D(other);
			if (isActiveAndEnabled && other.gameObject.tag.Equals(Tags.Sea)) {
				_enemyController.ReactionOnSea(other);

				if (_debug) {
					Debug.Log($"<color=yellow>Enemy trigger Sea</color> {_enemyController.EnemyType} Hit {other.gameObject.name}");
				}
			}
		}

		public override void Damage(Collider2D collider) {
			base.Damage(collider);

		}
	}
}
