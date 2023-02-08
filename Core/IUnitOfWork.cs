

namespace MapperApp.Core
{
    public interface IUnitOfWork
    {
        IDriverRepository Drivers { get; }
        void CompleteChange();
    }
}