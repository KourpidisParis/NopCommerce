
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Shipping.Elta.Models;
public record EltaShippingModel : BaseNopModel
{
    #region Properties

    [NopResourceDisplayName("Plugins.Shipping.Elta.Fields.Status")]
    public bool Status { get; set; }

    [NopResourceDisplayName("Plugins.Shipping.Elta.Fields.Cost")]
    public decimal Cost { get; set; }

    [NopResourceDisplayName("Plugins.Shipping.Elta.Fields.Title")]
    public string Title { get; set; }


    #endregion
}
