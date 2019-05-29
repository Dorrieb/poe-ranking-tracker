# Path of Exile ranking tracker #

This tracker displays your current progression in any league/event.
It periodically polls ladder information to display the rank and progress information of the character you chose.

# Configuration #

1. Select a league from the drop-down list
2. Fill in your account name
3. Select a character from the drop-down list
4. Optionally, fill in your [Session Id](#session-id-cookie "Goto Session Id information")
5. Select options you want to display in the tracker
6. Modify the colors of the tracker
7. Click OK to start the tracker

# Tracker #

Double click on the tracker to return to configuration form.

To exit :
* Click on tracker to gain focus, and ALT+F4
* Double-click on tracker to return to configuration, and click on close button

# Language #

Application language is automatically set according to OS language.
Currently, english and french languages are available and can be chosen.

# API information #

## Limits ##
The official [API](https://www.pathofexile.com/developer/docs/api) used to retrieve ladder information is limited in terms of requests, and it is not really made to follow realtime progress for POE characters.
This [post](https://www.pathofexile.com/forum/view-thread/2079853/filter-account-type/staff) by staff member Novynn explains the rules regarding the limits.

According to tests I have done, one request per second is allowed to avoid timeouts (without the Session Id cookie). Maybe this limit is more drastic when GGG servers are overloaded.
It's also possible that some IPs are associated with different limits.

This tracker tries to be as nice as possible by not spamming the API server. The goal is to be as efficient as possible, without falling in the rate-limited zone where download is suspended for 10, 20, or 30 seconds, even minutes sometimes.
After each request done to the API server, the response is checked to verify if the next request can be done immediately, or if a delay is necessary.

This has consequences, specifically if your rank is low. The lower the rank, the longer it will take to display updated information.
Assuming one second delay is constantly necessary to retrieve 200 entries, this gives the following metrics :
* Rank 13875 => 70s
* Rank 7453  => 38s
* Rank 2890  => 14s
* Rank 625   => 4s
* Rank < 200 => 1s

## Session Id cookie ##

The Session Id cookie is automatically created when you login on [Path of Exile](https://www.pathofexile.com) website.
It allows you to navigate on different pages without having to login each time.
The interesting thing here is that it also allows to increase the rules limits (the requests number can be doubled).

Follow this [guide](https://github.com/Stickymaddness/Procurement/wiki/SessionID) to retrieve your Session Id.
Once you have your Session Id, just paste it in the configuration field.

Be sure to not share your Session Id cookie with anyone.

## Maximum rank ##

Ranks over 15000 are not displayed.

## Credits ##

Icon made by [DinosoftLabs](https://www.flaticon.com/authors/dinosoftlabs) from www.flaticon.com 
