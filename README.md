# gw2lam

A location-aware music player for [Guild Wars 2](https://www.guildwars2.com/) (GW2) written in C#.

GW2 provides a player's character and position data in a [memory-mapped file](https://en.wikipedia.org/wiki/Memory-mapped_file) formatted for use with [Mumble's](http://wiki.mumble.info/wiki/Link) positional audio support. The [GwApiNET](https://gwapinet.codeplex.com/) library is used to access this memory-mapped file to obtain the player's current location in the GW2 world and [NAudio](https://naudio.codeplex.com/) is used to play custom music the player has provided for that specific location.


## Differences with GW's Soundtrack Customization
Gw2lam provides a different experience than GW2's own [soundtrack customization features](https://wiki.guildwars2.com/wiki/Custom_music):

##### Advantages
* Players can customize the music of every map, including WvW maps, PvP maps, and story-based instanced maps
* Each map can have unique, dedicated music track(s)
* Utilizes a simple directory structure for customizing map music instead of playlists

##### Disadvantages
* No support for main menu music (cannot detect when you are in the character selection screen)
* Cannot customize night time, underwater, battle, and downed music (this information is not provided in GW2's memory-mapped file


## Usage
~~[Download the gw2lam distributable here.](#)~~ *(Still in development!)*

There is no installation executable, simply extract the contents of the zip file into a new folder in any location. Run gw2lam.exe alongside Guild Wars 2 (it does not matter which one is started first). 

Inside the main folder is a directory called **music**. When customizing the music of a map, create a subdirectory in **music** with the same name as your desired map. Place your music files inside that subdirectory. 

For example, if you want to customize the music of [Divinity's Reach](https://wiki.guildwars2.com/wiki/Divinity%27s_reach), the human capital of [Tyria](https://wiki.guildwars2.com/wiki/Tyria), you would place your custom music in **music/Divinity's Reach**. For convenience, the gw2lam distributable has most of the subdirectories for major maps already created.

Music can be customized while both GW2 and gw2lam are already running. However, you will need to exit and re-enter a map to hear the changes. At the moment, only the .mp3, .ogg, and .wav formats are supported.


## Compiling

gw2lam was built using .NET 4.5. It is depdendent on these two libraries:
* [GwApiNET](https://gwapinet.codeplex.com/)
* [NAudio](https://naudio.codeplex.com/)
* [NVorbis](https://nvorbis.codeplex.com/)

You will need these two libraries referenced in your project in order to compile this project.

## License
```
The MIT License (MIT)

Copyright (c) 2014 Kevin Sacro

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
