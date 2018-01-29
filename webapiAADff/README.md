Add a `secrets.config` file to project root (alongside Web.config):

```xml
<appSettings>
  <add key="ida:ClientId" value="xxxxxxxx-xxxx-xxxx-xxxxx-xxxxxxxxxxxx" />
  <add key="ida:Tenant" value="TENANT.onmicrosoft.com" />
  <add key="ida:Audience" value="https://TENANT.onmicrosoft.com/THIS-WEBAPI-NAME" />
  <add key="ida:Password" value="thEappSeCreT=" />
</appSettings>
```
