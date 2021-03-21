using DotVVM.Framework.ViewModel;

namespace Scheduler.App.Pages
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