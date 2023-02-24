using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.World.Grid.Trigger {
	public class UnitLandTrigger : UnitTrigger<GridUnitLandType> {
		protected override void OnEnable() {
			base.OnEnable();

		}
		#region Tag Controll
		protected override void SwitchTag() {
			base.SwitchTag();

			if(gameObject.tag.Equals(Tags.Land)) {
				gameObject.tag = Tags.Sea;
			}
		}

		public override void ResetTagToDefault() {
			base.ResetTagToDefault();
			gameObject.tag = Tags.Land;
		}
		#endregion

		protected override void OnTriggerEnter(Collider other) {
			base.OnTriggerEnter(other);
			if(isActiveAndEnabled) {
				if (other.gameObject.tag.Equals(Tags.Character)) {
					_gridUnit.ReactionToHitCharacter(other, _gridUnit);
				}

				else if (other.gameObject.tag.Equals(Tags.EnemySea)) {
					_gridUnit.ReactionToHitSeaEnemy(other);
				}
			}
		}
	}
}
