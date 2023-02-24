using Assets.Scripts.Character.Trigger;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.World.Grid {
	public class GridUnitSea : GridUnit<GridUnitSeaType> {
		public override void ShowUnit(UnityEngine.Vector3 _startPosition, GridUnitSeaType enemyType) {
			base.ShowUnit(_startPosition, enemyType);

			if (_gridUnitType != enemyType) {
				return;
			}
		}

		public override void ReactionToHitCharacter(Collider colliderHitObject, GridUnit<GridUnitSeaType> gridUnit) {
			base.ReactionToHitCharacter(colliderHitObject, gridUnit);

			colliderHitObject.gameObject.GetComponent<CharacterTrigger>().HitSeaUnit(gridUnit);
		}
	}
}
