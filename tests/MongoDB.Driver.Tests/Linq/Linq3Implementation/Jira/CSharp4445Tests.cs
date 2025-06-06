﻿/* Copyright 2010-present MongoDB Inc.
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

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongoDB.Driver.TestHelpers;
using Xunit;

namespace MongoDB.Driver.Tests.Linq.Linq3Implementation.Jira
{
    public class CSharp4445Tests : LinqIntegrationTest<CSharp4445Tests.ClassFixture>
    {
        public CSharp4445Tests(ClassFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public void AggregateFluent_Limit_with_int_should_work()
        {
            var collection = Fixture.Collection;
            int limit = 1;

            var aggregate =
                collection.Aggregate()
                .Limit(limit);

            var stages = Translate(collection, aggregate);

            AssertStages(stages, "{ $limit : 1 }");
            stages[0]["$limit"].BsonType.Should().Be(BsonType.Int64);

            var results = aggregate.ToList();
            results.Select(x => x.Id).Should().Equal(1);
        }

        [Fact]
        public void AggregateFluent_Limit_with_long_should_work()
        {
            var collection = Fixture.Collection;
            long limit = 1;

            var aggregate =
                collection.Aggregate()
                .Limit(limit);

            var stages = Translate(collection, aggregate);

            AssertStages(stages, "{ $limit : 1 }");
            stages[0]["$limit"].BsonType.Should().Be(BsonType.Int64);

            var results = aggregate.ToList();
            results.Select(x => x.Id).Should().Equal(1);
        }

        [Fact]
        public void AggregateFluent_Skip_with_int_should_work()
        {
            var collection = Fixture.Collection;
            int skip = 1;

            var aggregate =
                collection.Aggregate()
                .Skip(skip);

            var stages = Translate(collection, aggregate);

            AssertStages(stages, "{ $skip : 1 }");
            stages[0]["$skip"].BsonType.Should().Be(BsonType.Int64);

            var results = aggregate.ToList();
            results.Select(x => x.Id).Should().Equal(2);
        }

        [Fact]
        public void AggregateFluent_Skip_with_long_should_work()
        {
            var collection = Fixture.Collection;
            long limit = 1;

            var aggregate =
                collection.Aggregate()
                .Skip(limit);

            var stages = Translate(collection, aggregate);

            AssertStages(stages, "{ $skip : 1 }");
            stages[0]["$skip"].BsonType.Should().Be(BsonType.Int64);

            var results = aggregate.ToList();
            results.Select(x => x.Id).Should().Equal(2);
        }

        [Fact]
        public void PipelineDefinitionBuilder_Limit_with_int_should_work()
        {
            var collection = Fixture.Collection;
            int limit = 1;

            var pipeline =
                new EmptyPipelineDefinition<C>()
                .Limit(limit);

            var stages = Translate(pipeline, collection.DocumentSerializer, translationOptions: null);

            AssertStages(stages, "{ $limit : 1 }");
            stages[0]["$limit"].BsonType.Should().Be(BsonType.Int64);

            var results = collection.Aggregate(pipeline).ToList();
            results.Select(x => x.Id).Should().Equal(1);
        }

        [Fact]
        public void PipelineDefinitionBuilder_Limit_with_long_should_work()
        {
            var collection = Fixture.Collection;
            long limit = 1;

            var pipeline =
                new EmptyPipelineDefinition<C>()
                .Limit(limit);

            var stages = Translate(pipeline, collection.DocumentSerializer, translationOptions: null);

            AssertStages(stages, "{ $limit : 1 }");
            stages[0]["$limit"].BsonType.Should().Be(BsonType.Int64);

            var results = collection.Aggregate(pipeline).ToList();
            results.Select(x => x.Id).Should().Equal(1);
        }

        [Fact]
        public void PipelineDefinitionBuilder_Skip_with_int_should_work()
        {
            var collection = Fixture.Collection;
            int skip = 1;

            var pipeline =
                new EmptyPipelineDefinition<C>()
                .Skip(skip);

            var stages = Translate(pipeline, collection.DocumentSerializer, translationOptions: null);

            AssertStages(stages, "{ $skip : 1 }");
            stages[0]["$skip"].BsonType.Should().Be(BsonType.Int64);

            var results = collection.Aggregate(pipeline).ToList();
            results.Select(x => x.Id).Should().Equal(2);
        }

        [Fact]
        public void PipelineDefinitionBuilder_Skip_with_long_should_work()
        {
            var collection = Fixture.Collection;
            long skip = 1;

            var pipeline =
                new EmptyPipelineDefinition<C>()
                .Skip(skip);

            var stages = Translate(pipeline, collection.DocumentSerializer, translationOptions: null);

            AssertStages(stages, "{ $skip : 1 }");
            stages[0]["$skip"].BsonType.Should().Be(BsonType.Int64);

            var results = collection.Aggregate(pipeline).ToList();
            results.Select(x => x.Id).Should().Equal(2);
        }

        [Fact]
        public void Queryable_Skip_with_int_should_work()
        {
            var collection = Fixture.Collection;
            int count = 1;

            var queryable =
                collection.AsQueryable()
                .Skip(count);

            var stages = Translate(collection, queryable);

            AssertStages(stages, "{ $skip : 1 }");
            stages[0]["$skip"].BsonType.Should().Be(BsonType.Int64);

            var results = queryable.ToList();
            results.Select(x => x.Id).Should().Equal(2);
        }

        [Fact]
        public void Queryable_Skip_with_long_should_work()
        {
            var collection = Fixture.Collection;
            long count = 1;

            var queryable =
                collection.AsQueryable()
                .Skip(count);

            var stages = Translate(collection, queryable);

            AssertStages(stages, "{ $skip : 1 }");
            stages[0]["$skip"].BsonType.Should().Be(BsonType.Int64);

            var results = queryable.ToList();
            results.Select(x => x.Id).Should().Equal(2);
        }

        [Fact]
        public void Queryable_Take_with_int_should_work()
        {
            var collection = Fixture.Collection;
            int count = 1;

            var queryable =
                collection.AsQueryable()
                .Take(count);

            var stages = Translate(collection, queryable);

            AssertStages(stages, "{ $limit : 1 }");
            stages[0]["$limit"].BsonType.Should().Be(BsonType.Int64);

            var results = queryable.ToList();
            results.Select(x => x.Id).Should().Equal(1);
        }

        [Fact]
        public void Queryable_Take_with_long_should_work()
        {
            var collection = Fixture.Collection;
            long count = 1;

            var queryable =
                collection.AsQueryable()
                .Take(count);

            var stages = Translate(collection, queryable);

            AssertStages(stages, "{ $limit : 1 }");
            stages[0]["$limit"].BsonType.Should().Be(BsonType.Int64);

            var results = queryable.ToList();
            results.Select(x => x.Id).Should().Equal(1);
        }

        public class C
        {
            public int Id { get; set; }
        }

        public sealed class ClassFixture : MongoCollectionFixture<C>
        {
            protected override IEnumerable<C> InitialData =>
            [
                new C { Id = 1 },
                new C { Id = 2 }
            ];
        }
    }
}
