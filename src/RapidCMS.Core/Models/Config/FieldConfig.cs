using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RapidCMS.Core.Abstractions.Config;
using RapidCMS.Core.Abstractions.Data;
using RapidCMS.Core.Abstractions.Forms;
using RapidCMS.Core.Abstractions.Metadata;
using RapidCMS.Core.Attributes;
using RapidCMS.Core.Enums;
using RapidCMS.Core.Exceptions;
using RapidCMS.Core.Extensions;
using RapidCMS.Core.Helpers;

namespace RapidCMS.Core.Models.Config
{
    internal class FieldConfig
    {
        internal int Index { get; set; }

        internal string? Name { get; set; }
        internal string? Description { get; set; }

        internal Func<object, Task<bool>> IsVisible { get; set; } = x => Task.FromResult(true);
        internal Func<object, Task<bool>> IsDisabled { get; set; } = x => Task.FromResult(false);

        internal IExpressionMetadata? Expression { get; set; }
        internal IPropertyMetadata? Property { get; set; }

        internal RelationConfig? Relation { get; set; }

        internal EditorType EditorType { get; set; }
        internal DisplayType DisplayType { get; set; }
        internal Type? CustomType { get; set; }

        internal IPropertyMetadata? OrderByExpression { get; set; }
        internal OrderByType DefaultOrder { get; set; }
    }

