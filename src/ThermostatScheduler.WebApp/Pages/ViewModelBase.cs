using DotVVM.Framework.ViewModel;

namespace ThermostatScheduler.WebApp.Pages
{
    public abstract class ViewModelBase : DotvvmViewModelBase
    {
        protected ViewModelBase(IDependencies dependencies)
        {
        }

        public interface IDependencies
        {
        }

        public class Dependencies : IDependencies
        {
        }
    }
}