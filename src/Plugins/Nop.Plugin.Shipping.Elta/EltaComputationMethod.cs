
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
    private readonly ISettingService _settingService;
    private readonly ILocalizationService _localizationService;

    #endregion
    public EltaComputationMethod(IWebHelper webHelper,
        ISettingService settingService,
        ILocalizationService localizationService)
    {
        _webHelper = webHelper;
        _settingService = settingService;
        _localizationService = localizationService;
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

    public Task<GetShippingOptionResponse> GetShippingOptionsAsync(GetShippingOptionRequest getShippingOptionRequest)
    {
        var response = new GetShippingOptionResponse();
        response.ShippingOptions.Add(new ShippingOption
        {
            Name = "Elta",
            Rate = 4,
            Description = "Elta shipping method",
            TransitDays = 5
        });

        return Task.FromResult(response);
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
