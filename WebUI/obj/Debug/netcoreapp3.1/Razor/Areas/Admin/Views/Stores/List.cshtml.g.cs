#pragma checksum "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8ec33d684b858adf6d363ddf4d06a49fb3504c65"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Admin_Views_Stores_List), @"mvc.1.0.view", @"/Areas/Admin/Views/Stores/List.cshtml")]
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
#line 1 "C:\Work\WebUI\Areas\Admin\Views\_ViewImports.cshtml"
using WebUI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Work\WebUI\Areas\Admin\Views\_ViewImports.cshtml"
using WebUI.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Work\WebUI\Areas\Admin\Views\_ViewImports.cshtml"
using Data.Model;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
using X.PagedList.Mvc.Core;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
using X.PagedList;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
using X.PagedList.Mvc.Bootstrap4.Core;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
using WebUI.Resources;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8ec33d684b858adf6d363ddf4d06a49fb3504c65", @"/Areas/Admin/Views/Stores/List.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"161b0084e5a9b06127b999358baa060333d73446", @"/Areas/Admin/Views/_ViewImports.cshtml")]
    public class Areas_Admin_Views_Stores_List : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<WebUI.Areas.Admin.Models.StoreListVM>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/ModalDialog.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 8 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
  
    ViewData["Title"] = "StoreList";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div id=\"ModalDialog\"></div>\r\n\r\n");
#nullable restore
#line 14 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
 if (TempData["SM"] != null)
{


#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"alert alert-success\">\r\n        ");
#nullable restore
#line 18 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
   Write(TempData["SM"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </div>\r\n");
#nullable restore
#line 20 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"row\">\r\n    <div class=\"col\">\r\n        <h4>");
#nullable restore
#line 24 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
       Write(resources["StoreList"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n    </div>\r\n    <div class=\"col\" style=\"text-align: right;\">\r\n        ");
#nullable restore
#line 27 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
   Write(Html.ActionLink(resources["CreateStore"], "CreateStore", "Stores", null, htmlAttributes: new { @class = "btn btn-outline-primary" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </div>\r\n</div>\r\n\r\n");
#nullable restore
#line 31 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
 if (!Model.Any())
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h2 class=\"text-center\">");
#nullable restore
#line 33 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
                       Write(resources["StoreListIsEmpty"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n");
#nullable restore
#line 34 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
}
else
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <table class=\"table\">\r\n        <thead>\r\n            <tr>\r\n                <th>\r\n                    ");
#nullable restore
#line 41 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
               Write(Html.DisplayNameFor(model => model.StoreCode));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </th>\r\n                <th>\r\n                    ");
#nullable restore
#line 44 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
               Write(Html.DisplayNameFor(model => model.StoreName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </th>\r\n                <th>\r\n                    ");
#nullable restore
#line 47 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
               Write(Html.DisplayNameFor(model => model.CityId));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </th>\r\n                <th>\r\n                    ");
#nullable restore
#line 50 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
               Write(Html.DisplayNameFor(model => model.IsActive));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </th>\r\n                <th>\r\n                    ");
#nullable restore
#line 53 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
               Write(Html.DisplayNameFor(model => model.IsBlocked));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </th>\r\n                <th></th>\r\n            </tr>\r\n        </thead>\r\n        <tbody>\r\n");
#nullable restore
#line 59 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
             foreach (var item in Model)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr>\r\n                    <td>\r\n                        ");
#nullable restore
#line 63 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
                   Write(Html.DisplayFor(modelItem => item.StoreCode));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 66 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
                   Write(Html.DisplayFor(modelItem => item.StoreName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 69 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
                   Write(Html.DisplayFor(modelItem => item.CityName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 72 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
                   Write(Html.DisplayFor(modelItem => item.IsActive));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 75 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
                   Write(Html.DisplayFor(modelItem => item.IsBlocked));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 78 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
                   Write(Html.ActionLink(resources["Edit"], "EditStore", "Stores", new { id = item.StoreId }, htmlAttributes: new { @class = "btn btn-outline-primary" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        <button type=\"button\" class=\"btn btn-outline-danger\" data-toggle=\"ajax-modal\"\r\n                                data-target=\"#deleteStore\" data-url=\"");
#nullable restore
#line 80 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
                                                                Write(Url.Action($"DeleteStore/{item.StoreId}"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\">\r\n                            ");
#nullable restore
#line 81 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
                       Write(resources["Delete"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </button>\r\n                    </td>\r\n                </tr>\r\n");
#nullable restore
#line 85 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </tbody>\r\n    </table>\r\n");
#nullable restore
#line 88 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
Write(Html.PagedListPager((IPagedList)Model, page => Url.Action("List", null), Bootstrap4PagedListRenderOptions.Default));

#line default
#line hidden
#nullable disable
#nullable restore
#line 88 "C:\Work\WebUI\Areas\Admin\Views\Stores\List.cshtml"
                                                                                                                       
}

#line default
#line hidden
#nullable disable
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8ec33d684b858adf6d363ddf4d06a49fb3504c6511988", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
            }
            );
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<WebUI.Areas.Admin.Models.StoreListVM>> Html { get; private set; }
    }
}
#pragma warning restore 1591
