## _How do i run this?_

Add a `secrets.config` file to project root (alongside Web.config) and hit F5 in Visual Studio.

```xml
<appSettings>
  <add key="ida:ClientId" value="xxxxxxxx-xxxx-xxxx-xxxxx-xxxxxxxxxxxx" />
  <add key="ida:Tenant" value="TENANT.onmicrosoft.com" />
  <add key="ida:Audience" value="https://TENANT.onmicrosoft.com/THIS-WEBAPI-NAME" />
  <add key="ida:Password" value="thEappSeCreT=" />
</appSettings>
```

## ADAL.NET tracing (quite useful especially for debugging token cache issues)

The `/api/groups` controller is instrumented with ADAL.NET tracing:

```
*** ADAL DEBUG *** 2018-01-31T14:19:36.5347373Z: 13f38a85-8aae-4144-b44d-d513b32f2eb0 - AcquireTokenHandlerBase.cs: ADAL PCL.Desktop with assembly version '3.17.3.35304', file version '3.17.41219.2324' and informational version 'b6afaeae7cff965e66649e0ee7e8c29071d5a7e6' is running...
*** ADAL DEBUG *** 2018-01-31T14:19:36.5367376Z: 13f38a85-8aae-4144-b44d-d513b32f2eb0 - AcquireTokenHandlerBase.cs: === Token Acquisition started:
	Authority: https://login.microsoftonline.com/swearjarbank.onmicrosoft.com/
	Resource: https://graph.microsoft.com
	ClientId: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
	CacheType: null
	Authentication Target: Client
	
*** ADAL DEBUG *** 2018-01-31T14:19:36.7287701Z: 13f38a85-8aae-4144-b44d-d513b32f2eb0 - AcquireTokenHandlerBase.cs: Loading from cache.
*** ADAL DEBUG *** 2018-01-31T14:19:36.7337721Z: 13f38a85-8aae-4144-b44d-d513b32f2eb0 - TokenCache.cs: Looking up cache for a token...
*** ADAL DEBUG *** 2018-01-31T14:19:36.7417477Z: 13f38a85-8aae-4144-b44d-d513b32f2eb0 - TokenCache.cs: No matching token was found in the cache
*** ADAL DEBUG *** 2018-01-31T14:19:37.0767932Z: 13f38a85-8aae-4144-b44d-d513b32f2eb0 - TokenCache.cs: Storing token in the cache...
*** ADAL DEBUG *** 2018-01-31T14:19:37.0787674Z: 13f38a85-8aae-4144-b44d-d513b32f2eb0 - TokenCache.cs: An item was stored in the cache
*** ADAL DEBUG *** 2018-01-31T14:19:37.0827921Z: 13f38a85-8aae-4144-b44d-d513b32f2eb0 - AcquireTokenHandlerBase.cs: === Token Acquisition finished successfully. An access token was retuned:
	Access Token Hash: fwF0VSJPEH4nXqDrMWIXqtEzDd4K1M60m5RQn/uTK8E=
	Expiration Time: 1/31/2018 3:19:36 PM +00:00
	User Hash: null

[...]

 *** ADAL DEBUG *** 2018-01-31T14:22:15.0195926Z: 96245e57-4737-41b5-a0a9-c5457be1e13e - TokenCache.cs: Looking up cache for a token...
 *** ADAL DEBUG *** 2018-01-31T14:22:15.0215926Z: 96245e57-4737-41b5-a0a9-c5457be1e13e - TokenCache.cs: An item matching the requested resource was found in the cache
 *** ADAL DEBUG *** 2018-01-31T14:22:15.0246369Z: 96245e57-4737-41b5-a0a9-c5457be1e13e - TokenCache.cs: 57.35080272 minutes left until token in cache expires
 *** ADAL DEBUG *** 2018-01-31T14:22:15.0255933Z: 96245e57-4737-41b5-a0a9-c5457be1e13e - TokenCache.cs: A matching item (access token or refresh token or both) was found in the cache
 *** ADAL DEBUG *** 2018-01-31T14:22:15.0265926Z: 96245e57-4737-41b5-a0a9-c5457be1e13e - AcquireTokenHandlerBase.cs: === Token Acquisition finished successfully. An access token was retuned:
	Access Token Hash: fwF0VSJPEH4nXqDrMWIXqtEzDd4K1M60m5RQn/uTK8E=
	Expiration Time: 1/31/2018 3:19:36 PM +00:00
	User Hash: 

```

## To implement ADAL.NET tracing in your API, follow these steps

If you don't have this already, enable tracing in your Web API first (inside `Register(HttpConfiguration config)`, usually called in `WebApiConfig.cs`):

```csharp
config.EnableSystemDiagnosticsTracing();
```

To enable ADAL tracing, simply implement [this public `Log()` method](https://github.com/AzureAD/azure-activedirectory-library-for-dotnet/blob/dev/adal/src/Microsoft.IdentityModel.Clients.ActiveDirectory/IAdalLogCallback.cs#L77-L82):

```csharp
// Startup.cs
// There may be a better place for it, but Startup works just fine.

public class AdalLoggerCallback : IAdalLogCallback
{
    public void Log(LogLevel level, string message)
    {
        Trace.TraceInformation($"*** ADAL DEBUG *** {message}");
    }
}
```

then call `LoggerCallbackHandler.Callback = new Startup.AdalLoggerCallback();` from a controller that calls into the ADAL library.


### NOTE

There's no tracing for the Bearer token middleware (`app.UseWindowsAzureActiveDirectoryBearerAuthentication()`), check out subchapter _[Exposing a protected Web API](https://www.microsoftpressstore.com/store/modern-authentication-with-azure-active-directory-for-9780735696945)_ (page 232) to learn how to add notifications to your token middleware.
