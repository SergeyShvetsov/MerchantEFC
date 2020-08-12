#pragma checksum "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "102e464745b3643c39f14d93f8a7e2b5c7c3ad64"
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"102e464745b3643c39f14d93f8a7e2b5c7c3ad64", @"/Areas/Admin/Views/Products/List.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"161b0084e5a9b06127b999358baa060333d73446", @"/Areas/Admin/Views/_ViewImports.cshtml")]
    public class Areas_Admin_Views_Products_List : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<WebUI.Areas.Admin.Models.ProductListVM>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/Images/no_image.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("card-img"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
            WriteLiteral("    <div class=\"alert alert-success\">\r\n        ");
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
            WriteLiteral("                <tr>\r\n                    <td>\r\n                        <div class=\"card mb-3\" style=\"max-width: 540px;\">\r\n                            <div class=\"row no-gutters\">\r\n                                <div class=\"col-md-4\">\r\n");
#nullable restore
#line 44 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
                                     if (item.Image == null)
                                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "102e464745b3643c39f14d93f8a7e2b5c7c3ad647857", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 47 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
                                    }
                                    else
                                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                        <img");
            BeginWriteAttribute("src", " src=\"", 1498, "\"", 1564, 2);
            WriteAttributeValue("", 1504, "data:image;base64,", 1504, 18, true);
#nullable restore
#line 50 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
WriteAttributeValue("", 1522, System.Convert.ToBase64String(item.Image), 1522, 42, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"card-img\">\r\n");
#nullable restore
#line 51 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
                                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                                </div>\r\n                                <div class=\"col-md-8\">\r\n                                    <div class=\"card-body\">\r\n                                        <h5 class=\"card-title\">");
#nullable restore
#line 55 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
                                                          Write(item.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h5>\r\n                                        <p class=\"card-text\">");
#nullable restore
#line 56 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
                                                        Write(item.Price);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                                    </div>\r\n                                </div>\r\n                            </div>\r\n                        </div>\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 63 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
                   Write(Html.ActionLink(resources["Edit"], "EditProduct", "Products", new { id = item.ProductId }, htmlAttributes: new { @class = "btn btn-outline-primary" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        <button type=\"button\" class=\"btn btn-outline-danger\" data-toggle=\"ajax-modal\"\r\n                                data-target=\"#deleteProduct\" data-url=\"");
#nullable restore
#line 65 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
                                                                  Write(Url.Action($"DeleteProduct/{item.ProductId}"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\">\r\n                            ");
#nullable restore
#line 66 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
                       Write(resources["Delete"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </button>\r\n                    </td>\r\n                </tr>\r\n");
#nullable restore
#line 70 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </tbody>\r\n    </table>\r\n");
#nullable restore
#line 73 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
Write(Html.PagedListPager((IPagedList)Model, page => Url.Action("List", null), Bootstrap4PagedListRenderOptions.Default));

#line default
#line hidden
#nullable disable
#nullable restore
#line 73 "D:\Work\MerchantEFC\WebUI\Areas\Admin\Views\Products\List.cshtml"
                                                                                                                       
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