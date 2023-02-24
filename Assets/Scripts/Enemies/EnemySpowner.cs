using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Helpers;
using Assets.Scripts.Enemies.Current;
using Assets.Scripts.Enemies.Storage;
using Assets.Scripts.Player;
using Assets.Scripts.World;
using Assets.Scripts.World.Grid;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

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
		private List<EnemyControllerAbstract<EnemySeaType>> _enemysSea =
			new List<EnemyControllerAbstract<EnemySeaType>>();
		private List<EnemyControllerAbstract<EnemyLandType>> _enemysLand =
			new List<EnemyControllerAbstract<EnemyLandType>>();
		#endregion

		#region Get\Set
		public bool LevelStarted { get; set; } = false;
		#endregion

		private void Awake() {
			_spownEnemiesCoroutine = StartCoroutine(PrepareEnemys());
			PrepareCamera();
		}

		private void OnEnable() {
			GameManager.LevelStartAction += ReactionStartLevel;
			GameManager.LevelFinishAction += ReactionFinishLevel;
		}

		private void OnDisable() {
			GameManager.LevelStartAction -= ReactionStartLevel;
			GameManager.LevelFinishAction -= ReactionFinishLevel;

			ResetCoroutine();
		}

		private void OnDestroy() {
			ResetCoroutine();
		}

		#region Reaction
		private void ReactionStartLevel() {
			LevelStarted = true;
		}
		private void ReactionFinishLevel(LevelResult levelResult) {
			if (levelResult == LevelResult.Win) {
				LevelStarted = true;
			}
		}
		#endregion

		private IEnumerator PrepareEnemys() {
			for (int i = 0; i < _startingEnemiesCountPool; i++) {

				for (int sea = 0; sea < typeof(EnemySeaType).GetEnumValues().Length; sea++) {
					if ((EnemySeaType)sea != EnemySeaType.None) {
						yield return AddSeaEnemyAsync((EnemySeaType)sea);
					}
				}

				for (int land = 0; land < typeof(EnemyLandType).GetEnumValues().Length; land++) {
					if ((EnemyLandType)land != EnemyLandType.None) {
						yield return AddLandEnemyAsync((EnemyLandType)land);
					}
				}
			}

			_spownEnemiesCoroutine = null;
		}
		private void AddSeaEnemy(EnemySeaType seaEnemyType) {
			var enemyControllerAbstract = _enemyStorageSO.GetSeaEnemyByType(seaEnemyType).EnemyController;

			var enemy = Instantiate(enemyControllerAbstract, _containerSea);
			enemy.ResetEnemy();

			_enemysSea.Add(enemy);
		}

		private IEnumerator AddSeaEnemyAsync(EnemySeaType seaEnemyType) {
			var gridUnitSea = _enemyStorageSO.GetAssetReferenceSeaUnitByType(seaEnemyType);
			var unitTask = gridUnitSea.InstantiateAsync(_containerSea);

			yield return unitTask;

			if (unitTask.Status == AsyncOperationStatus.Succeeded) {
				if (unitTask.IsDone) {
					GameObject gameObject = null!;
					try {
						gameObject = unitTask.Result;
					}
					catch (System.Exception ex) {
						Debug.LogError($"Sea enemy Game Object not Instantiate Error = {ex}");
					}

					EnemySea unit = null!;
					try {
						unit = gameObject.GetComponent<EnemySea>();
					}
					catch (System.Exception ex) {
						Debug.LogError($"Sea enemy can`t take component {nameof(EnemySea)} Error = {ex}");
					}

					if(unit != null) {
						unit.ResetEnemy();
						_enemysSea.Add(unit);
					}

					else {
							Debug.LogError($"Sea unit == null! \t\t {gridUnitSea?.Asset?.name}");
					}
				}
			}

			else {
				Debug.LogError($"Can`t InstantiateAsync Land");
			}
		}

		private void AddLandEnemy(EnemyLandType enemyLandType) {
			var enemyControllerAbstract = _enemyStorageSO.GetLandEnemyByType(enemyLandType).EnemyController;

			var enemy = Instantiate(enemyControllerAbstract, _containerLand);
			enemy.ResetEnemy();

			_enemysLand.Add(enemy);
		}

		private IEnumerator AddLandEnemyAsync(EnemyLandType enemyLandType) {
			var gridUnitLand = _enemyStorageSO.GetAssetReferenceLandUnitByType(enemyLandType);
			var unitTask = gridUnitLand.InstantiateAsync(_containerLand);

			yield return unitTask;

			if (unitTask.Status == AsyncOperationStatus.Succeeded) {
				if (unitTask.IsDone) {
					GameObject gameObject = null!;
					try {
						gameObject = unitTask.Result;
					}
					catch (System.Exception ex) {
						Debug.LogError($"Land enemy Game Object not Instantiate Error = {ex}");
					}

					EnemyLand unit = null!;
					try {
						unit = gameObject.GetComponent<EnemyLand>();
					}
					catch (System.Exception ex) {
						Debug.LogError($"Land enemy can`t take component {nameof(EnemyLand)} Error = {ex}");
					}

					if(unit != null) {
						unit.ResetEnemy();
						_enemysLand.Add(unit);
					}

					else {
						Debug.LogError($"Land unit == null! \t\ttype= {enemyLandType}");
					}
				}
			}

			else {
				Debug.LogError($"Can`t InstantiateAsync Land");
			}
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

		private void ResetCoroutine() {
			if (_spownEnemiesCoroutine != null) {
				StopCoroutine(_spownEnemiesCoroutine);
			}
		}

		public void OnValidate() {
			if (IsValidate) {
				PrepareCamera();
			}
		}
	}
}
