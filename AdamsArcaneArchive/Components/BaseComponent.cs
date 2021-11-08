using AdamsArcaneArchive.Data;
using AdamsArcaneArchive.Data.Models;
using AdamsArcaneArchive.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdamsArcaneArchive.Components
{
    public class CISBaseComponent : ComponentBase, IDisposable
    {
        [CascadingParameter]
        public Error Error { get; set; }

        [Inject]
        NavigationManager _navigationManager { get; set; }

        [Inject]
        IConfiguration _configuration { get; set; }

        [Inject]
        protected IDbContextFactory<AppDbContext> DbContextFactory { get; set; }

        [Inject]
        public IJSRuntime JsInterop { get; set; }

        protected bool Busy { get; set; }
        protected bool Loading { get; set; }
        protected AppDbContext Context { get; set; }
        protected NavigationManager NavigationManager { get; set; }
        protected IConfiguration Configuration { get; set; }

        protected virtual async Task InitDependencies()
        {
            Context = DbContextFactory.CreateDbContext();
            NavigationManager = _navigationManager;
            Configuration = _configuration;
        }

        public virtual void Dispose()
        {
            Context?.Dispose();
        }
    }

    public class CISAuthorizedComponent : CISBaseComponent
    {
        [Inject]
        NavigationManager _navigationManager { get; set; }

        [Inject]
        IConfiguration _configuration { get; set; }

        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        protected override async Task InitDependencies()
        {
            Context = DbContextFactory.CreateDbContext();
            NavigationManager = _navigationManager;
            Configuration = _configuration;

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var isAdmin = authState.User.IsInRole(AppRole.Administrator);

            if (!isAdmin)
            {
                NavigationManager.NavigateTo("/Identity/Account/Login");
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            return;
        }
    }
}
