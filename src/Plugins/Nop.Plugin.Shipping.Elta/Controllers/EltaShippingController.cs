using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Shipping.Elta.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Shipping.Elta.Controllers;

[AuthorizeAdmin]
[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
public class EltaShippingController : BasePluginController
{
    #region Fields
    private readonly EltaSettings _eltaSettings;
    private readonly ILanguageService _languageService;
    private readonly INotificationService _notificationService;
    private readonly ILocalizationService _localizationService;
    private readonly ISettingService _settingService;
    #endregion


    #region Ctor
    public EltaShippingController(EltaSettings eltaSettings,
        ILanguageService languageService,
        INotificationService notificationService,
        ILocalizationService localizationService,
        ISettingService settingService)
    {
        _eltaSettings = eltaSettings;
        _languageService = languageService;
        _notificationService = notificationService;
        _localizationService = localizationService;
        _settingService = settingService;
    }
    #endregion

    [CheckPermission(StandardPermission.Configuration.MANAGE_SHIPPING_SETTINGS)]
    public async Task<IActionResult> Configure()
    {
        //prepare common model

        var model = new EltaShippingModel()
        {
            Status = _eltaSettings.Status,
            Cost = _eltaSettings.Cost,
            Title = _eltaSettings.Title
        };

        //locales
        await AddLocalesAsync(_languageService, model.Locales, async (locale, languageId) =>
        {
            locale.Title = await _localizationService
                .GetLocalizedSettingAsync(_eltaSettings, x => x.Title, languageId, 0, false, false);
        });

        return View("~/Plugins/Shipping.Elta/Views/Configure.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_SHIPPING_SETTINGS)]
    public async Task<IActionResult> Configure (EltaShippingModel model)
    {
        if (!ModelState.IsValid)
            return await Configure();

        //save settings
        _eltaSettings.Status = model.Status;
        _eltaSettings.Cost = model.Cost;
        _eltaSettings.Title = model.Title;

        await _settingService.SaveSettingAsync(_eltaSettings);

        //localization
        foreach (var localized in model.Locales)
        {
            await _localizationService.SaveLocalizedSettingAsync(_eltaSettings,
                x => x.Title, localized.LanguageId, localized.Title);
        }

        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));
        return await Configure();
    }
}
