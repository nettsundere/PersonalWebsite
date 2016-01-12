using System;

namespace PersonalWebsite.Services
{
    public class PrivateDefaultsService : IPrivateDefaultsService
    {
        private readonly ILanguageManipulationService _languageManipulationService;

        private readonly IPageConfiguration _pageConfiguration;

        /// <summary>
        /// Provides default language-related settings for the private area of a website.
        /// </summary>
        /// <param name="pageConfiguration"><typeparamref name="IPageConfiguration"/> configuration.</param>
        /// <param name="languageManipulationService"><typeparamref name="ILanguageManipulationService"/></param>
        public PrivateDefaultsService(IPageConfiguration pageConfiguration, ILanguageManipulationService languageManipulationService)
        {
            if (pageConfiguration == null)
            {
                throw new ArgumentNullException(nameof(pageConfiguration));
            }

            if (languageManipulationService == null)
            {
                throw new ArgumentNullException(nameof(languageManipulationService));
            }

            _pageConfiguration = pageConfiguration;
            _languageManipulationService = languageManipulationService;

        }    
        
        public void Setup()
        {
            _languageManipulationService.SetLanguage(_pageConfiguration.DefaultLanguage);
        }
    }
}