    internal class FieldConfig<TEntity, TValue> : FieldConfig, IDisplayFieldConfig<TEntity, TValue>, IEditorFieldConfig<TEntity, TValue>
        where TEntity : IEntity
    {
        IDisplayFieldConfig<TEntity, TValue> IDisplayFieldConfig<TEntity, TValue>.SetName(string name)
        {
            Name = name;
            return this;
        }

        IDisplayFieldConfig<TEntity, TValue> IDisplayFieldConfig<TEntity, TValue>.SetDescription(string description)
        {
            Description = description;
            return this;
        }

        IDisplayFieldConfig<TEntity, TValue> IDisplayFieldConfig<TEntity, TValue>.SetType(DisplayType type)
        {
            DisplayType = type;
            EditorType = EditorType.None;
            return this;
        }

        IDisplayFieldConfig<TEntity, TValue> IDisplayFieldConfig<TEntity, TValue>.SetType(Type type)
        {
            DisplayType = DisplayType.Custom;
            CustomType = type;
            return this;
        }

        IDisplayFieldConfig<TEntity, TValue> IIsHideable<IDisplayFieldConfig<TEntity, TValue>, TEntity>.VisibleWhen(Func<IEditContext<TEntity>, bool> predicate)
        {
            IsVisible = context => Task.FromResult(predicate.Invoke((IEditContext<TEntity>)context));
            return this;
        }

        IDisplayFieldConfig<TEntity, TValue> IIsHideable<IDisplayFieldConfig<TEntity, TValue>, TEntity>.VisibleWhen(Func<IEditContext<TEntity>, Task<bool>> predicate)
        {
            IsVisible = context => predicate.Invoke((IEditContext<TEntity>)context);
            return this;
        }

        IDisplayFieldConfig<TEntity, TValue> IHasOrderBy<TEntity, IDisplayFieldConfig<TEntity, TValue>>.SetOrderByExpression<TOrderByValue>(Expression<Func<TEntity, TOrderByValue>> orderByExpression, OrderByType defaultOrder)
        {
            OrderByExpression = PropertyMetadataHelper.GetPropertyMetadata(orderByExpression);
            DefaultOrder = defaultOrder;
            return this;
        }

        IDisplayFieldConfig<TEntity, TValue> IHasOrderBy<TEntity, IDisplayFieldConfig<TEntity, TValue>>.SetOrderByExpression<TDatabaseEntity, TOrderByValue>(Expression<Func<TDatabaseEntity, TOrderByValue>> orderByExpression, OrderByType defaultOrder)
        {
            OrderByExpression = PropertyMetadataHelper.GetPropertyMetadata(orderByExpression);
            DefaultOrder = defaultOrder;
            return this;
        }

        IEditorFieldConfig<TEntity, TValue> IEditorFieldConfig<TEntity, TValue>.SetName(string name)
        {
            Name = name;
            return this;
        }

        IEditorFieldConfig<TEntity, TValue> IEditorFieldConfig<TEntity, TValue>.SetDescription(string description)
        {
            Description = description;
            return this;
        }

        IEditorFieldConfig<TEntity, TValue> IEditorFieldConfig<TEntity, TValue>.SetType(EditorType type)
        {
            EditorType = type;
            DisplayType = DisplayType.None;
            return this;
        }

        IEditorFieldConfig<TEntity, TValue> IEditorFieldConfig<TEntity, TValue>.SetType(DisplayType type)
        {
            EditorType = EditorType.None;
            DisplayType = type;
            return this;
        }

        IEditorFieldConfig<TEntity, TValue> IEditorFieldConfig<TEntity, TValue>.SetType(Type type)
        {
            EditorType = EditorType.Custom;
            CustomType = type;
            return this;
        }

        IEditorFieldConfig<TEntity, TValue> IEditorFieldConfig<TEntity, TValue>.SetDataCollection<TDataCollection>()
        {
            if (EditorType != EditorType.Custom && EditorType.GetCustomAttribute<RelationAttribute>()?.Type != RelationType.One)
            {
                throw new InvalidOperationException("Cannot add DataRelation to Editor with no support for RelationType.One");
            }

            var config = new DataProviderRelationConfig(typeof(TDataCollection));

            Relation = config;

            return this;
        }

        IEditorFieldConfig<TEntity, TValue> IEditorFieldConfig<TEntity, TValue>.SetCollectionRelation<TRelatedEntity>(
            string collectionAlias, Action<ICollectionRelationConfig<TEntity, TRelatedEntity>> configure)
        {
            if (EditorType != EditorType.Custom && !(EditorType.GetCustomAttribute<RelationAttribute>()?.Type.In(RelationType.One, RelationType.Many) ?? false))
            {
                throw new InvalidOperationException("Cannot add CollectionRelation to Editor with no support for RelationType.One / RelationType.Many");
            }

            var config = new CollectionRelationConfig<TEntity, TRelatedEntity>(collectionAlias);

            configure.Invoke(config);

            Relation = config;

            return this;
        }

        IEditorFieldConfig<TEntity, TValue> IEditorFieldConfig<TEntity, TValue>.SetCollectionRelation<TRelatedEntity, TRelatedRepository>(
            Action<ICollectionRelationConfig<TEntity, TRelatedEntity>> configure)
        {
            if (EditorType != EditorType.Custom && !(EditorType.GetCustomAttribute<RelationAttribute>()?.Type.In(RelationType.One, RelationType.Many) ?? false))
            {
                throw new InvalidOperationException("Cannot add CollectionRelation to Editor with no support for RelationType.One / RelationType.Many");
            }

            var config = new CollectionRelationConfig<TEntity, TRelatedEntity>(typeof(TRelatedRepository));

            configure.Invoke(config);

            Relation = config;

            return this;
        }

        IEditorFieldConfig<TEntity, TValue> IEditorFieldConfig<TEntity, TValue>.SetCollectionRelation<TRelatedEntity, TKey>(
            Expression<Func<TValue, IEnumerable<TKey>>> relatedElements, string collectionAlias, Action<ICollectionRelationConfig<TEntity, TRelatedEntity>> configure)
        {
            if (EditorType != EditorType.Custom && !(EditorType.GetCustomAttribute<RelationAttribute>()?.Type.In(RelationType.Many) ?? false))
            {
                throw new InvalidOperationException("Cannot add CollectionRelation with relatedElements to Editor with no support for RelationType.Many");
            }

            var relatedElementsGetter = PropertyMetadataHelper.GetPropertyMetadata(relatedElements);
            var config = new CollectionRelationConfig<TEntity, TRelatedEntity>(collectionAlias, relatedElementsGetter ?? throw new InvalidExpressionException(nameof(relatedElements)));

            configure.Invoke(config);

            Relation = config;

            return this;
        }

        IEditorFieldConfig<TEntity, TValue> IEditorFieldConfig<TEntity, TValue>.SetCollectionRelation<TRelatedEntity, TKey, TRelatedRepository>(
            Expression<Func<TValue, IEnumerable<TKey>>> relatedElements, Action<ICollectionRelationConfig<TEntity, TRelatedEntity>> configure)
        {
            if (EditorType != EditorType.Custom && !(EditorType.GetCustomAttribute<RelationAttribute>()?.Type.In(RelationType.Many) ?? false))
            {
                throw new InvalidOperationException("Cannot add CollectionRelation with relatedElements to Editor with no support for RelationType.Many");
            }

            var relatedElementsGetter = PropertyMetadataHelper.GetPropertyMetadata(relatedElements);
            var config = new CollectionRelationConfig<TEntity, TRelatedEntity>(typeof(TRelatedRepository), relatedElementsGetter ?? throw new InvalidExpressionException(nameof(relatedElements)));

            configure.Invoke(config);

            Relation = config;

            return this;
        }

        IEditorFieldConfig<TEntity, TValue> IIsDisableable<IEditorFieldConfig<TEntity, TValue>, TEntity>.DisableWhen(Func<IEditContext<TEntity>, bool> predicate)
        {
            IsDisabled = context => Task.FromResult(predicate.Invoke((IEditContext<TEntity>)context));
            return this;
        }

        IEditorFieldConfig<TEntity, TValue> IIsDisableable<IEditorFieldConfig<TEntity, TValue>, TEntity>.DisableWhen(Func<IEditContext<TEntity>, Task<bool>> predicate)
        {
            IsDisabled = context => predicate.Invoke((IEditContext<TEntity>)context);
            return this;
        }

        IEditorFieldConfig<TEntity, TValue> IIsHideable<IEditorFieldConfig<TEntity, TValue>, TEntity>.VisibleWhen(Func<IEditContext<TEntity>, bool> predicate)
        {
            IsVisible = context => Task.FromResult(predicate.Invoke((IEditContext<TEntity>)context));
            return this;
        }

        IEditorFieldConfig<TEntity, TValue> IIsHideable<IEditorFieldConfig<TEntity, TValue>, TEntity>.VisibleWhen(Func<IEditContext<TEntity>, Task<bool>> predicate)
        {
            IsVisible = context => predicate.Invoke((IEditContext<TEntity>)context);
            return this;
        }

        IEditorFieldConfig<TEntity, TValue> IHasOrderBy<TEntity, IEditorFieldConfig<TEntity, TValue>>.SetOrderByExpression<TOrderByValue>(Expression<Func<TEntity, TOrderByValue>> orderByExpression, OrderByType defaultOrder)
        {
            OrderByExpression = PropertyMetadataHelper.GetPropertyMetadata(orderByExpression);
            DefaultOrder = defaultOrder;
            return this;
        }

        IEditorFieldConfig<TEntity, TValue> IHasOrderBy<TEntity, IEditorFieldConfig<TEntity, TValue>>.SetOrderByExpression<TDatabaseEntity, TOrderByValue>(Expression<Func<TDatabaseEntity, TOrderByValue>> orderByExpression, OrderByType defaultOrder)
        {
            OrderByExpression = PropertyMetadataHelper.GetPropertyMetadata(orderByExpression);
            DefaultOrder = defaultOrder;
            return this;
        }

        
    }
}
