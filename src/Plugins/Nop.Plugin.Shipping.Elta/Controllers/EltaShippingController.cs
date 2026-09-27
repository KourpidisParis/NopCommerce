using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Shipping.Elta.Models;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Shipping.Elta.Controllers;

[AuthorizeAdmin]
[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
public class EltaShippingController : BasePluginController
{
    #region Fields
    private readonly EltaSettings _eltaSettings;
    #endregion


    #region Ctor
    public EltaShippingController(EltaSettings eltaSettings)
    {
        _eltaSettings = eltaSettings;
    }
    #endregion

    [CheckPermission(StandardPermission.Configuration.MANAGE_SHIPPING_SETTINGS)]
    public async Task<IActionResult> Configure()
    {
        //prepare common model

        var model = new EltaShippingModel()
        {
            Status  = _eltaSettings.Status,
            Cost    = _eltaSettings.Cost,
            Title   = _eltaSettings.Title
        };

        return View("~/Plugins/Shipping.Elta/Views/Configure.cshtml", model);
    }
}
