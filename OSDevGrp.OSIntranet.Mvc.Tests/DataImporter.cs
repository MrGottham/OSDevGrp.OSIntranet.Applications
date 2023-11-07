using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic;
using OSDevGrp.OSIntranet.BusinessLogic.Common.CommandHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Common.QueryHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
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
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Migrations;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        [TestCase(@"Medier.liu")]
        [Ignore("Test which imports data and should only be run once")]
        public async Task Import_Medias_FromFile(string fileName)
        {
	        IList<IMediaLibraryCommand> mediaLibraryCommands = new List<IMediaLibraryCommand>();

	        using (IServiceScope serviceScope = _serviceProvider.CreateScope())
	        {
		        IValidator validator = serviceScope.ServiceProvider.GetRequiredService<IValidator>();
		        IClaimResolver claimResolver = serviceScope.ServiceProvider.GetRequiredService<IClaimResolver>();
		        IMediaLibraryRepository mediaLibraryRepository = serviceScope.ServiceProvider.GetRequiredService<IMediaLibraryRepository>();
		        ICommonRepository commonRepository = serviceScope.ServiceProvider.GetRequiredService<ICommonRepository>();

		        IConfiguration configuration = serviceScope.ServiceProvider.GetRequiredService<IConfiguration>();
		        Guid existingMediaPersonalityIdentifier = Guid.Parse(configuration["TestData:MediaLibrary:ExistingMediaPersonalityIdentifier"]!);
                Guid? existingMovieIdentifier = Guid.Parse(configuration["TestData:MediaLibrary:ExistingMovieIdentifier"]!);
                Guid? existingMusicIdentifier = Guid.Parse(configuration["TestData:MediaLibrary:ExistingMusicIdentifier"]!);
                Guid? existingBookIdentifier = Guid.Parse(configuration["TestData:MediaLibrary:ExistingBookIdentifier"]!);

				mediaLibraryCommands.Add(BuildCreateMichaelFalchCommand(existingMediaPersonalityIdentifier, validator, claimResolver, mediaLibraryRepository, commonRepository));
				mediaLibraryCommands.Add(BuildCreateRobertDeNiroCommand(Guid.NewGuid(), validator, claimResolver, mediaLibraryRepository, commonRepository));

				await ImportFromFile(fileName, Encoding.UTF8, (lineNumber, line) =>
		        {
			        if (lineNumber <= 1)
			        {
				        return Task.CompletedTask;
			        }

			        if (string.IsNullOrWhiteSpace(line))
			        {
				        return Task.CompletedTask;
			        }

					string[] values = CsvSplitter(QuoteRemover(line), "\";\"");
			        switch (int.Parse(values[0], CultureInfo.InvariantCulture))
			        {
				        case 4000:
					        mediaLibraryCommands.Add(BuildCreateMovieCommand(existingMovieIdentifier ?? Guid.NewGuid(), values, validator, claimResolver, mediaLibraryRepository, commonRepository));
					        existingMovieIdentifier = null;
					        break;

				        case 4010:
					        mediaLibraryCommands.Add(BuildCreateMusicCommand(existingMusicIdentifier ?? Guid.NewGuid(), values, validator, claimResolver, mediaLibraryRepository, commonRepository));
					        existingMusicIdentifier = null;
					        break;

				        case 4020:
					        mediaLibraryCommands.Add(BuildCreateBookCommand(existingBookIdentifier ?? Guid.NewGuid(), values, validator, claimResolver, mediaLibraryRepository, commonRepository));
					        existingBookIdentifier = null;
					        break;

				        default:
					        throw new NotSupportedException($"Unsupported table: {values[0]}");
			        }

			        return Task.CompletedTask;
		        });
	        }

	        foreach (IMediaLibraryCommand mediaLibraryCommand in mediaLibraryCommands)
	        {
		        if (mediaLibraryCommand is ICreateMediaPersonalityCommand createMediaPersonalityCommand)
		        {
			        await ExecuteCommand(createMediaPersonalityCommand);
					continue;
				}

		        if (mediaLibraryCommand is ICreateMovieCommand createMovieCommand)
		        {
			        await ExecuteCommand(createMovieCommand);
			        continue;
		        }

		        if (mediaLibraryCommand is ICreateMusicCommand createMusicCommand)
		        {
			        await ExecuteCommand(createMusicCommand);
			        continue;
		        }

		        if (mediaLibraryCommand is ICreateBookCommand createBookCommand)
		        {
			        await ExecuteCommand(createBookCommand);
			        continue;
		        }

		        throw new NotSupportedException($"Unsupported command: {mediaLibraryCommand}");
	        }

	        Assert.That((await ExecuteQuery<IGetMediaPersonalityCollectionQuery, IEnumerable<IMediaPersonality>>(MediaLibraryQueryFactory.BuildGetMediaPersonalityCollectionQuery(null))).Count(), Is.EqualTo(2));
	        Assert.That((await ExecuteQuery<IGetMediaCollectionQuery, IEnumerable<IMedia>>(MediaLibraryQueryFactory.BuildGetMediaCollectionQuery(null))).Count(), Is.EqualTo(204));
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
            Claim nameClaim = ClaimHelper.CreateNameClaim(MigrationHelper.MigrationUserIdentifier);
            Claim mediaLibraryModifierClaim = ClaimHelper.CreateMediaLibraryModifierClaim();

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {nameClaim, mediaLibraryModifierClaim});

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

        private static ICreateMediaPersonalityCommand BuildCreateMichaelFalchCommand(Guid mediaPersonalityIdentifier, IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
        {
	        NullGuard.NotNull(validator, nameof(validator))
		        .NotNull(claimResolver, nameof(claimResolver))
		        .NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
		        .NotNull(commonRepository, nameof(commonRepository));

	        return BuildCreateMediaPersonalityCommand(
		        mediaPersonalityIdentifier,
		        "Michael",
		        null,
		        "Falch",
		        1,
		        new DateTime(1956, 9, 16),
		        null,
		        "https://falch.dk/",
		        Convert.FromBase64String("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"),
		        validator,
		        claimResolver,
		        mediaLibraryRepository,
		        commonRepository);
        }

        private static ICreateMediaPersonalityCommand BuildCreateRobertDeNiroCommand(Guid mediaPersonalityIdentifier, IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
        {
	        NullGuard.NotNull(validator, nameof(validator))
		        .NotNull(claimResolver, nameof(claimResolver))
		        .NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
		        .NotNull(commonRepository, nameof(commonRepository));

	        return BuildCreateMediaPersonalityCommand(
		        mediaPersonalityIdentifier,
		        "Robert",
		        "De",
		        "Niro",
		        5,
		        new DateTime(1943, 8, 17),
		        null,
		        "https://da.wikipedia.org/wiki/Robert_De_Niro",
		        Convert.FromBase64String("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"),
		        validator,
		        claimResolver,
		        mediaLibraryRepository,
		        commonRepository);
        }

        private static ICreateMediaPersonalityCommand BuildCreateMediaPersonalityCommand(Guid mediaPersonalityIdentifier, string givenName, string middleName, string surname, int nationalityIdentifier, DateTime? birthDate, DateTime? dateOfDead, string url, byte[] image, IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
        {
	        NullGuard.NotNullOrWhiteSpace(surname, nameof(surname))
		        .NotNull(validator, nameof(validator))
		        .NotNull(claimResolver, nameof(claimResolver))
		        .NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
		        .NotNull(commonRepository, nameof(commonRepository));

	        ICreateMediaPersonalityCommand createMediaPersonalityCommand = MediaLibraryCommandFactory.BuildCreateMediaPersonalityCommand(
		        mediaPersonalityIdentifier,
		        givenName,
		        middleName,
		        surname,
		        nationalityIdentifier,
		        birthDate,
		        dateOfDead,
		        url,
		        image);

	        createMediaPersonalityCommand.Validate(validator, claimResolver, mediaLibraryRepository, commonRepository);

	        return createMediaPersonalityCommand;
		}

        private static ICreateMovieCommand BuildCreateMovieCommand(Guid mediaIdentifier, string[] values, IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
        {
	        NullGuard.NotNull(values, nameof(values))
		        .NotNull(validator, nameof(validator))
		        .NotNull(claimResolver, nameof(claimResolver))
		        .NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
		        .NotNull(commonRepository, nameof(commonRepository));

	        ICreateMovieCommand createMovieCommand = MediaLibraryCommandFactory.BuildCreateMovieCommand(
		        mediaIdentifier,
		        values[3],
		        values[4],
		        values[6],
		        values[7],
		        ToMovieGenreIdentifier(int.Parse(values[8], CultureInfo.InvariantCulture)),
		        null,
		        ToMediaTypeIdentifier(int.Parse(values[10], CultureInfo.InvariantCulture)),
		        string.IsNullOrWhiteSpace(values[12]) == false ? short.Parse(values[12], CultureInfo.InvariantCulture) : null,
		        string.IsNullOrWhiteSpace(values[13]) == false ? short.Parse(values[13], CultureInfo.InvariantCulture) : null,
		        null,
		        null,
		        Array.Empty<Guid>(),
		        Array.Empty<Guid>());

	        createMovieCommand.Validate(validator, claimResolver, mediaLibraryRepository, commonRepository);

	        return createMovieCommand;
        }

        private static ICreateMusicCommand BuildCreateMusicCommand(Guid mediaIdentifier, string[] values, IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
        {
	        NullGuard.NotNull(values, nameof(values))
		        .NotNull(validator, nameof(validator))
		        .NotNull(claimResolver, nameof(claimResolver))
		        .NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
		        .NotNull(commonRepository, nameof(commonRepository));

	        ICreateMusicCommand createMusicCommand = MediaLibraryCommandFactory.BuildCreateMusicCommand(
		        mediaIdentifier,
		        values[3],
		        values[4],
		        values[6],
		        values[7],
		        ToMusicGenreIdentifier(int.Parse(values[8], CultureInfo.InvariantCulture)),
		        ToMediaTypeIdentifier(int.Parse(values[10], CultureInfo.InvariantCulture)),
		        string.IsNullOrWhiteSpace(values[12]) == false ? short.Parse(values[12], CultureInfo.InvariantCulture) : null,
		        string.IsNullOrWhiteSpace(values[13]) == false ? short.Parse(values[13], CultureInfo.InvariantCulture) : null,
		        null,
		        null,
		        Array.Empty<Guid>());

	        createMusicCommand.Validate(validator, claimResolver, mediaLibraryRepository, commonRepository);

	        return createMusicCommand;
        }

        private static ICreateBookCommand BuildCreateBookCommand(Guid mediaIdentifier, string[] values, IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
        {
	        NullGuard.NotNull(values, nameof(values))
		        .NotNull(validator, nameof(validator))
		        .NotNull(claimResolver, nameof(claimResolver))
		        .NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
		        .NotNull(commonRepository, nameof(commonRepository));
	    
	        ICreateBookCommand createBookCommand = MediaLibraryCommandFactory.BuildCreateBookCommand(
		        mediaIdentifier,
		        values[3],
		        values[4],
		        values[6],
		        values[7],
		        ToBookGenreIdentifier(int.Parse(values[8], CultureInfo.InvariantCulture)),
		        null,
		        ToMediaTypeIdentifier(int.Parse(values[10], CultureInfo.InvariantCulture)),
		        string.IsNullOrWhiteSpace(values[14]) == false ? values[14] : null,
		        string.IsNullOrWhiteSpace(values[12]) == false ? short.Parse(values[12], CultureInfo.InvariantCulture) : null,
		        null,
		        null,
		        Array.Empty<Guid>());

	        createBookCommand.Validate(validator, claimResolver, mediaLibraryRepository, commonRepository);

	        return createBookCommand;
        }

        private static string[] CsvSplitter(string line, char separator)
        {
	        NullGuard.NotNullOrWhiteSpace(line, nameof(line))
		        .NotNull(separator, nameof(separator));

	        return CsvSplitter(line, $"{separator}");
        }

        private static string[] CsvSplitter(string line, string separator)
        {
	        NullGuard.NotNullOrWhiteSpace(line, nameof(line))
		        .NotNull(separator, nameof(separator));

	        return line.Split(separator)
		        .Select(value => string.IsNullOrWhiteSpace(value) ? value : QuoteRemover(value))
		        .ToArray();
        }

        private static string QuoteRemover(string value)
        {
	        NullGuard.NotNullOrWhiteSpace(value, nameof(value));

	        if (value.StartsWith('"') && value.EndsWith('"'))
	        {
		        return value.Substring(1, value.Length - 2);
	        }

	        return value;
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