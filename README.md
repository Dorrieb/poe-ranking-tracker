# Path of Exile ranking tracker #

This tracker displays your current progression in any league/event (including private leagues).
It periodically polls ladder information to display the rank and progress information of the character you chose.

You can customize the default renderers by simply editing the provided HTML templates, or better, create new ones !

# Configuration #

![Configuration sample](https://i.imgur.com/VUhGdlg.png)

1. Choose a language from the "Language" drop-down list
   1. Default language is computed from your OS language
2. Select a league from the "League/Race" drop-down list
   1. You can enter the name of a private league instead of selecting a public league.
3. Fill in your account name
4. Select a character from the "Character Name" drop-down list
   1. The characters list is populated after selecting a league and filling your account name
5. Optionally, fill in your [Session Id](#session-id-cookie "Goto Session Id information")
   1. Session Id cookie is used to boost the retrieval of ladder information
   2. You can leave this field empty, it will work anyway
6. Select the HTML template used to render the tracker
7. Click OK to start the tracker


# Tracker #

![Tracker sample](https://i.imgur.com/stKpIr5.png)

The information displayed on the tracker are periodically refreshed (10 seconds interval).

You can move the tracker anywhere you want. It will be displayed on top of other applications.

Double click on the tracker to return to configuration form.

# Templates #

Several HTML templates are provided.

* Light

![Tracker light](https://i.imgur.com/HlMStBG.png)

* Light with experience per hour

![Tracker light with XP](https://i.imgur.com/sO2vEvE.png)

* Detailed

![Tracker detailed](https://i.imgur.com/stKpIr5.png)

* Detailed without progress bar

![Tracker detailed no progress](https://i.imgur.com/CaFZkgr.png)

You can customize them, or even create your own templates.

CSS and JavaScript are supported in tracker rendering.

# Language #

Application language is automatically set according to OS language.
Currently, english and french languages are available and can be chosen.

# API information #

## Limits ##
The official [API](https://www.pathofexile.com/developer/docs/api) used to retrieve ladder information is limited in terms of requests, and it is not really made to follow realtime progress for POE characters.
This [post](https://www.pathofexile.com/forum/view-thread/2079853/filter-account-type/staff) by staff member Novynn explains the rules regarding the limits.

According to tests I have done, one request per second is allowed to avoid timeouts (without the Session Id cookie). Maybe this limit is more drastic when GGG servers are overloaded.
It's also possible that some IPs are associated with different limits.

This tracker tries to be as nice as possible by not spamming the API server. The goal is to be as efficient as possible, without falling in the rate-limited zone where IP is banned for 10, 20, or 30 seconds, even minutes sometimes.
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

You can track the ranking of any characters from any account name with your own Session Id cookie.

Be sure to not share your Session Id cookie with anyone.

## Maximum rank ##

Ranks over 15000 are not displayed.

## Credits ##

Icon made by [DinosoftLabs](https://www.flaticon.com/authors/dinosoftlabs) from www.flaticon.com 
