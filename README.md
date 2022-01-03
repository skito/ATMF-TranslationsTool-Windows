# ATMF Translations Tool for Windows
![GitHub release (latest by date)](https://img.shields.io/badge/platform-windows-lightgrey)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/skito/ATMF-TranslationsTool-Windows?label=Beta)


![Main screen](https://github.com/skito/ATMF-TranslationsTool-Windows/blob/master/Screenshots/main.png)

Tool for Windows which enables you to easily manage ATMF translations (workspaces). 

## Definitions when working with the app

__WORKSPACE__

The main ATMF culture folder. Opening **non ATMF-structured folders** will have **unexpected results**!

__LANGUAGE__

Language folder (first level folder) inside the main ATMF culture folder.

__NAMESPACE__

Path to the JSON translations file based from the language path.

Example structure:
```
culture/
|__en-US/
   |__page.json
   |__header.json
   |__settings/
      |__profile.json
|__bg-BG/
   |__page.json
   |__header.json
   |__settings/
```

In the example above there are 3 namespaces - ``page``, ``header``, ``settings/profile``.


## IMPORTANT

Please keep in mind that this is STILL BETA VERSION. It works, but it may contain a lot of bugs. Layout and functions are very basic. If you wish to make it better and have a coding capabilities on .NET - then you're welcome to contribute.

