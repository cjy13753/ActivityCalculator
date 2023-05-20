# ActivityCalculator
This is a customized activity time calculator based on my Google Calendar.

## How to use
1. You need OAuth 2.0 Client Id from Google API Console in the name of `credentials.json`.
2. You need `appsettings.json` file **in the root directory** where you store the following data:
	1. `CalendarId`: the id of the specific calendar that you want to get events from.
	2. `CredentialPath`: it's the path of the `credentials.json` file.