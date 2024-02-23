using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.RefreshableTokenCreator
{
	[TestFixture]
	public class FromTokenBasedQueryWithTokenBasedQueryTests
	{
		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenTokenBasedQueryIsNull_ThrowsNotSupportedException()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.FromTokenBasedQuery((ITokenBasedQuery) null));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenTokenBasedQueryIsNull_ThrowsNotSupportedExceptionWhereMessageIsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.FromTokenBasedQuery((ITokenBasedQuery) null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message, Is.Not.Null);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenTokenBasedQueryIsNull_ThrowsNotSupportedExceptionWithInformativeMessage()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.FromTokenBasedQuery((ITokenBasedQuery) null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message, Is.EqualTo($"Cannot build an {nameof(IRefreshableToken)} from {nameof(ITokenBasedQuery)}."));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenTokenBasedQueryIsNotNull_ReturnsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.FromTokenBasedQuery(CreateTokenBasedQuery()));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenTokenBasedQueryIsNotNull_ThrowsNotSupportedExceptionWhereMessageIsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.FromTokenBasedQuery(CreateTokenBasedQuery()));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message, Is.Not.Null);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenTokenBasedQueryIsNotNull_ThrowsNotSupportedExceptionWithInformativeMessage()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			ITokenBasedQuery tokenBasedQuery = CreateTokenBasedQuery();
			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.FromTokenBasedQuery(tokenBasedQuery));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message, Is.EqualTo($"Cannot build an {nameof(IRefreshableToken)} from {tokenBasedQuery.GetType().Name}."));
			// ReSharper restore PossibleNullReferenceException
		}

		private static ITokenCreator<IRefreshableToken> CreateSut()
		{
			return new Domain.Security.RefreshableTokenCreator();
		}

		private ITokenBasedQuery CreateTokenBasedQuery()
		{
			return CreateTokenBasedQueryMock().Object;
		}

		private Mock<ITokenBasedQuery> CreateTokenBasedQueryMock()
		{
			return new Mock<ITokenBasedQuery>();
		}
	}
}