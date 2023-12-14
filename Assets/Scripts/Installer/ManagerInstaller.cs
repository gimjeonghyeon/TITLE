using Zenject;

namespace Playground
{
    public class ManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<UserInfoManager>().AsSingle();
        }
    }
}