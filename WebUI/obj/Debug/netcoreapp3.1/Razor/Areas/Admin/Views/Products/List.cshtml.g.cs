#pragma checksum "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0b77d40c768c260569a1d1c37b8635124e3a4150"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Admin_Views_Products_List), @"mvc.1.0.view", @"/Areas/Admin/Views/Products/List.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\_ViewImports.cshtml"
using WebUI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\_ViewImports.cshtml"
using WebUI.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\_ViewImports.cshtml"
using Data.Model;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
using X.PagedList.Mvc.Core;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
using X.PagedList;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
using X.PagedList.Mvc.Bootstrap4.Core;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
using WebUI.Resources;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0b77d40c768c260569a1d1c37b8635124e3a4150", @"/Areas/Admin/Views/Products/List.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"161b0084e5a9b06127b999358baa060333d73446", @"/Areas/Admin/Views/_ViewImports.cshtml")]
    public class Areas_Admin_Views_Products_List : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<WebUI.Areas.Admin.Models.ProductListVM>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 8 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
  
    ViewData["Title"] = "ProductList";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 12 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
 if (TempData["SM"] != null)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"alert alert-success\">\r\n\r\n        ");
#nullable restore
#line 16 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
   Write(TempData["SM"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </div>\r\n");
#nullable restore
#line 18 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"row\">\r\n    <div class=\"col\">\r\n        <h4>");
#nullable restore
#line 22 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
       Write(resources["ProductList"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n    </div>\r\n    <div class=\"col\" style=\"text-align: right;\">\r\n        ");
#nullable restore
#line 25 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
   Write(Html.ActionLink(resources["CreateProduct"], "CreateProduct", "Products", null, htmlAttributes: new { @class = "btn btn-outline-primary" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </div>\r\n</div>\r\n\r\n");
#nullable restore
#line 29 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
 if (!Model.Any())
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h2 class=\"text-center\">");
#nullable restore
#line 31 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
                       Write(resources["ProductListIsEmpty"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n");
#nullable restore
#line 32 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
}
else
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <table class=\"table\">\r\n        <tbody>\r\n");
#nullable restore
#line 37 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
             foreach (var item in Model)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                <tr>
                    <td>
                        <div class=""card mb-3"" style=""max-width: 540px;"">
                            <div class=""row no-gutters"">
                                <div class=""col-md-4"">
                                    <img");
            BeginWriteAttribute("src", " src=\"", 1179, "\"", 1242, 4);
            WriteAttributeValue("", 1185, "/Shop/ProductImage?id=", 1185, 22, true);
#nullable restore
#line 44 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
WriteAttributeValue("", 1207, item.ProductId, 1207, 15, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1222, "&s=", 1222, 3, true);
#nullable restore
#line 44 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
WriteAttributeValue("", 1225, ImageSize.Medium, 1225, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n                                </div>\r\n                                <div class=\"col-md-8\">\r\n                                    <div class=\"card-body\">\r\n                                        <h5 class=\"card-title\">");
#nullable restore
#line 48 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
                                                          Write(item.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h5>\r\n                                        <p class=\"card-text\">");
#nullable restore
#line 49 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
                                                        Write(item.Price);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                                    </div>\r\n                                </div>\r\n                            </div>\r\n                        </div>\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 56 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
                   Write(Html.ActionLink(resources["Edit"], "EditProduct", "Products", new { id = item.ProductId }, htmlAttributes: new { @class = "btn btn-outline-primary" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        <button type=\"button\" class=\"btn btn-outline-danger\" data-toggle=\"ajax-modal\"\r\n                                data-target=\"#deleteProduct\" data-url=\"");
#nullable restore
#line 58 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
                                                                  Write(Url.Action($"DeleteProduct/{item.ProductId}"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\">\r\n                            ");
#nullable restore
#line 59 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
                       Write(resources["Delete"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </button>\r\n                    </td>\r\n                </tr>\r\n");
#nullable restore
#line 63 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </tbody>\r\n    </table>\r\n");
#nullable restore
#line 66 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
Write(Html.PagedListPager((IPagedList)Model, page => Url.Action("List", new { page }), Bootstrap4PagedListRenderOptions.Default));

#line default
#line hidden
#nullable disable
#nullable restore
#line 66 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
                                                                                                                               
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public LocalizationService resources { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<WebUI.Areas.Admin.Models.ProductListVM>> Html { get; private set; }
    }
}
#pragma warning restore 1591
