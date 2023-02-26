using System;
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
using Random = UnityEngine.Random;

namespace Assets.Scripts.Enemies {
	public class EnemySpowner : MonoBehaviour, IValidateHalper {
		[Header("Base")]
		[SerializeField] private DependencyInjections _dependencyInjections = default;
		[SerializeField] private EnemyStorage _enemyStorageSO = default;
		[SerializeField] private LevelStorage _levelStorageSO = default;
		[SerializeField] private PlayerStorage _playerStorageSO = default;

		[Header("Settings")]
		[SerializeField] private float _durationSpawnEnemy = 0.5f;

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

		private Level _currentLevel = default;
		private Coroutine _tempSpawnEnemyCoroutine = null;
		private GridManager _tempGridManager = null;

		private List<EnemySeaType> _tempListSeatype = default;
		private List<EnemyLandType> _tempListLandtype = default;

		private Vector3 _tempPlayerPosition = Vector3.zero;

		#endregion

		private void Awake() {
			_spownEnemiesCoroutine = StartCoroutine(PrepareEnemysAsync());
			//PrepareEnemys();
			PrepareCamera();
		}
		private void Start() {
			_tempGridManager = _dependencyInjections.GridManager;
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
			ControllVisabilityEnemy();

			HideAllEnemy(() => {
				StartSpownEnemies();
			});
		}
		private void ReactionFinishLevel(LevelResult levelResult) {
			if (levelResult == LevelResult.Win) {

			}
		}

		private void ControllVisabilityEnemy() {
			if (!_enemiesContainer.gameObject.activeSelf) {
				_enemiesContainer.gameObject.SetActive(true);
			}
		}
		#endregion

		#region Enemy spawn controll


		private void StartSpownEnemies() {
			_currentLevel = _levelStorageSO.GetNextLevel(_playerStorageSO.ConcretePlayer.PlayerLevel);

			if (_tempSpawnEnemyCoroutine == null) {
				_tempSpawnEnemyCoroutine = StartCoroutine(SpownEnemies());
			}
		}
		private Vector3 GetRandomSeaPoints() {
			List<Vector3> list = _tempGridManager.GetListAllSeaPosition();
			if (list != null && list.Count > 0) {

				return list[Random.Range(0, list.Count)];
			}

			else {
				Debug.LogError("List Sea point is empty");
			}

			return Vector3.zero;
		}

		private Vector3 GetRandomLandPoints() {
			List<Vector3> list = _tempGridManager.GetListAllLandPosition();
			if (list != null && list.Count > 0) {

				return list[Random.Range(0, list.Count)];
			}

			else {
				Debug.LogError("List Land point is empty");
			}

			return Vector3.zero;
		}

		private IEnumerator SpownEnemies() {
			_tempListSeatype = new List<EnemySeaType>();
			_tempListLandtype = new List<EnemyLandType>();

			_tempListSeatype = _currentLevel.EnemiesSea;
			_tempListLandtype = _currentLevel.EnemiesLand;

			_tempPlayerPosition = _dependencyInjections.PlayerPosition.position;
			if (_tempListSeatype.Count > 0) {
				foreach (var sea in _tempListSeatype) {
					SpawnSeaEnemy(GetRandomSeaPoints(), _tempPlayerPosition, sea);
					yield return new WaitForSeconds(_durationSpawnEnemy);
				}
			}

			if (_tempListLandtype.Count > 0) {
				foreach (var land in _tempListLandtype) {
					SpawnLandEnemy(GetRandomLandPoints(), _tempPlayerPosition, land);
					yield return new WaitForSeconds(_durationSpawnEnemy);
				}
			}

			_tempSpawnEnemyCoroutine = null!;
		}
		#endregion

		private IEnumerator PrepareEnemysAsync() {
			for (int i = 0; i < _startingEnemiesCountPool; i++) {

				for (int sea = 0; sea < typeof(EnemySeaType).GetEnumValues().Length; sea++) {
					yield return AddSeaEnemyAsync((EnemySeaType)sea);
				}

				for (int land = 0; land < typeof(EnemyLandType).GetEnumValues().Length; land++) {
					yield return AddLandEnemyAsync((EnemyLandType)land);
				}
			}

			_spownEnemiesCoroutine = null!;
		}

