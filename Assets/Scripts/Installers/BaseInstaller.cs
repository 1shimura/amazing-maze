using Client;
using Client.Managers;
using Zenject;

namespace Installers
{
    public class BaseInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IGameSettingsManager>().To<GameSettingsManager>().AsSingle();
            Container.Bind<ILoadingManager>().To<LoadingManager>().AsSingle();
        }
    }
}