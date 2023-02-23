using Assets.Scripts.Common;

namespace Assets.Scripts.World.Grid {
	public class GridUnitSea : GridUnit<GridUnitSeaType> {
		public override void ShowUnit(UnityEngine.Vector3 _startPosition, GridUnitSeaType enemyType) {
			base.ShowUnit(_startPosition, enemyType);

			if (_gridUnitType != enemyType) {
				return;
			}
		}
	}
}
