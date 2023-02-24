using System;
using Assets.Scripts.Common;

namespace Assets.Scripts.Enemies.Storage {
	[Serializable]
	public class EnemySeaUnit: EnemyUnit<EnemySeaType> {
		public override void SetEnemyControllerType() {
			base.SetEnemyControllerType();

		}

		public override void SetEnemyMoveSpeed() {
			base.SetEnemyMoveSpeed();

		}

		public override void SetEnemyAttackDistance() {
			base.SetEnemyAttackDistance();

		}
	}
}
