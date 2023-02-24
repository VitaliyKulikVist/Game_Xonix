using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Enemies.Trigger {
	public class EnemySeaTriger: EnemyTriger<EnemySeaType> {

		protected override void OnEnable() {
			base.OnEnable();

		}

		protected override void OnTriggerEnter(Collider other) {
			base.OnTriggerEnter(other);
			if(isActiveAndEnabled && other.gameObject.tag.Equals(Tags.Land)) {
				_enemyController.ReactionOnLand(other);
			}
		}

		public override void Damage(Collider collider) {
			base.Damage(collider);

		}
	}
}
