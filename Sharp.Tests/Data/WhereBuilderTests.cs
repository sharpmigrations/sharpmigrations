using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NUnit.Framework;
using Moq;
using Sharp.Data;
using Sharp.Data.Databases;
using Sharp.Data.Filters;
using Sharp.Util;

namespace Sharp.Tests.Data {

	[TestFixture]
	public class WhereBuilderTests {
		private Mock<Dialect> _dialect;
		private WhereBuilder _whereBuilder;

		private string _wordWhere = "where";

		private Filter _filter1;
		private Filter _filter2;
		private Filter _filter3;
		private Filter _filter4;

		[SetUp]
		public void Init() {
			_dialect = new Mock<Dialect>();

			_dialect.Setup(p => p.WordWhere).Returns(_wordWhere);
			_dialect.Setup(p => p.GetParameterName(It.IsAny<int>()))
					.Returns((int input) => ":par" + input);

			_whereBuilder = new WhereBuilder(_dialect.Object, 0);

			_filter1 = CreateFilter(1);
			_filter2 = CreateFilter(2);
			_filter3 = CreateFilter(3);
			_filter4 = CreateFilter(4);
		}

		private Filter CreateFilter(int num) {
			return Filter.Eq("col" + num, num);
		}

		[Test]
		public void Should_append_where_word() {
			string whereSql = _whereBuilder.Build(_filter1);

			Assert.That(whereSql.Contains(_wordWhere));
		}

		[Test]
		public void Can_generate_whereSql_with_one_condition() {
			string whereSql = _whereBuilder.Build(_filter1);

			AssertSql.AreEqual("where (col1 = :par0)", whereSql);
		}

		[Test]
		public void Can_generate_where_with_logical_and() {
			Filter filter = Filter.And(_filter1, _filter2);

			string whereSql = _whereBuilder.Build(filter);

			AssertSql.AreEqual("where ((col1 = :par0) and (col2 = :par1))", whereSql);
		}

		[Test]
		public void Can_generate_where_with_logical_or() {
			Filter filter = Filter.Or(_filter1, _filter2);

			string whereSql = _whereBuilder.Build(filter);

			AssertSql.AreEqual("where ((col1 = :par0) or (col2 = :par1))", whereSql);
		}

		[Test]
		public void Can_generate_whereSql_with_composite_AND_and_OR() {
			Filter filterAnd1 = Filter.And(_filter1, _filter2);
			Filter filterAnd2 = Filter.And(_filter3, _filter4);

			Filter filter = Filter.Or(filterAnd1, filterAnd2);

			string whereSql = _whereBuilder.Build(filter);

			AssertSql.AreEqual("where (((col1 = :par0) and (col2 = :par1)) or ((col3 = :par2) and (col4 = :par3)))", whereSql);
		}
	}
}