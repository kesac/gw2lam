# Glam

### What is this?
Glam is an alternative to [Guild Wars 2's](https://www.guildwars2.com/) [soundtrack customization](https://wiki.guildwars2.com/wiki/Customized_soundtrack) features. It supports map-specific music instead of situation-specific music. Glam stands for *Guild Wars 2 Location-Aware Music*.

### How does it work?
GW2 provides a player's character and position data in a [memory-mapped file](https://en.wikipedia.org/wiki/Memory-mapped_file),  formatted for use with [Mumble's](http://wiki.mumble.info/wiki/Link) positional audio support. Glam is used to access this memory-mapped file to obtain the player's current location in the GW2 world and play custom music for that specific location.

### What are the differences compared with the built-in soundtrack customization?

##### Advantages
* You can customize the music for every map, including WvW maps, PvP maps, and story-based instanced maps
* Each map can have unique, dedicated music track(s)
* It utilizes a simple directory structure for customizing map music

##### Disadvantages
* No support for main menu music (GW2 does not provide position data when you're not in a map)
* Cannot customize music for night-time, battle, underwater, etc. situations. (GW2's memory-mapped file does not provide this information)

### Usage - Glam Desktop Player
~~[Download the desktop player here.](#)~~ *(Still in development!)*

There is no installer, simply extract the contents of the zip file into a new folder in any location. Run Glam.Desktop.exe alongside Guild Wars 2 (it does not matter which one is started first). 

Inside the application folder is a directory called *music*. When customizing the music of a map, create a subdirectory in *music* with the same name as your desired map. Place your music **files** or **playlists** inside that subdirectory. 

For example, if you want to customize the music of [Divinity's Reach](https://wiki.guildwars2.com/wiki/Divinity%27s_reach), you would place your custom music in *music/Divinity's Reach*. 

Music can be customized while both GW2 and Glam is already running. You will need to exit and re-enter a map to hear the changes however.

### Supported Audio Formats
 * mp3
 * wav
 * ogg

### Supported Playlist Formats
 * m3u

## License
```
The MIT License (MIT)

Copyright (c) 2014-2016 Kevin Sacro

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
