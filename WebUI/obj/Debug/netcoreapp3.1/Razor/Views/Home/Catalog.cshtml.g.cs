#pragma checksum "D:\Work\MerchantEFC\WebUI\Views\Home\Catalog.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b1a9d8ea366559081832464ff7efbd97921803c2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Catalog), @"mvc.1.0.view", @"/Views/Home/Catalog.cshtml")]
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
#line 1 "D:\Work\MerchantEFC\WebUI\Views\_ViewImports.cshtml"
using WebUI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Work\MerchantEFC\WebUI\Views\_ViewImports.cshtml"
using WebUI.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Work\MerchantEFC\WebUI\Views\_ViewImports.cshtml"
using Data.Model;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b1a9d8ea366559081832464ff7efbd97921803c2", @"/Views/Home/Catalog.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"161b0084e5a9b06127b999358baa060333d73446", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Catalog : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<WebUI.Models.ProductCardVM>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "D:\Work\MerchantEFC\WebUI\Views\Home\Catalog.cshtml"
  
    ViewData["Title"] = "Catalog";

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"row mt-5 mb-2\">\r\n");
#nullable restore
#line 7 "D:\Work\MerchantEFC\WebUI\Views\Home\Catalog.cshtml"
     for (var i = 0; i < 6; i++)
    {
        

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\Work\MerchantEFC\WebUI\Views\Home\Catalog.cshtml"
         foreach (var item in Model)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <div class=\"col-6 col-lg-4 col-xl-3 col-xxl-auto product-card mt-5 mb-2\">\r\n                <a");
            BeginWriteAttribute("href", " href=\"", 317, "\"", 324, 0);
            EndWriteAttribute();
            WriteLiteral(" style=\"text-decoration: none;\">\r\n                    <img");
            BeginWriteAttribute("src", " src=\"", 383, "\"", 424, 2);
            WriteAttributeValue("", 389, "data:image;base64,", 389, 18, true);
#nullable restore
#line 13 "D:\Work\MerchantEFC\WebUI\Views\Home\Catalog.cshtml"
WriteAttributeValue("", 407, item.Base64Image, 407, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                    <p>");
#nullable restore
#line 14 "D:\Work\MerchantEFC\WebUI\Views\Home\Catalog.cshtml"
                  Write(item.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                    <p >");
#nullable restore
#line 15 "D:\Work\MerchantEFC\WebUI\Views\Home\Catalog.cshtml"
                   Write(item.PriceText);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                </a>\r\n            </div>\r\n");
#nullable restore
#line 18 "D:\Work\MerchantEFC\WebUI\Views\Home\Catalog.cshtml"
        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "D:\Work\MerchantEFC\WebUI\Views\Home\Catalog.cshtml"
         
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<WebUI.Models.ProductCardVM>> Html { get; private set; }
    }
}
#pragma warning restore 1591
