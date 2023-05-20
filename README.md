# ActivityCalculator
This is a customized activity time calculator based on my Google Calendar.

## How to use
1. You need OAuth 2.0 Client Id from Google API Console in the name of `credentials.json`.
2. You need `appsettings.json` file **in the root directory** where you store the following data:
	1. `CalendarId`: the id of the specific calendar that you want to get events from([How can I get the CalendarId?](https://developers.google.com/calendar/api/v3/reference/calendarList/list))
	3. `CredentialPath`: it's the path of the `credentials.json` file.

## Todo
- [ ] Make it a simple web app so I can use it on any devices where I can log in to my Google account.
- [ ] Detect if there are at least two concurrent events. If so, fail the operation.
