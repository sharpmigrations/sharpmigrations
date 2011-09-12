using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sharp.Data.Filters;
using Sharp.Data.Query;

namespace Sharp.Tests.Data.Filters {
	[TestFixture]
	public class CompareOperatorToSymbolTests {

		[Test]
		public void Should_return_equal_symbol() {
			Assert.AreEqual("=",CompareOperatorToSymbol.Get(CompareOperator.Equals));
		}

		[Test]
		public void Should_return_greater_symbol() {
			Assert.AreEqual(">", CompareOperatorToSymbol.Get(CompareOperator.GreaterThan));
		}

		[Test]
		public void Should_return_less_than_symbol() {
			Assert.AreEqual("<", CompareOperatorToSymbol.Get(CompareOperator.LessThan));
		}

		[Test]
		public void Should_return_greater_or_equal_symbol() {
			Assert.AreEqual(">=", CompareOperatorToSymbol.Get(CompareOperator.GreaterOrEqualThan));
		}

		[Test]
		public void Should_return_less_or_equal_symbol() {
			Assert.AreEqual("<=", CompareOperatorToSymbol.Get(CompareOperator.LessOrEqualThan));
		}
	}
}
