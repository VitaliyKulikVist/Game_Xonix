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

		protected override void OnTriggerEnter2D(Collider2D other) {
			base.OnTriggerEnter2D(other);
			if(isActiveAndEnabled) {
				if (other.gameObject.tag.Equals(Tags.Character)) {
					_gridUnit.ReactionToHitCharacter(other, _gridUnit);

					if (_debug) {
						Debug.Log($"<color=green>On Trigger Enter Player Land</color> {_gridUnit.GetGridUnitType}\t\t {other.gameObject.name}");
					}
				}

				else if (other.gameObject.tag.Equals(Tags.EnemySea)) {
					_gridUnit.ReactionToHitSeaEnemy(other);
					if (_debug) {
						Debug.Log($"<color=green>On Trigger Enter Enemy Sea</color> {_gridUnit.GetGridUnitType}\t\t {other.gameObject.name}");
					}
				}
			}
		}
	}
}
