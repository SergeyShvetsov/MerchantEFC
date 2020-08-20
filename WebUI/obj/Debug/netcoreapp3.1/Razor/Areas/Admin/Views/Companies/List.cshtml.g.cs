#pragma checksum "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ccd02a6f4b2c4e73d6eea9a4dbc39bdbed3db696"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Admin_Views_Companies_List), @"mvc.1.0.view", @"/Areas/Admin/Views/Companies/List.cshtml")]
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
#line 1 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
using X.PagedList.Mvc.Core;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
using X.PagedList;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
using X.PagedList.Mvc.Bootstrap4.Core;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
using WebUI.Resources;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ccd02a6f4b2c4e73d6eea9a4dbc39bdbed3db696", @"/Areas/Admin/Views/Companies/List.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"161b0084e5a9b06127b999358baa060333d73446", @"/Areas/Admin/Views/_ViewImports.cshtml")]
    public class Areas_Admin_Views_Companies_List : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<WebUI.Areas.Admin.Models.CompanyListVM>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 8 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
  
    ViewData["Title"] = "CompanyList";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 12 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
 if (TempData["SM"] != null)
{


#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"alert alert-success\">\r\n        ");
#nullable restore
#line 16 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
   Write(TempData["SM"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </div>\r\n");
#nullable restore
#line 18 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"row\">\r\n    <div class=\"col\">\r\n        <h4>");
#nullable restore
#line 22 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
       Write(resources["CompanyList"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n    </div>\r\n    <div class=\"col\" style=\"text-align: right;\">\r\n        <button type=\"button\" class=\"btn btn-outline-primary\" data-toggle=\"ajax-modal\"\r\n                data-target=\"#createCompany\" data-url=\"");
#nullable restore
#line 26 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
                                                  Write(Url.Action($"CreateCompany"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\">\r\n            ");
#nullable restore
#line 27 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
       Write(resources["CreateCompany"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </button>\r\n    </div>\r\n</div>\r\n\r\n");
#nullable restore
#line 32 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
 if (!Model.Any())
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h2 class=\"text-center\">");
#nullable restore
#line 34 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
                       Write(resources["CompanyListIsEmpty"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n");
#nullable restore
#line 35 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
}
else
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <table class=\"table\">\r\n        <thead>\r\n            <tr>\r\n                <th>\r\n                    ");
#nullable restore
#line 42 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
               Write(Html.DisplayNameFor(model => model.Code));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </th>\r\n                <th>\r\n                    ");
#nullable restore
#line 45 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
               Write(Html.DisplayNameFor(model => model.Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </th>\r\n                <th>\r\n                    ");
#nullable restore
#line 48 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
               Write(Html.DisplayNameFor(model => model.IsActive));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </th>\r\n                <th>\r\n                    ");
#nullable restore
#line 51 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
               Write(Html.DisplayNameFor(model => model.IsBlocked));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </th>\r\n                <th></th>\r\n            </tr>\r\n        </thead>\r\n        <tbody>\r\n");
#nullable restore
#line 57 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
             foreach (var item in Model)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr>\r\n                    <td>\r\n                        <input type=\"hidden\"");
            BeginWriteAttribute("value", " value=\"", 1621, "\"", 1637, 1);
#nullable restore
#line 61 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
WriteAttributeValue("", 1629, item.Id, 1629, 8, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n                        ");
#nullable restore
#line 62 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
                   Write(Html.DisplayFor(modelItem => item.Code));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 65 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
                   Write(Html.DisplayFor(modelItem => item.Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 68 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
                   Write(Html.DisplayFor(modelItem => item.IsActive));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 71 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
                   Write(Html.DisplayFor(modelItem => item.IsBlocked));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        <button type=\"button\" class=\"btn btn-outline-primary\" data-toggle=\"ajax-modal\"\r\n                                data-target=\"#editCompany\" data-url=\"");
#nullable restore
#line 75 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
                                                                Write(Url.Action($"EditCompany/{item.Id}"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\">\r\n                            ");
#nullable restore
#line 76 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
                       Write(resources["Edit"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </button>\r\n                        <button type=\"button\" class=\"btn btn-outline-danger\" data-toggle=\"ajax-modal\"\r\n                                data-target=\"#deleteCompany\" data-url=\"");
#nullable restore
#line 79 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
                                                                  Write(Url.Action($"DeleteCompany/{item.Id}"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\">\r\n                            ");
#nullable restore
#line 80 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
                       Write(resources["Delete"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </button>\r\n                    </td>\r\n                </tr>\r\n");
#nullable restore
#line 84 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </tbody>\r\n    </table>\r\n");
#nullable restore
#line 87 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
Write(Html.PagedListPager((IPagedList)Model, page => Url.Action("List", new { page }), Bootstrap4PagedListRenderOptions.Default));

#line default
#line hidden
#nullable disable
#nullable restore
#line 87 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Companies\List.cshtml"
                                                                                                                               
}

#line default
#line hidden
#nullable disable
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<WebUI.Areas.Admin.Models.CompanyListVM>> Html { get; private set; }
    }
}
#pragma warning restore 1591
