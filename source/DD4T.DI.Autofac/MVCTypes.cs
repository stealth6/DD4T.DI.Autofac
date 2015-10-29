using Autofac;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DD4T.DI.Autofac
{
    public static class MVCTypes
    {
        public static void RegisterMVCTypes(this ContainerBuilder builder)
        {
            var location = string.Format(@"{0}\bin\", AppDomain.CurrentDomain.BaseDirectory);
            var file = Directory.GetFiles(location, "DD4T.MVC.dll").FirstOrDefault();
            if (file == null)
                return;

            var load = Assembly.LoadFile(file);
            var provider = AppDomain.CurrentDomain.GetAssemblies().Where(ass => ass.FullName.StartsWith("DD4T.MVC")).FirstOrDefault();
            if (provider == null)
                return;

            var providerTypes = provider.GetTypes();

            var iComponentPresentationRenderer = providerTypes.Where(a => a.FullName.Equals("DD4T.Mvc.Html.IComponentPresentationRenderer")).FirstOrDefault();
            var defaultComponentPresentationRenderer = providerTypes.Where(a => a.FullName.Equals("DD4T.Mvc.Html.DefaultComponentPresentationRenderer")).FirstOrDefault();

            if (iComponentPresentationRenderer == null || defaultComponentPresentationRenderer == null)
                return;

            builder.RegisterType(defaultComponentPresentationRenderer).As(new[] { iComponentPresentationRenderer }).PreserveExistingDefaults();
        }
    }
}
