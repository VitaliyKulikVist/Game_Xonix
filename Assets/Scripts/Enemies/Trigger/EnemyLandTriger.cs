using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Enemies.Trigger {
	public class EnemyLandTriger : EnemyTriger<EnemyLandType> {

		protected override void OnEnable() {
			base.OnEnable();

		}

		protected override void OnTriggerEnter(Collider other) {
			base.OnTriggerEnter(other);
			if (isActiveAndEnabled && other.gameObject.tag.Equals(Tags.Sea)) {
				_enemyController.ReactionOnSea(other);
			}
		}

		public override void Damage(Collider collider) {
			base.Damage(collider);

		}
	}
}
