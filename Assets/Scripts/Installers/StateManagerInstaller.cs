using UnityEngine;
using Zenject;

public class StateManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameStateManager>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<DimensionStateHolder>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<LavaFreezer>().AsSingle().NonLazy();
    }
}