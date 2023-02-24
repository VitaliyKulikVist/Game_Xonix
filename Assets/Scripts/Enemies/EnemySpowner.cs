using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Helpers;
using Assets.Scripts.Enemies.Storage;
using Assets.Scripts.Player;
using Assets.Scripts.World;
using UnityEngine;

namespace Assets.Scripts.Enemies {
	public class EnemySpowner : MonoBehaviour, IValidateHalper {
		[Header("Base")]
		[SerializeField] private DependencyInjections _dependencyInjections = default;
		[SerializeField] private EnemyStorage _enemyStorageSO = default;
		[SerializeField] private GameStorageSettings _gameStorageSettingsSO = default;
		[SerializeField] private LevelStorage _levelStorageSO = default;
		[SerializeField] private PlayerStorage _playerStorageSO = default;

		[Header("Enemy")]
		[SerializeField] private Transform _enemiesContainer = default;
		[SerializeField] private Transform _containerSea = default;
		[SerializeField] private Transform _containerLand = default;

		[Header("Pool settings")]
		[SerializeField] private int _startingEnemiesCountPool = 30;

		[field: Header("On Validate")]
		[field: SerializeField] public bool IsValidate { get; set; }

		#region Variables
		private Coroutine _spownEnemiesCoroutine = null;
		private Coroutine _selectNearTargetCoroutine = null;
		private List<EnemyControllerAbstract<EnemySeaType>> _enemysSea = 
			new List<EnemyControllerAbstract<EnemySeaType>>();
		private List<EnemyControllerAbstract<EnemyLandType>> _enemysLand = 
			new List<EnemyControllerAbstract<EnemyLandType>>();
		#endregion

		#region Get\Set
		public bool LevelStarted { get; set; } = false;
		#endregion

		private void Awake() {
			PrepareEnemys();
			PrepareCamera();
		}

		private void OnEnable() {
			GameManager.LevelStartAction += ReactionStartLevel;
			GameManager.LevelFinishAction += ReactionFinishLevel;
		}

		private void OnDisable() {
			GameManager.LevelStartAction -= ReactionStartLevel;
			GameManager.LevelFinishAction -= ReactionFinishLevel;
		}

		#region Reaction
		private void ReactionStartLevel() {
			LevelStarted = true;
		}
		private void ReactionFinishLevel(LevelResult levelResult) {
			if(levelResult == LevelResult.Win) {
				LevelStarted = true;
			}
		}
		#endregion

		private void PrepareEnemys() {
			for (int i = 0; i < _startingEnemiesCountPool; i++) {
				
				for (int j = 0; j < typeof(EnemySeaType).GetEnumValues().Length; j++) {
					if((EnemySeaType)j != EnemySeaType.None) {
						AddSeaEnemy((EnemySeaType)j);
					}
				}

				for (int j = 0; j < typeof(EnemyLandType).GetEnumValues().Length; j++) {
					if ((EnemyLandType)j != EnemyLandType.None) {
						AddLandEnemy((EnemyLandType)j);
					}
				}
			}
		}
		private void AddSeaEnemy(EnemySeaType seaEnemyType) {
			var enemyControllerAbstract = _enemyStorageSO.GetSeaEnemyByType(seaEnemyType).EnemyController;

			var enemy = Instantiate(enemyControllerAbstract, _containerSea);
			enemy.ResetEnemy();

			_enemysSea.Add(enemy);
		}

		private void AddLandEnemy(EnemyLandType enemyLandType) {
			var enemyControllerAbstract = _enemyStorageSO.GetLandEnemyByType(enemyLandType).EnemyController;

			var enemy = Instantiate(enemyControllerAbstract, _containerLand);
			enemy.ResetEnemy();

			_enemysLand.Add(enemy);
		}

		private void SpawnSeaEnemy(Vector3 _startPosition, Vector3 _direction, EnemySeaType enemyType) {
			if (_enemysSea.Find(enem => enem.IsFree && enem.EnemyType == enemyType) == null) {
				AddSeaEnemy(enemyType);
			}

			_enemysSea.Find(someEnemy => someEnemy.IsFree).ShowEnemy(_startPosition, _direction, enemyType);
		}

		private void SpawnLandEnemy(Vector3 _startPosition, Vector3 _direction, EnemyLandType enemyType) {
			if (_enemysLand.Find(enem => enem.IsFree && enem.EnemyType == enemyType) == null) {
				AddLandEnemy(enemyType);
			}

			_enemysLand.Find(someEnemy => someEnemy.IsFree).ShowEnemy(_startPosition, _direction, enemyType);
		}

		private void PrepareCamera() {
			var canvas = gameObject.GetComponent<Canvas>();

			if (canvas != null 
				&& canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace 
				&& canvas.worldCamera == null) {
				canvas.worldCamera = Camera.main;
			}
		}

		public void OnValidate() {
			if(IsValidate) {
				PrepareCamera();
			}
		}
	}
}
