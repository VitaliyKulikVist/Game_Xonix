using Assets.Scripts.Common;

namespace Assets.Scripts.World.Grid {
	public class GridUnitLand : GridUnit<GridUnitLandType> {
		public override void ShowUnit(UnityEngine.Vector3 _startPosition, GridUnitLandType enemyType) {
			base.ShowUnit(_startPosition, enemyType);

			if (_gridUnitType != enemyType) {
				return;
			}
		}
	}
}
