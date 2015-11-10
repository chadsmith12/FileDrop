using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace FileDrop.EntityFramework.Repositories
{
    public abstract class FileDropRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<FileDropDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected FileDropRepositoryBase(IDbContextProvider<FileDropDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class FileDropRepositoryBase<TEntity> : FileDropRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected FileDropRepositoryBase(IDbContextProvider<FileDropDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
