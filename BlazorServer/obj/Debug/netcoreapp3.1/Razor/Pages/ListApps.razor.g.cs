#pragma checksum "C:\Users\papa\Documents\TimeControl\BlazorServer\Pages\ListApps.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "059c54954e4a5cbbcc1de522afecce6a05559142"
// <auto-generated/>
#pragma warning disable 1591
namespace BlazorServer.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\papa\Documents\TimeControl\BlazorServer\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\papa\Documents\TimeControl\BlazorServer\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\papa\Documents\TimeControl\BlazorServer\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\papa\Documents\TimeControl\BlazorServer\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\papa\Documents\TimeControl\BlazorServer\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\papa\Documents\TimeControl\BlazorServer\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\papa\Documents\TimeControl\BlazorServer\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\papa\Documents\TimeControl\BlazorServer\_Imports.razor"
using BlazorServer;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\papa\Documents\TimeControl\BlazorServer\_Imports.razor"
using BlazorServer.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\papa\Documents\TimeControl\BlazorServer\Pages\ListApps.razor"
using BlazorServer.Data;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/ListApps")]
    public partial class ListApps : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<h1>Connect</h1>\r\n\r\n");
            __builder.OpenElement(1, "button");
            __builder.AddAttribute(2, "class", "btn btn-primary");
            __builder.AddAttribute(3, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 7 "C:\Users\papa\Documents\TimeControl\BlazorServer\Pages\ListApps.razor"
                                          ListAllApps

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(4, "Show apps");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 9 "C:\Users\papa\Documents\TimeControl\BlazorServer\Pages\ListApps.razor"
       
    private int currentCount = 0;
    
    private void ListAllApps()
    {
        socClient.Connect("DESKTOP-3T3SQ7S");
        
        List<Common.AppsPersist> lap=socClient.SendMessage(null,null,0,(int)Common.Command.list);
        
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private SocClient socClient { get; set; }
    }
}
#pragma warning restore 1591
