using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Enemies.Trigger {
	public class EnemySeaTriger: EnemyTriger<EnemySeaType> {

		protected override void OnEnable() {
			base.OnEnable();

		}

		protected override void OnTriggerEnter2D(Collider2D other) {
			base.OnTriggerEnter2D(other);
			if(isActiveAndEnabled && other.gameObject.tag.Equals(Tags.Land)) {
				_enemyController.ReactionOnLand(other);

				if (_debug) {
					Debug.Log($"<color=green>Enemy trigger Sea</color> {_enemyController.EnemyType} Hit {other.gameObject.name}");
				}
			}
		}

		public override void Damage(Collider2D collider) {
			base.Damage(collider);

		}
	}
}
