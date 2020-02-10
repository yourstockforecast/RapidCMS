using System;
using System.Threading.Tasks;
using RapidCMS.Core.Abstractions.Data;
using RapidCMS.Core.Abstractions.Forms;

namespace RapidCMS.Core.Abstractions.Config
{
    public interface IIsDisableable<TReturn, TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// Sets an expression which determine whether this should be disabled.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TReturn DisableWhen(Func<IEditContext<TEntity>, bool> predicate);

        /// <summary>
        /// Sets an expression which determine whether this should be disabled.
        /// </summary>
        /// <param name="predicate"></param>
        TReturn DisableWhen(Func<IEditContext<TEntity>, Task<bool>> predicate);
    }
}
