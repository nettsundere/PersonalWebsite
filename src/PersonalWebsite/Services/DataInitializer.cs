using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WebsiteContent.Repositories;

namespace PersonalWebsite.Services
{
    /// <summary>
    /// Data initializer - data migration replacement.
    /// </summary>
    public class DataInitializer 
    {
        /// <summary>
        /// Service provider.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Required data repository.
        /// </summary>
        private readonly IRequiredDataRepository _requiredDataRepository;

        /// <summary>
        /// Internal content repository.
        /// </summary>
        private readonly IInternalContentRepository _internalContentRepository;

        /// <summary>
        /// Application user repository.
        /// </summary>
        private readonly IApplicationUserRepository _applicationUserRepository;
        
        /// <summary>
        /// Create <see cref="DataInitializer"/>.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        public DataInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _requiredDataRepository = serviceProvider.GetService<IRequiredDataRepository>();

            _internalContentRepository = _serviceProvider.GetService<IInternalContentRepository>();
            _applicationUserRepository = _serviceProvider.GetService<IApplicationUserRepository>();
        }

        /// <summary>
        /// Ensure required content is available.
        /// </summary>
        public async Task EnsureRequiredContentsAvailableAsync()
        {
            var contents = _requiredDataRepository.GetCriticalContent();
            await _internalContentRepository.EnsureContentListAvailableAsync(contents);
        }

        /// <summary>
        /// Ensure first user exists.
        /// </summary>
        public async Task EnsureInitialUserAvailableAsync()
        {
            var user = _requiredDataRepository.GetDefaultUserData();
            await _applicationUserRepository.EnsureUserAvailableAsync(user);
        }
    }
}
