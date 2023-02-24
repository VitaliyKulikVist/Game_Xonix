using Assets.Scripts.Character.Trigger;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.World.Grid {
	public class GridUnitLand : GridUnit<GridUnitLandType> {
		public override void ShowUnit(UnityEngine.Vector3 _startPosition, GridUnitLandType enemyType) {
			base.ShowUnit(_startPosition, enemyType);

			if (_gridUnitType != enemyType) {
				return;
			}
		}

		public override void ReactionToHitCharacter(Collider colliderHitObject, GridUnit<GridUnitLandType> gridUnit) {
			base.ReactionToHitCharacter(colliderHitObject, gridUnit);

			colliderHitObject.gameObject.GetComponent<CharacterTrigger>().HitLandUnit(gridUnit);
		}
	}
}
