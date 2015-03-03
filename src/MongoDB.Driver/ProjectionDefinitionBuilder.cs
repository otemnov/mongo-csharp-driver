﻿/* Copyright 2010-2014 MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver.Core.Misc;

namespace MongoDB.Driver
{
    /// <summary>
    /// Extension methods for projections.
    /// </summary>
    public static class ProjectionExtensions
    {
        private static class BuilderCache<TDocument>
        {
            public static ProjectionBuilder<TDocument> Instance = new ProjectionBuilder<TDocument>();
        }

        /// <summary>
        /// Combines an existing projection with a positional operator projection.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="projection">The projection.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>
        /// A combined projection.
        /// </returns>
        public static ProjectionDefinition<TDocument> ElemMatch<TDocument>(this ProjectionDefinition<TDocument> projection, FieldName<TDocument> fieldName)
        {
            var builder = BuilderCache<TDocument>.Instance;
            return builder.Combine(projection, builder.ElemMatch(fieldName));
        }

        /// <summary>
        /// Combines an existing projection with a positional operator projection.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="projection">The projection.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>
        /// A combined projection.
        /// </returns>
        public static ProjectionDefinition<TDocument> ElemMatch<TDocument>(this ProjectionDefinition<TDocument> projection, Expression<Func<TDocument, object>> fieldName)
        {
            var builder = BuilderCache<TDocument>.Instance;
            return builder.Combine(projection, builder.ElemMatch(fieldName));
        }

        /// <summary>
        /// Combines an existing projection with a projection that filters the contents of an array.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="projection">The projection.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// A combined projection.
        /// </returns>
        public static ProjectionDefinition<TDocument> ElemMatch<TDocument, TField, TItem>(this ProjectionDefinition<TDocument> projection, FieldName<TDocument, TField> fieldName, FilterDefinition<TItem> filter)
            where TField : IEnumerable<TItem>
        {
            var builder = BuilderCache<TDocument>.Instance;
            return builder.Combine(projection, builder.ElemMatch(fieldName, filter));
        }

        /// <summary>
        /// Combines an existing projection with a projection that filters the contents of an array.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="projection">The projection.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// A combined projection.
        /// </returns>
        public static ProjectionDefinition<TDocument> ElemMatch<TDocument, TItem>(this ProjectionDefinition<TDocument> projection, string fieldName, FilterDefinition<TItem> filter)
        {
            var builder = BuilderCache<TDocument>.Instance;
            return builder.Combine(projection, builder.ElemMatch(fieldName, filter));
        }

        /// <summary>
        /// Combines an existing projection with a projection that filters the contents of an array.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="projection">The projection.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// A combined projection.
        /// </returns>
        public static ProjectionDefinition<TDocument> ElemMatch<TDocument, TField, TItem>(this ProjectionDefinition<TDocument> projection, Expression<Func<TDocument, TField>> fieldName, FilterDefinition<TItem> filter)
            where TField : IEnumerable<TItem>
        {
            var builder = BuilderCache<TDocument>.Instance;
            return builder.Combine(projection, builder.ElemMatch(fieldName, filter));
        }

        /// <summary>
        /// Combines an existing projection with a projection that filters the contents of an array.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="projection">The projection.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// A combined projection.
        /// </returns>
        public static ProjectionDefinition<TDocument> ElemMatch<TDocument, TField, TItem>(this ProjectionDefinition<TDocument> projection, Expression<Func<TDocument, TField>> fieldName, Expression<Func<TItem, bool>> filter)
            where TField : IEnumerable<TItem>
        {
            var builder = BuilderCache<TDocument>.Instance;
            return builder.Combine(projection, builder.ElemMatch(fieldName, filter));
        }

        /// <summary>
        /// Combines an existing projection with a projection that excludes a field.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="projection">The projection.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>
        /// A combined projection.
        /// </returns>
        public static ProjectionDefinition<TDocument> Exclude<TDocument>(this ProjectionDefinition<TDocument> projection, FieldName<TDocument> fieldName)
        {
            var builder = BuilderCache<TDocument>.Instance;
            return builder.Combine(projection, builder.Exclude(fieldName));
        }

        /// <summary>
        /// Combines an existing projection with a projection that excludes a field.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="projection">The projection.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>
        /// A combined projection.
        /// </returns>
        public static ProjectionDefinition<TDocument> Exclude<TDocument>(this ProjectionDefinition<TDocument> projection, Expression<Func<TDocument, object>> fieldName)
        {
            var builder = BuilderCache<TDocument>.Instance;
            return builder.Combine(projection, builder.Exclude(fieldName));
        }

        /// <summary>
        /// Combines an existing projection with a projection that includes a field.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="projection">The projection.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>
        /// A combined projection.
        /// </returns>
        public static ProjectionDefinition<TDocument> Include<TDocument>(this ProjectionDefinition<TDocument> projection, FieldName<TDocument> fieldName)
        {
            var builder = BuilderCache<TDocument>.Instance;
            return builder.Combine(projection, builder.Include(fieldName));
        }

        /// <summary>
        /// Combines an existing projection with a projection that includes a field.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="projection">The projection.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>
        /// A combined projection.
        /// </returns>
        public static ProjectionDefinition<TDocument> Include<TDocument>(this ProjectionDefinition<TDocument> projection, Expression<Func<TDocument, object>> fieldName)
        {
            var builder = BuilderCache<TDocument>.Instance;
            return builder.Combine(projection, builder.Include(fieldName));
        }

        /// <summary>
        /// Combines an existing projection with a text score projection.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="projection">The projection.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>
        /// A combined projection.
        /// </returns>
        public static ProjectionDefinition<TDocument> MetaTextScore<TDocument>(this ProjectionDefinition<TDocument> projection, string fieldName)
        {
            var builder = BuilderCache<TDocument>.Instance;
            return builder.Combine(projection, builder.MetaTextScore(fieldName));
        }

        /// <summary>
        /// Combines an existing projection with an array slice projection.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="projection">The projection.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="limit">The limit.</param>
        /// <returns>
        /// A combined projection.
        /// </returns>
        public static ProjectionDefinition<TDocument> Slice<TDocument>(this ProjectionDefinition<TDocument> projection, FieldName<TDocument> fieldName, int skip, int? limit = null)
        {
            var builder = BuilderCache<TDocument>.Instance;
            return builder.Combine(projection, builder.Slice(fieldName, skip, limit));
        }

        /// <summary>
        /// Combines an existing projection with an array slice projection.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="projection">The projection.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="limit">The limit.</param>
        /// <returns>
        /// A combined projection.
        /// </returns>
        public static ProjectionDefinition<TDocument> Slice<TDocument>(this ProjectionDefinition<TDocument> projection, Expression<Func<TDocument, object>> fieldName, int skip, int? limit = null)
        {
            var builder = BuilderCache<TDocument>.Instance;
            return builder.Combine(projection, builder.Slice(fieldName, skip, limit));
        }
    }

    /// <summary>
    /// A builder for a projection.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    public sealed class ProjectionBuilder<TSource>
    {
        /// <summary>
        /// Combines the specified projections.
        /// </summary>
        /// <param name="projections">The projections.</param>
        /// <returns>
        /// A combined projection.
        /// </returns>
        public ProjectionDefinition<TSource> Combine(params ProjectionDefinition<TSource>[] projections)
        {
            return Combine((IEnumerable<ProjectionDefinition<TSource>>)projections);
        }

        /// <summary>
        /// Combines the specified projections.
        /// </summary>
        /// <param name="projections">The projections.</param>
        /// <returns>
        /// A combined projection.
        /// </returns>
        public ProjectionDefinition<TSource> Combine(IEnumerable<ProjectionDefinition<TSource>> projections)
        {
            return new CombinedProjectionDefinition<TSource>(projections);
        }

        /// <summary>
        /// Creates a positional operator projection.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>A positional operator projection.</returns>
        public ProjectionDefinition<TSource> ElemMatch(FieldName<TSource> fieldName)
        {
            return new PositionalOperatorProjectionDefinition<TSource>(fieldName);
        }

        /// <summary>
        /// Creates a positional operator projection.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>A positional operator projection.</returns>
        public ProjectionDefinition<TSource> ElemMatch(Expression<Func<TSource, object>> fieldName)
        {
            return ElemMatch(new ExpressionFieldName<TSource>(fieldName));
        }

        /// <summary>
        /// Creates a projection that filters the contents of an array.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// An array filtering projection.
        /// </returns>
        public ProjectionDefinition<TSource> ElemMatch<TField, TItem>(FieldName<TSource, TField> fieldName, FilterDefinition<TItem> filter)
            where TField : IEnumerable<TItem>
        {
            return new ElementMatchProjectionDefinition<TSource, TField, TItem>(fieldName, filter);
        }

        /// <summary>
        /// Creates a projection that filters the contents of an array.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// An array filtering projection.
        /// </returns>
        public ProjectionDefinition<TSource> ElemMatch<TItem>(string fieldName, FilterDefinition<TItem> filter)
        {
            return ElemMatch(
                new StringFieldName<TSource, IEnumerable<TItem>>(fieldName),
                filter);
        }

        /// <summary>
        /// Creates a projection that filters the contents of an array.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// An array filtering projection.
        /// </returns>
        public ProjectionDefinition<TSource> ElemMatch<TField, TItem>(Expression<Func<TSource, TField>> fieldName, FilterDefinition<TItem> filter)
            where TField : IEnumerable<TItem>
        {
            return ElemMatch(new ExpressionFieldName<TSource, TField>(fieldName), filter);
        }

        /// <summary>
        /// Creates a projection that filters the contents of an array.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// An array filtering projection.
        /// </returns>
        public ProjectionDefinition<TSource> ElemMatch<TField, TItem>(Expression<Func<TSource, TField>> fieldName, Expression<Func<TItem, bool>> filter)
            where TField : IEnumerable<TItem>
        {
            return ElemMatch(new ExpressionFieldName<TSource, TField>(fieldName), new ExpressionFilterDefinition<TItem>(filter));
        }

        /// <summary>
        /// Creates a projection that excludes a field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>
        /// An exclusion projection.
        /// </returns>
        public ProjectionDefinition<TSource> Exclude(FieldName<TSource> fieldName)
        {
            return new SingleFieldProjectionDefinition<TSource>(fieldName, 0);
        }

        /// <summary>
        /// Creates a projection that excludes a field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>
        /// An exclusion projection.
        /// </returns>
        public ProjectionDefinition<TSource> Exclude(Expression<Func<TSource, object>> fieldName)
        {
            return Exclude(new ExpressionFieldName<TSource>(fieldName));
        }

        /// <summary>
        /// Creates a projection based on the expression.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// An expression projection.
        /// </returns>
        public ProjectionDefinition<TSource, TResult> Expression<TResult>(Expression<Func<TSource, TResult>> expression)
        {
            return new FindExpressionProjectionDefinition<TSource, TResult>(expression);
        }

        /// <summary>
        /// Creates a projection that includes a field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>
        /// An inclusion projection.
        /// </returns>
        public ProjectionDefinition<TSource> Include(FieldName<TSource> fieldName)
        {
            return new SingleFieldProjectionDefinition<TSource>(fieldName, 1);
        }

        /// <summary>
        /// Creates a projection that includes a field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>
        /// An inclusion projection.
        /// </returns>
        public ProjectionDefinition<TSource> Include(Expression<Func<TSource, object>> fieldName)
        {
            return Include(new ExpressionFieldName<TSource>(fieldName));
        }

        /// <summary>
        /// Creates a text score projection.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>
        /// A text score projection.
        /// </returns>
        public ProjectionDefinition<TSource> MetaTextScore(string fieldName)
        {
            return new SingleFieldProjectionDefinition<TSource>(fieldName, new BsonDocument("$meta", "textScore"));
        }

        /// <summary>
        /// Creates an array slice projection.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="limit">The limit.</param>
        /// <returns>
        /// An array slice projection.
        /// </returns>
        public ProjectionDefinition<TSource> Slice(FieldName<TSource> fieldName, int skip, int? limit = null)
        {
            var value = limit.HasValue ? (BsonValue)new BsonArray { skip, limit.Value } : skip;
            return new SingleFieldProjectionDefinition<TSource>(fieldName, new BsonDocument("$slice", value));
        }

        /// <summary>
        /// Creates an array slice projection.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="limit">The limit.</param>
        /// <returns>
        /// An array slice projection.
        /// </returns>
        public ProjectionDefinition<TSource> Slice(Expression<Func<TSource, object>> fieldName, int skip, int? limit = null)
        {
            return Slice(new ExpressionFieldName<TSource>(fieldName), skip, limit);
        }
    }

    internal sealed class CombinedProjectionDefinition<TSource> : ProjectionDefinition<TSource>
    {
        private readonly List<ProjectionDefinition<TSource>> _projections;

        public CombinedProjectionDefinition(IEnumerable<ProjectionDefinition<TSource>> projections)
        {
            _projections = Ensure.IsNotNull(projections, "projections").ToList();
        }

        public override BsonDocument Render(IBsonSerializer<TSource> sourceSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            var document = new BsonDocument();

            foreach (var projection in _projections)
            {
                var renderedProjection = projection.Render(sourceSerializer, serializerRegistry);

                foreach (var element in renderedProjection.Elements)
                {
                    // last one wins
                    document.Remove(element.Name);
                    document.Add(element);
                }
            }

            return document;
        }
    }

    internal sealed class ElementMatchProjectionDefinition<TSource, TField, TItem> : ProjectionDefinition<TSource>
    {
        private readonly FieldName<TSource, TField> _fieldName;
        private readonly FilterDefinition<TItem> _filter;

        public ElementMatchProjectionDefinition(FieldName<TSource, TField> fieldName, FilterDefinition<TItem> filter)
        {
            _fieldName = Ensure.IsNotNull(fieldName, "fieldName");
            _filter = filter;
        }

        public override BsonDocument Render(IBsonSerializer<TSource> sourceSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            var renderedFieldName = _fieldName.Render(sourceSerializer, serializerRegistry);

            var arraySerializer = renderedFieldName.FieldSerializer as IBsonArraySerializer;
            if (arraySerializer == null)
            {
                var message = string.Format("The serializer for field '{0}' must implement IBsonArraySerializer.", renderedFieldName.FieldName);
                throw new InvalidOperationException(message);
            }
            var itemSerializer = (IBsonSerializer<TItem>)arraySerializer.GetItemSerializationInfo().Serializer;

            var renderedFilter = _filter.Render(itemSerializer, serializerRegistry);

            return new BsonDocument(renderedFieldName.FieldName, new BsonDocument("$elemMatch", renderedFilter));
        }
    }

    internal sealed class PositionalOperatorProjectionDefinition<TSource> : ProjectionDefinition<TSource>
    {
        private readonly FieldName<TSource> _fieldName;

        public PositionalOperatorProjectionDefinition(FieldName<TSource> fieldName)
        {
            _fieldName = Ensure.IsNotNull(fieldName, "fieldName");
        }

        public override BsonDocument Render(IBsonSerializer<TSource> sourceSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            var renderedFieldName = _fieldName.Render(sourceSerializer, serializerRegistry);
            return new BsonDocument(renderedFieldName + ".$", 1);
        }
    }

    internal sealed class SingleFieldProjectionDefinition<TSource> : ProjectionDefinition<TSource>
    {
        private readonly FieldName<TSource> _fieldName;
        private readonly BsonValue _value;

        public SingleFieldProjectionDefinition(FieldName<TSource> fieldName, BsonValue value)
        {
            _fieldName = Ensure.IsNotNull(fieldName, "fieldName");
            _value = Ensure.IsNotNull(value, "value");
        }

        public override BsonDocument Render(IBsonSerializer<TSource> sourceSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            var renderedFieldName = _fieldName.Render(sourceSerializer, serializerRegistry);
            return new BsonDocument(renderedFieldName, _value);
        }
    }
}