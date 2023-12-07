# Smartcache
**Login**
```sh
POST http://localhost/login
Content-Type: application/json
{
    "Name": "User",
    "Password": "123"
}
```
**AddEmail**
```sh
POST http://localhost/email/test@email.com
Authorization: Bearer xxx
```
**GetEmail**
```sh
GET http://localhost/email/test@email.com
Authorization: Bearer xxx
```

**Test skaliranja**<br/>
<img src="https://github.com/otiv33/nm_smartcache/blob/main/scaling.png">

**Primerjava smart cache in tradicionalni API**<br/>
<img src="https://github.com/otiv33/nm_smartcache/blob/main/compare.png">

**Na spletu je tudi objavljen SmartCache (brez avtentikacije)**<br/>
<a href="https://smartcacheapi-app-20231206183237.wittyocean-6dc4acf6.germanywestcentral.azurecontainerapps.io">https://smartcacheapi-app-20231206183237.wittyocean-6dc4acf6.germanywestcentral.azurecontainerapps.io</a>
