using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.MediaBinding
{
	[TestFixture]
	public class AllowDeletionTests
	{
		#region Private variables

		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void AllowDeletion_WhenCalled_AssertDeletableIsTrueOnMediaPersonality(bool deletable)
		{
			IDeletable sut = CreateSut(deletable);

			Assert.That(sut.Deletable, Is.EqualTo(deletable));

			sut.AllowDeletion();

			Assert.That(sut.Deletable, Is.True);
		}

		private IDeletable CreateSut(bool deletable)
		{
			return new Domain.MediaLibrary.MediaBinding(_fixture.BuildMediaMock().Object, _fixture.Create<MediaRole>(), _fixture.BuildMediaPersonalityMock().Object, deletable);
		}
	}
}