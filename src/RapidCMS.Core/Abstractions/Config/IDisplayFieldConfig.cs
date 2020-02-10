using System;
using RapidCMS.Core.Enums;
using RapidCMS.Core.Abstractions.Data;

namespace RapidCMS.Core.Abstractions.Config
{
    public interface IDisplayFieldConfig<TEntity, TValue> : 
            IHasOrderBy<TEntity, IDisplayFieldConfig<TEntity, TValue>>,
            IIsHideable<IDisplayFieldConfig<TEntity, TValue>, TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// Sets the name of this field, used in table and list views.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IDisplayFieldConfig<TEntity, TValue> SetName(string name);

        /// <summary>
        /// Sets the description of this field, displayed under the name.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        IDisplayFieldConfig<TEntity, TValue> SetDescription(string description);

        /// <summary>
        /// Sets the type of build-in display used for this field.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IDisplayFieldConfig<TEntity, TValue> SetType(DisplayType type);

        /// <summary>
        /// Sets the type of custom razor component used for this field.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IDisplayFieldConfig<TEntity, TValue> SetType(Type type);
    }
}
