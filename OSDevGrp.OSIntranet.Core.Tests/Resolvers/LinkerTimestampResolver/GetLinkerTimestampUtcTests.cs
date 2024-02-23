using NUnit.Framework;
using System;
using System.Reflection;

namespace OSDevGrp.OSIntranet.Core.Tests.Resolvers.LinkerTimestampResolver
{
	[TestFixture]
	public class GetLinkerTimestampUtcTests
	{
		[Test]
		[Category("UnitTest")]
		public void GetLinkerTimestampUtc_WhenAssemblyIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Core.Resolvers.LinkerTimestampResolver.GetLinkerTimestampUtc(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("assembly"));
		}

		[Test]
		[Category("UnitTest")]
		public void GetLinkerTimestampUtc_WhenCalled_ReturnsLinkerTimestamp()
		{
			DateTime result = Core.Resolvers.LinkerTimestampResolver.GetLinkerTimestampUtc(GetAssembly());

			Assert.That(result, Is.GreaterThan(DateTime.MinValue));
		}

		[Test]
		[Category("UnitTest")]
		public void GetLinkerTimestampUtc_WhenCalled_ReturnsLinkerTimestampAsUniversalTime()
		{
			DateTime result = Core.Resolvers.LinkerTimestampResolver.GetLinkerTimestampUtc(GetAssembly());

			Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
		}

		private Assembly GetAssembly()
		{
			return typeof(GetLinkerTimestampUtcTests).Assembly;
		}
	}
}