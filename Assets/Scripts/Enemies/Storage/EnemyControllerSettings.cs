using System;
using UnityEngine;

namespace Assets.Scripts.Enemies.Storage {
	[Serializable]
	public class EnemyControllerSettings<TEnums>
		where TEnums : System.Enum {
		[field:SerializeField] public EnemyControllerAbstract<TEnums> EnemyController { get;private set; }
		[field: SerializeField] public float EnemySpeed { get; private set; }
	}
}