		private void PrepareEnemys() {
			for (int i = 0; i < _startingEnemiesCountPool; i++) {

				for (int sea = 0; sea < typeof(EnemySeaType).GetEnumValues().Length; sea++) {
					AddSeaEnemy((EnemySeaType)sea);
				}

				for (int land = 0; land < typeof(EnemyLandType).GetEnumValues().Length; land++) {
					AddLandEnemy((EnemyLandType)land);
				}
			}
		}

		private void AddSeaEnemy(EnemySeaType seaEnemyType, Action callBack = null!) {
			var enemyControllerAbstract = _enemyStorageSO.GetSeaEnemyByType(seaEnemyType).EnemyController;

			var enemy = Instantiate(enemyControllerAbstract, _containerSea);
			enemy.ResetEnemy();

			_enemysSea.Add(enemy);

			callBack?.Invoke();
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
					catch (Exception ex) {
						Debug.LogError($"Sea enemy Game Object not Instantiate Error = {ex}");
					}

					EnemySea unit = null!;
					try {
						unit = gameObject.GetComponent<EnemySea>();
					}
					catch (Exception ex) {
						Debug.LogError($"Sea enemy can`t take component {nameof(EnemySea)} Error = {ex}");
					}
					finally {
						if (unit != null) {
							unit.ResetEnemy();
							_enemysSea.Add(unit);
						}

						else {
							Debug.LogError($"Sea unit == null! \t\t {gridUnitSea?.Asset?.name}");
						}
					}
				}

				else {
					Debug.LogError($"Sea unit is NOT DONE \t\ttype= {seaEnemyType}");
				}
			}

			else {
				Debug.LogError($"Can`t InstantiateAsync Land");
			}
		}

		private void AddLandEnemy(EnemyLandType enemyLandType, Action callBack = null!) {
			var enemyControllerAbstract = _enemyStorageSO.GetLandEnemyByType(enemyLandType).EnemyController;

			var enemy = Instantiate(enemyControllerAbstract, _containerLand);
			enemy.ResetEnemy();

			_enemysLand.Add(enemy);

			callBack?.Invoke();
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
					catch (Exception ex) {
						Debug.LogError($"Land enemy Game Object not Instantiate Error = {ex}");
					}

					EnemyLand unit = null!;
					try {
						unit = gameObject.GetComponent<EnemyLand>();
					}
					catch (Exception ex) {
						Debug.LogError($"Land enemy can`t take component {nameof(EnemyLand)} Error = {ex}");
					}
					finally {
						if (unit != null) {
							unit.ResetEnemy();
							_enemysLand.Add(unit);
						}

						else {
							Debug.LogError($"Land unit == null! \t\ttype= {enemyLandType}");
						}
					}
				}

				else {
					Debug.LogError($"Land unit is NOT DONE \t\ttype= {enemyLandType}");
				}
			}

			else {
				Debug.LogError($"Can`t InstantiateAsync Land");
			}
		}

		private void SpawnSeaEnemy(Vector3 _startPosition, Vector3 _direction, EnemySeaType enemyType) {
			var tempSea = _enemysSea.Find(enem => enem.IsFree && enem.EnemyType == enemyType);

			if (tempSea == null) {
				AddSeaEnemy(enemyType, () => {
					SpawnSeaEnemy(_startPosition, _direction, enemyType);
				});
			}

			else {
				tempSea.ShowEnemy(_startPosition, _direction, enemyType);
			}
		}

		private void SpawnLandEnemy(Vector3 _startPosition, Vector3 _direction, EnemyLandType enemyType) {
			var tempLand = _enemysLand.Find(enem => enem.IsFree && enem.EnemyType == enemyType);

			if (tempLand == null) {
				AddLandEnemy(enemyType, () => {
					SpawnLandEnemy(_startPosition, _direction, enemyType);
				});
			}

			else {
				tempLand.ShowEnemy(_startPosition, _direction, enemyType);
			}
		}

		private void HideAllEnemy(Action callBack) {
			_enemysSea.FindAll(enemy => !enemy.IsFree).ForEach(enemy => enemy.HideEnemy());
			_enemysLand.FindAll(enemy => !enemy.IsFree).ForEach(enemy => enemy.HideEnemy());

			callBack?.Invoke();
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

			else if (_tempSpawnEnemyCoroutine != null) {
				StopCoroutine(_tempSpawnEnemyCoroutine);
			}
		}

		public void OnValidate() {
			if (IsValidate) {
				PrepareCamera();
			}
		}
	}
}
