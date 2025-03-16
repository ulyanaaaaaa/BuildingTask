using Zenject;
using UnityEngine;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameObject buttonsParent;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameConfig gameConfig;

    public override void InstallBindings()
    {
        var playerControls = new PlayerControls();
        Container.Bind<PlayerControls>().FromInstance(playerControls).AsSingle();
    
        Container.Bind<GameConfig>().FromInstance(gameConfig).AsSingle();
        
        Container.Bind<BuildingData[]>().FromInstance(gameConfig.BuildingData).AsSingle();
    
        Container.Bind<BuildingPool>().FromComponentInNewPrefabResource("BuildingPool").AsSingle().NonLazy();
        Container.Bind<GridManager>().FromComponentInNewPrefabResource("GridManager").AsSingle().NonLazy();
        Container.Bind<ButtonsService>().FromComponentInNewPrefabResource("ButtonsService").AsSingle().NonLazy();
        Container.Bind<BuildingsGrid>().FromComponentInNewPrefabResource("BuildingGrid").AsSingle().NonLazy();
        Container.Bind<PlaceButton>().FromComponentInNewPrefabResource("PlaceButton").AsTransient();
        Container.Bind<DeleteButton>().FromComponentInNewPrefabResource("DeleteButton").AsTransient();
        Container.Bind<Bootstrap>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<GameObject>().WithId("ButtonsParent").FromInstance(buttonsParent).AsCached();
        Container.Bind<Canvas>().FromInstance(canvas).AsSingle();
    }
}