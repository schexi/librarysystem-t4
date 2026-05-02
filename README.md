Branchen Deploy-MVC är huvud-applikationen som fungerar, det är den som är driftsatt i azure via:

https://t4bibliotek.azurewebsites.net/

Dock har innehållet från Deploy-MVC kopierats över till main för tydlighet. Huvudapplikationen i Azure är dock integrerad CI/CD med Deploy-MVC sen start och behålls därför så.

*Versionen som är driftsatt i Azure är den senaste stabila versionen av systemet som vi har, då vissa APIer innehöll problem eller inte kunde driftsättas förrän kritiskt nära deadline, vilket resulterade i att gruppen behöll det stabila systemet som var preparerat för integration för att undvika last minute kraschar.*


 Edins del av projektet, Notifications API, finns implementerad i branchen edin-notifications.

Gruppen valde att inte integrera denna del i huvudbranchen (main) i ett sent skede av projektet för att undvika att introducera potentiella fel i den gemensamma slutversionen samt att Azure vart nere en längre period.

För att möjliggöra en korrekt bedömning av min implementation har funktionaliteten därför lagts i en separat branch där den kan köras och testas självständigt.

Adishkas del av projektet, User-Api, finns implementerad i mappen User-API i main-branchen.
API:et är driftsatt på Azure via: https://user-api-adde.azurewebsites.net
User-API är nu integrerat i systemet via appsettings.json.

admin kod (HV-Admin-2026)


UPPDATERAD README 24/4-2026

Notifications API är nu integrerat i systemet.

Notifikationer hämtas i NotificationsController och visas på /Notifications i webbappen.

---

Körs genom detta command i main branchen:
dotnet run --project ./LibrarySystem-T4/LibrarySystem-T4.csproj

