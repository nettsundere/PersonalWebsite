using PersonalWebsite.Repositories;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace PersonalWebsite.Services
{
    public class DataInitializer : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IRequiredDataRepository _requiredDataRepository;
        private readonly IInternalContentRepository _internalContentRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;


        public DataInitializer(IServiceProvider serviceProvider)
        {
            if(serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            _serviceProvider = serviceProvider;
            _requiredDataRepository = serviceProvider.GetService<IRequiredDataRepository>();

            _internalContentRepository = _serviceProvider.GetService<IInternalContentRepository>();
            _applicationUserRepository = _serviceProvider.GetService<IApplicationUserRepository>();
        }

        public void EnsureRequiredContentsAvailable()
        {
            var contents = _requiredDataRepository.GetCriticalContent();
            _internalContentRepository.EnsureContentsRangeAvailable(contents);
        }

        public void EnsureInitialUserAvaialble()
        {
            var user = _requiredDataRepository.GetInitialUserData();
            _applicationUserRepository.EnsureUserAvailable(user);
        }

        public void ClearInitialUser()
        {
            var user = _requiredDataRepository.GetInitialUserData();
            _applicationUserRepository.DeleteUserByEMail(user.EMail);
        }

        public void ClearRequiredContents()
        {
            var requiredContentsNames = from x in _requiredDataRepository.GetCriticalContent()
                                        select x.InternalCaption;

            _internalContentRepository.DeleteContentsByInternalCaptions(requiredContentsNames);
        }

        private bool _disposed = false;

        public void Dispose()
        {
            if (!_disposed)
            {
                _internalContentRepository.Dispose();
                _applicationUserRepository.Dispose();
                _disposed = true;
            }

            GC.SuppressFinalize(this);
        }

        ~DataInitializer() {
            Dispose();
        }
    }
}
