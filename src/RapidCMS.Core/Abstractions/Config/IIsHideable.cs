using System;
using System.Threading.Tasks;
using RapidCMS.Core.Abstractions.Data;
using RapidCMS.Core.Abstractions.Forms;

namespace RapidCMS.Core.Abstractions.Config
{
    public interface IIsHideable<TReturn, TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// Sets an expression which determines whether this should be visible.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TReturn VisibleWhen(Func<IEditContext<TEntity>, bool> predicate);

        /// <summary>
        /// Sets an expression which determines whether this should be visible.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TReturn VisibleWhen(Func<IEditContext<TEntity>, Task<bool>> predicate);
    }
}
