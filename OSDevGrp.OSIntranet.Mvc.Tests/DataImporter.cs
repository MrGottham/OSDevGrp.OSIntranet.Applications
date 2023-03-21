using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic;
using OSDevGrp.OSIntranet.BusinessLogic.Common.CommandHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Common.QueryHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Core.Resolvers;
using OSDevGrp.OSIntranet.Domain;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories;
using OSDevGrp.OSIntranet.Repositories.Migrations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Mvc.Tests
{
	[TestFixture]
    public class DataImporter
    {
        #region Private variables

        private IServiceProvider _serviceProvider;

        #endregion

        [SetUp]
        public void SetUp()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            // ReSharper disable UnusedParameter.Local
            serviceCollection.AddTransient(serviceProvider => CreateConfiguration());
            serviceCollection.AddTransient(serviceProvider => CreatePrincipalResolver());
            serviceCollection.AddTransient(serviceProvider => CreateLoggerFactory());
            // ReSharper restore UnusedParameter.Local

            serviceCollection.AddEventPublisher();
            serviceCollection.AddQueryBus();
            serviceCollection.AddQueryHandlers(typeof(GetLetterHeadQueryHandler).Assembly);
            serviceCollection.AddCommandBus();
            serviceCollection.AddCommandHandlers(typeof(CreateLetterHeadCommandHandler).Assembly);
            serviceCollection.AddDomainLogic();
            serviceCollection.AddBusinessLogicValidators();
            serviceCollection.AddBusinessLogicHelpers();
            serviceCollection.AddRepositories();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [TearDown]
        public void TearDown()
        {
            ((ServiceProvider)_serviceProvider).DisposeAsync()
                .GetAwaiter()
                .GetResult();
        }

        [Test]
        [Category("DataImport")]
        [TestCase("tabel.liu")]
        [Ignore("Test which imports data and should only be run once")]
        public async Task Import_MovieGenres_FromFile(string fileName)
        {
	        await ImportFromFile(fileName, Encoding.UTF8, async (lineNumber, line) =>
	        {
		        if (lineNumber <= 2)
		        {
			        return;
		        }

		        string[] values = CsvSplitter(line, ',');
		        if (int.TryParse(values[24], out int tableNumber) == false || tableNumber != 4030)
		        {
			        return;
		        }

		        int number = ToMovieGenreIdentifier(int.Parse(values[0]));

		        IMovieGenre movieGenre = await ExecuteQuery<IGetMovieGenreQuery, IMovieGenre>(MediaLibraryQueryFactory.BuildGetMovieGenreQuery(number));
		        if (movieGenre != null)
		        {
			        return;
		        }

		        await ExecuteCommand(MediaLibraryCommandFactory.BuildCreateMovieGenreCommand(number, values[1]));
	        });

            Assert.That((await ExecuteQuery<EmptyQuery, IEnumerable<IMovieGenre>>(new EmptyQuery())).Count(), Is.EqualTo(19));
        }

        [Test]
        [Category("DataImport")]
        [TestCase("tabel.liu")]
        [Ignore("Test which imports data and should only be run once")]
        public async Task Import_MusicGenres_FromFile(string fileName)
        {
	        await ImportFromFile(fileName, Encoding.UTF8, async (lineNumber, line) =>
	        {
		        if (lineNumber <= 2)
		        {
			        return;
		        }

		        string[] values = CsvSplitter(line, ',');
		        if (int.TryParse(values[24], out int tableNumber) == false || tableNumber != 4040)
		        {
			        return;
		        }

		        int number = ToMusicGenreIdentifier(int.Parse(values[0]));

		        IMusicGenre musicGenre = await ExecuteQuery<IGetMusicGenreQuery, IMusicGenre>(MediaLibraryQueryFactory.BuildGetMusicGenreQuery(number));
		        if (musicGenre != null)
		        {
			        return;
		        }

		        await ExecuteCommand(MediaLibraryCommandFactory.BuildCreateMusicGenreCommand(number, values[1]));
	        });

	        Assert.That((await ExecuteQuery<EmptyQuery, IEnumerable<IMusicGenre>>(new EmptyQuery())).Count(), Is.EqualTo(7));
        }

		[Test]
        [Category("DataImport")]
        [TestCase("tabel.liu")]
		[Ignore("Test which imports data and should only be run once")]
        public async Task Import_BookGenres_FromFile(string fileName)
        {
	        await ImportFromFile(fileName, Encoding.UTF8, async (lineNumber, line) =>
	        {
		        if (lineNumber <= 2)
		        {
			        return;
		        }

		        string[] values = CsvSplitter(line, ',');
		        if (int.TryParse(values[24], out int tableNumber) == false || tableNumber != 4050)
		        {
			        return;
		        }

		        int number = ToBookGenreIdentifier(int.Parse(values[0]));

		        IBookGenre bookGenre = await ExecuteQuery<IGetBookGenreQuery, IBookGenre>(MediaLibraryQueryFactory.BuildGetBookGenreQuery(number));
		        if (bookGenre != null)
		        {
			        return;
		        }

		        await ExecuteCommand(MediaLibraryCommandFactory.BuildCreateBookGenreCommand(number, values[1]));
	        });

	        Assert.That((await ExecuteQuery<EmptyQuery, IEnumerable<IBookGenre>>(new EmptyQuery())).Count(), Is.EqualTo(8));
        }

		[Test]
        [Category("DataImport")]
        [TestCase("tabel.liu")]
		[Ignore("Test which imports data and should only be run once")]
        public async Task Import_MediaTypes_FromFile(string fileName)
        {
	        await ImportFromFile(fileName, Encoding.UTF8, async (lineNumber, line) =>
	        {
		        if (lineNumber <= 2)
		        {
			        return;
		        }

		        string[] values = CsvSplitter(line, ',');
		        if (int.TryParse(values[24], out int tableNumber) == false || tableNumber != 4070)
		        {
			        return;
		        }

		        int number = ToMediaTypeIdentifier(int.Parse(values[0]));

		        IMediaType mediaType = await ExecuteQuery<IGetMediaTypeQuery, IMediaType>(MediaLibraryQueryFactory.BuildGetMediaTypeQuery(number));
		        if (mediaType != null)
		        {
			        return;
		        }

		        await ExecuteCommand(MediaLibraryCommandFactory.BuildCreateMediaTypeCommand(number, values[1]));
	        });

	        Assert.That((await ExecuteQuery<EmptyQuery, IEnumerable<IMediaType>>(new EmptyQuery())).Count(), Is.EqualTo(11));
        }

		private IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder()
                .AddUserSecrets<DataImporter>()
                .Build();
        }

        private IPrincipalResolver CreatePrincipalResolver()
        {
            IPrincipal currentPrincipal = CreateClaimsPrincipal();
            return new GenericPrincipalResolver(currentPrincipal);
        }

        private ClaimsPrincipal CreateClaimsPrincipal()
        {
            Claim nameClaim = new Claim(ClaimTypes.Name, MigrationHelper.MigrationUserIdentifier);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { nameClaim });

            return new ClaimsPrincipal(claimsIdentity);
        }

        private ILoggerFactory CreateLoggerFactory()
        {
            return NullLoggerFactory.Instance;
        }

        public async Task<TResult> ExecuteQuery<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            NullGuard.NotNull(query, nameof(query));

            using IServiceScope serviceScope = _serviceProvider.CreateScope();

            IQueryBus queryBus = serviceScope.ServiceProvider.GetRequiredService<IQueryBus>();

            return await queryBus.QueryAsync<TQuery, TResult>(query);
        }

        private async Task ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            NullGuard.NotNull(command, nameof(command));

            using IServiceScope serviceScope = _serviceProvider.CreateScope();

            ICommandBus commandBus = serviceScope.ServiceProvider.GetRequiredService<ICommandBus>();

            await commandBus.PublishAsync(command);
        }

        private async Task ImportFromFile(string fileName, Encoding encoding, Func<int, string, Task> lineHandler)
        {
            NullGuard.NotNullOrWhiteSpace(fileName, nameof(fileName))
                .NotNull(encoding, nameof(encoding))
                .NotNull(lineHandler, nameof(lineHandler));

            await using FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader streamReader = new StreamReader(fileStream, encoding);
            int lineNumber = 0;
            while (streamReader.EndOfStream == false)
            {
                await lineHandler(++lineNumber, await streamReader.ReadLineAsync());
            }
        }

        private string[] CsvSplitter(string line, char separator)
        {
	        NullGuard.NotNullOrWhiteSpace(line, nameof(line))
		        .NotNull(separator, nameof(separator));

            return line.Split(separator)
                .Select(value => 
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        return value;
                    }

                    if (value.StartsWith('"') && value.EndsWith('"'))
                    {
                        return value.Substring(1, value.Length - 2);
                    }

                    return value;
                })
                .ToArray();
        }

        private static int ToMovieGenreIdentifier(int number)
        {
	        switch (number)
	        {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
	                return number;

		        default:
			        throw new NotSupportedException($"Unhandled number: {number}");
	        }
        }

        private static int ToMusicGenreIdentifier(int number)
        {
	        switch (number)
	        {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
	                return number;

		        default:
			        throw new NotSupportedException($"Unhandled number: {number}");
	        }
        }

        private static int ToBookGenreIdentifier(int number)
        {
	        switch (number)
	        {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
	                return number;

		        default:
			        throw new NotSupportedException($"Unhandled number: {number}");
	        }
        }

        private static int ToMediaTypeIdentifier(int number)
        {
	        switch (number)
	        {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
	                return number;

                case 10:
	                return 9;

                case 11:
	                return 10;

                case 19:
	                return 11;

                default:
                    throw new NotSupportedException($"Unhandled number: {number}");
	        }
        }
    }
}