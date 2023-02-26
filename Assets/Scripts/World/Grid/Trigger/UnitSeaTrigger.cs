using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.World.Grid.Trigger {
	public class UnitSeaTrigger : UnitTrigger<GridUnitSeaType> {
		protected override void OnEnable() {
			base.OnEnable();

		}
		#region Tag Controll
		protected override void SwitchTag() {
			base.SwitchTag();

			if (gameObject.tag.Equals(Tags.Sea)) {
				gameObject.tag = Tags.Land;
			}
		}

		public override void ResetTagToDefault() {
			base.ResetTagToDefault();
			gameObject.tag = Tags.Sea;
		}

		#endregion

		protected override void OnTriggerEnter2D(Collider2D other) {
			base.OnTriggerEnter2D(other);
			if (isActiveAndEnabled && other.gameObject.tag.Equals(Tags.EnemyLand)) {
				_gridUnit.ReactionToHitLandEnemy(other);
			}
		}
	}
}
