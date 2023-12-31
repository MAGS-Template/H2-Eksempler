# ADO.NET
Eksempler til ADO.NET - Læs mere om ADO.NET her [Notion ADO.NET](https://mercantec.notion.site/ADO-NET-3b3c18cd2eba409f824ae82c6e9d933c?pvs=4)
Enten skal man manuelt sætte databasen op eller starte med at køre consol applikationen en gang. 
## Opbygning af projekt med ADO.NET
### ADONETConsole
Her er en consoleapp, som implementeret meget lavpraktisk. Den bruger ikke et objekt, men bare 2 strenge til at integrerer med databasen.
Den indeholder 4 metoder udover main() og Init(). <br>
<strong>CreateDatabase</strong> her tjekker den om ADONET databasen allerede findes på vores databaseserver eller laver den databasen. <br>
<strong>InsertData</strong> her indsætter den værdierne som er skrevet ind for de 2 strenge inden programmet kører. <br>
<strong>CreateTable</strong> her laver den vores tabel med en SQL kommando som er gemt som en streng. <br>
<strong>ReadData</strong> her læser den databasen ud fra SQL kommando som er gemt som en streng og bruger et while loop til at skrive dem ud i konsollen. <br>

## BlazorADONET
Her er en Blazor applikation som gør brug af samme database og tabel som er blevet oprettet af vores Consolapp. Derfor skal den anden applikation startes inden Blazor virker. <br>
Under projektet er der 2 forskellige Razor pages som kommunikere med databasen. 

### CRUD.razor
Under den her side er alt koden inkluderet, der er normalt dårlig praksis, men det er for at vise udviklingen. Det eneste vi har saperet er at vores klasser er inde i et Class Libary kaldet Domain Models.
Standart siden er meget simpel med 2 input felter og en knap med en eventhandler. 
Her indefra er der defineret 4 metoder til at håndtere hver CRUD operation. <br>
<strong>Create</strong> bliver håndteret af <strong>InsertData()</strong><br>
<strong>Read</strong> bliver håndteret af <strong>ReadData()</strong><br>
<strong>Update</strong> bliver håndteret af <strong>EditData(int personID)</strong><br>
<strong>Delete</strong> bliver håndteret af <strong>DeleteData(int personID)</strong><br>
### Select.razor

Her er koden mere struktureret end på CRUD.razor filen. Vi har lagt alt vores database logik ind i Services mappen. Den giver en del mere konfigurering i starten af filen. <br>
Udover at vi skal bruge "@using Domain_Models" for at få adgang til vores Person-klasse. Skal vi også have adgang til Services mappen som vi gør med "@using BlazorADONET.Services". For at få adgang til vores services, skal vi også bruge dependency injection her "@inject IDatabaseService DatabaseService"

Vores DatabaseService har 2 metoder som vi gør brug af.

<strong>SearchPersons(string selectedOption, string searchValue)</strong> her kan vi søge en specifik person op i vores database ud fra de kolonner som der findes i databasen. Fordi at vi bruger ADO.NET er det ikke et problem med SQL Injection.

<strong>GetColumnNames()</strong> her bruger vi det indbyggede ["COLUMN_NAME"] til at få en liste med alle kolonne værdi, så vi senere kan bruge dem i vores dropdown menu!
