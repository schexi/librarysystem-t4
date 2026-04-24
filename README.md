Branchen Deploy-MVC är huvud-applikationen som fungerar, det är den som är driftsatt i azure via:

https://t4bibliotek.azurewebsites.net/

Dock har innehållet från Deploy-MVC kopierats över till main för tydlighet. Huvudapplikationen i Azure är dock integrerad CI/CD med Deploy-MVC sen start och behålls därför så.

*Versionen som är driftsatt i Azure är den senaste stabila versionen av systemet som vi har, då vissa APIer innehöll problem eller inte kunde driftsättas förrän kritiskt nära deadline, vilket resulterade i att gruppen behöll det stabila systemet som var preparerat för integration för att undvika last minute kraschar.*


 Edins del av projektet, Notifications API, finns implementerad i branchen edin-notifications.

Gruppen valde att inte integrera denna del i huvudbranchen (main) i ett sent skede av projektet för att undvika att introducera potentiella fel i den gemensamma slutversionen samt att Azure vart nere en längre period.

För att möjliggöra en korrekt bedömning av min implementation har funktionaliteten därför lagts i en separat branch där den kan köras och testas självständigt.

Adishkas del av projektet, User-Api, finns implementerad i branchen inlamning. Detta är en påbyggnad av det stabila driftsätta systemet men med User-Api integration, dock med viss förlust av vissa funktioner från de redan integrerade APIerna. Då azure krachade och versionen inte hamnade på github hann vi ej bygga in detta i systemet, gruppen ville ej riskera att nya inkommande fel därav implementerades denna aldrig i main. Därav har vi lagt en separat branch för denna funktion där man både kan skapa konto och logga in som user och admin. 

admin kod (HV-Admin-2026)


UPPDATERAD README 24/4-2026

Notifications API är nu integrerat i systemet.

Notifikationer hämtas i NotificationsController och visas på /Notifications i webbappen.

---

Körs genom detta command i main branchen:
dotnet run --project ./LibrarySystem-T4/LibrarySystem-T4.csproj

