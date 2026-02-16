using UnityEngine;
using Zenject;

public class StateManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameStateManager>().AsSingle().NonLazy();
        Container.Bind<DimensionStateHolder>().AsSingle().NonLazy();
    }
}