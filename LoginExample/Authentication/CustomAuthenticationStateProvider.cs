using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using LoginExample.Data;
using LoginExample.Data.Impl;
using LoginExample.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace LoginExample.Authentication {
public class CustomAuthenticationStateProvider : AuthenticationStateProvider {
    private readonly IJSRuntime jsRuntime;
    private readonly IUserService _userService;

    private User cachedUser;

    public CustomAuthenticationStateProvider(IJSRuntime jsRuntime, IUserService userService) {
        this.jsRuntime = jsRuntime;
        this._userService = userService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
        var identity = new ClaimsIdentity();
        if (cachedUser == null) {
            string userAsJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
            if (!string.IsNullOrEmpty(userAsJson)) {
                User tmp = JsonSerializer.Deserialize<User>(userAsJson);
                await ValidateLogin(tmp.UserName, tmp.Password);
            }
        } else {
            identity = SetupClaimsForUser(cachedUser);
        }

        ClaimsPrincipal cachedClaimsPrincipal = new ClaimsPrincipal(identity);
        return await Task.FromResult(new AuthenticationState(cachedClaimsPrincipal));
    }

    public async Task ValidateLogin(string username, string password) {
        if (string.IsNullOrEmpty(username)) throw new Exception("Enter username");
        if (string.IsNullOrEmpty(password)) throw new Exception("Enter password");

        ClaimsIdentity identity = new ClaimsIdentity();
        try {
            User user = await _userService.ValidateUserAsync(username, password);
            identity = SetupClaimsForUser(user);
            string serialisedData = JsonSerializer.Serialize(user);
            await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serialisedData);
            cachedUser = user;
        } catch (Exception e)
        {
            throw new Exception("Wrong credentials");
        }

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
    }

    public void Logout() {
        cachedUser = null;
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    private ClaimsIdentity SetupClaimsForUser(User user) {
        List<Claim> claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
        claims.Add(new Claim("Role", user.Role));
        claims.Add(new Claim("City", user.City));
        claims.Add(new Claim("Domain", user.Domain));
        claims.Add(new Claim("BirthYear", user.BirthYear.ToString()));
        claims.Add(new Claim("Level", user.SecurityLevel.ToString()));

        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth_type");
        return identity;
    }
}
}