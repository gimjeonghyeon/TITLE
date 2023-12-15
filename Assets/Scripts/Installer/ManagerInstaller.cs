using Playground.Firebase;
using UnityEngine;
using Zenject;

namespace Playground
{
    public class ManagerInstaller : MonoInstaller
    {
        #region Private
        
        [SerializeField] private ApplicationManager _applicationManager;

        #endregion
        
        public override void InstallBindings()
        {
            Container.Bind<ApplicationManager>().FromComponentInNewPrefab(_applicationManager).AsSingle().NonLazy();
            Container.Bind<FirebaseManager>().AsSingle();
            Container.Bind<UserInfoManager>().AsSingle();
        }
    }
}