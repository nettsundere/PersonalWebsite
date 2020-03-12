using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonalWebsite.Models;
using WebsiteContent.Lib;
using WebsiteContent.Models;
using Xunit;

namespace PersonalWebsite.Tests.Integration.Models
{
    public class ContentTests: IDisposable
    {
        /// <summary>
        /// Data DB context.
        /// </summary>
        private readonly DataDbContext _dataDbContext;

        /// <summary>
        /// Create <see cref="ContentTests"/>.
        /// </summary>
        public ContentTests()
        {
            // Database setup
            var connection = InMemoryConnectionHelper.SetupConnection();

            var services = new ServiceCollection();
            services.AddEntityFrameworkSqlite()
                .AddDbContext<DataDbContext>(options =>
                        options.UseSqlite(connection),
                    ServiceLifetime.Transient
                );

            var serviceProvider = services.BuildServiceProvider();

            _dataDbContext = serviceProvider.GetService<DataDbContext>();
            _dataDbContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task TestCanLoadContent()
        {
            var placeholderContentMarkup = "<h1>Test</h1>";
            var expectation = new Content
            {
                InternalCaption = PredefinedPages.Welcome.ToString(),
                Translations = new []
                {
                    new Translation { UrlName = "Welcome", Title = "Welcome", ContentMarkup = placeholderContentMarkup, Description = "Test1", State = DataAvailabilityState.published, Version = LanguageDefinition.en_us },
                    new Translation { UrlName = "Willkommen", Title = "Willkommen", ContentMarkup = placeholderContentMarkup, Description = "Test2", State = DataAvailabilityState.published, Version = LanguageDefinition.de_de },
                    new Translation { UrlName = "Privet",  Title = "Приветствую", ContentMarkup = placeholderContentMarkup, Description = "Test3", State = DataAvailabilityState.published, Version = LanguageDefinition.ru_ru }
                }
            };

            _dataDbContext.Content.Add(expectation);

            await _dataDbContext.SaveChangesAsync();

            var content = await _dataDbContext.Content.FirstAsync();
            Assert.NotNull(content.Translations);
        }

        public void Dispose()
        {
            _dataDbContext.Dispose();
        }
    }
}