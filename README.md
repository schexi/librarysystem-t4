Deploy-MVC är huvud-applikationen som fungerar, det är den som är driftsatt i azure via:

https://t4bibliotek.azurewebsites.net/


 Edins del av projektet, Notifications API, finns implementerad i branchen edin-notifications.

Gruppen valde att inte integrera denna del i huvudbranchen (main) i ett sent skede av projektet för att undvika att introducera potentiella fel i den gemensamma slutversionen samt att Azure vart nere en längre period.

För att möjliggöra en korrekt bedömning av min implementation har funktionaliteten därför lagts i en separat branch där den kan köras och testas självständigt.

Adishkas del av projektet, User-Api, finns implementerad i branchen inlamning. Då azure krachade hann vi ej bygga in detta i systemet, gruppen ville ej riskera att nya inkommande fel därav implementerades denna aldrig i main. Därav har vi lagt en separat branch för denna funktion där man både kan skapa konto och logga in som user och admin. 

admin kod (HV-Admin-2026)
