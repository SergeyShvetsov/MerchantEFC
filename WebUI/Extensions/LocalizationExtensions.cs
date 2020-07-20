using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using WebUI.Resources;

namespace WebUI.Extensions
{
    public static class LocalizationExtensions
    {
        public static IStringLocalizer GetLocalResources(this IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            return factory.Create("SharedResource", assemblyName.Name); 
        }
    }
}

