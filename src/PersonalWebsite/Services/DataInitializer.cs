using PersonalWebsite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace PersonalWebsite.Services
{
    public class DataInitializer : IDisposable
    {
        private IServiceProvider _serviceProvider;
        private IRequiredDataRepository _requiredDataRepository;
        private IInternalContentRepository _internalContentRepository;

        public DataInitializer(IServiceProvider serviceProvider)
        {
            if(serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            _serviceProvider = serviceProvider;
            _requiredDataRepository = serviceProvider.GetService<IRequiredDataRepository>();

            _internalContentRepository = _serviceProvider.GetService<IInternalContentRepository>();
        }

        public void EnsureRequiredContentsAvailable()
        {
            var contents = _requiredDataRepository.GetCriticalContent();
            _internalContentRepository.EnsureContentsRangeAvailable(contents);
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
                _disposed = true;
            }

            GC.SuppressFinalize(this);
        }

        ~DataInitializer() {
            Dispose();
        }
    }
}
