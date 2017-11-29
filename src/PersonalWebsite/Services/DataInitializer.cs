using Microsoft.Extensions.DependencyInjection;
using PersonalWebsite.Repositories;
using System;
using System.Linq;
using WebsiteContent.Repositories;

namespace PersonalWebsite.Services
{
    /// <summary>
    /// Data initializer - data migration replacement.
    /// </summary>
    public class DataInitializer : IDisposable
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
        /// Disposing status.
        /// </summary>
        private bool _isDisposed = false;

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
        /// Ensure required content is availalble.
        /// </summary>
        public void EnsureRequiredContentsAvailable()
        {
            GuardNotDisposed();

            var contents = _requiredDataRepository.GetCriticalContent();
            _internalContentRepository.EnsureContentListAvailable(contents);
        }

        /// <summary>
        /// Ensure first user exists.
        /// </summary>
        public void EnsureInitialUserAvaialble()
        {
            GuardNotDisposed();

            var user = _requiredDataRepository.GetDefaultUserData();
            _applicationUserRepository.EnsureUserAvailable(user);
        }

        /// <summary>
        /// Remove initial user.
        /// </summary>
        public void ClearInitialUser()
        {
            GuardNotDisposed();

            var user = _requiredDataRepository.GetDefaultUserData();
            _applicationUserRepository.DeleteUserByEMail(user.EMail);
        }

        /// <summary>
        /// Remove all required contnet.
        /// </summary>
        public void ClearRequiredContents()
        {
            GuardNotDisposed();

            var requiredContentsNames = _requiredDataRepository.GetCriticalContent().Select(x => x.InternalCaption).ToList();
            _internalContentRepository.DeleteContentsByInternalCaptions(requiredContentsNames);
        }

        /// <summary>
        /// Dispose the object.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _internalContentRepository.Dispose();
                _applicationUserRepository.Dispose();
                _isDisposed = true;
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~DataInitializer() {
            Dispose();
        }

        /// <summary>
        /// Throw if <see cref="DataInitializer"/> is disposed.
        /// </summary>
        private void GuardNotDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(ContentEditorRepository));
            }
        }
    }
}
