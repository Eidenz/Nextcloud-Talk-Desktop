# Nextcloud Talk Desktop

[__Download__](https://github.com/Eidenz/Nextcloud-Talk-Desktop/releases)

I needed a desktop app for Nextcloud Talk so I can get instant desktop notifications, but I didn't find any.

So I decided to make one myself using the well-documented Talk API.


|TODO|
|----------------|
|Add options to change sync timer, API keys, URL, etc..|
|Add support for Maintenance mode|


# Installation
## From released builds
__This app requires .NET framework 4.7.2 or later to work!__ An up-to-date Windows 10 installation should have it by default.

Simply extract the zip file to a place of your choice. __The user must have write access to the folder!__

Simply run Talk.exe and follow the setup guide.

On the first execution, Nextcloud will generate an API key for the app, so it doesn't have to deal with your real password and the risks of storing it.


## Building from source
_TO-DO_


## Usage
Once setup, the app will appear as a simple icon in your system tray.

Left clicking it will show a small window with an embedded Chromium browser pointing to your Talk instance.

You can exit the app by right clicking on the icon and pressing "Exit".

When you receive a Talk message, you'll see a desktop notification within 15 secs, showing both the message content and author.
