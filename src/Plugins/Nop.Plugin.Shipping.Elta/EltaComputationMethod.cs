
using Nop.Core;
using Nop.Core.Domain.Shipping;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Shipping;
using Nop.Services.Shipping.Tracking;

namespace Nop.Plugin.Shipping.Elta;


public class EltaComputationMethod : BasePlugin, IShippingRateComputationMethod
{
    #region Fields

    private readonly IWebHelper _webHelper;
    private readonly IWorkContext _workContext;
    private readonly ISettingService _settingService;
    private readonly ILocalizationService _localizationService;
    private readonly EltaSettings _eltaSettings;

    #endregion
    public EltaComputationMethod(IWebHelper webHelper,
        IWorkContext workContext,
        ISettingService settingService,
        ILocalizationService localizationService,
        EltaSettings eltaSettings)
    {
        _webHelper = webHelper;
        _workContext = workContext;
        _settingService = settingService;
        _localizationService = localizationService;
        _eltaSettings = eltaSettings;
    }
    #region Ctor

    #endregion

    public Task<decimal?> GetFixedRateAsync(GetShippingOptionRequest getShippingOptionRequest)
    {
        return Task.FromResult<decimal?>(null);
    }

    public Task<IShipmentTracker> GetShipmentTrackerAsync()
    {
        return Task.FromResult<IShipmentTracker>(null);
    }

    public async Task<GetShippingOptionResponse> GetShippingOptionsAsync(GetShippingOptionRequest getShippingOptionRequest)
    {
        //title in the customer's language (falls back to the standard value)
        var language = await _workContext.GetWorkingLanguageAsync();
        var title = await _localizationService.GetLocalizedSettingAsync(_eltaSettings, x => x.Title, language.Id, 0);

        var response = new GetShippingOptionResponse();
        response.ShippingOptions.Add(new ShippingOption
        {
            Name = title,
            Rate = _eltaSettings.Cost,
            Description = title,
            TransitDays = 5
        });

        return response;
    }

    public override string GetConfigurationPageUrl()
    {
        return $"{_webHelper.GetStoreLocation()}Admin/EltaShipping/Configure";
    }

    /// <summary>
    /// Install plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task InstallAsync()
    {
        //settings
        await _settingService.SaveSettingAsync(new EltaSettings
        {
            Status = true,
            Cost   = 4,
            Title   = ""
        });

        //locales
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Plugins.Shipping.Elta.Fields.Title"] = "Text",
            ["Plugins.Shipping.Elta.Fields.Title.Hint"] = "Type the text you want to show on checkout",
            ["Plugins.Shipping.Elta.Fields.Status"] = "Status",
            ["Plugins.Shipping.Elta.Fields.Cost"]   = "Cost",
        });

        await base.InstallAsync();
    }

    /// <summary>
    /// Uninstall plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task UninstallAsync()
    {
        //settings
        await _settingService.DeleteSettingAsync<EltaSettings>();

        //locales
        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Shipping.Elta");
    }

}
