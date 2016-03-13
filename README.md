# Glam

### What is this?
Glam is an alternative to [Guild Wars 2's](https://www.guildwars2.com/) built-in [soundtrack customization](https://wiki.guildwars2.com/wiki/Customized_soundtrack) feature. Glam supports location specific music instead of situation based music. Glam stands for *Guild Wars 2 Location-Aware Music*.

### How does it work?
GW2 writes a player's character and position data to a [memory-mapped file](https://wiki.guildwars2.com/wiki/API:MumbleLink). Glam uses the information within the memory-mapped file to find out a GW2 player's current location in the game world and plays custom music for that location.

#### Advantages over built-in customization
* You can customize the music for every map, including WvW maps, PvP maps, and story-based instanced maps
* Each map can have unique, dedicated music tracks
* Customizing is easier: Glam uses a simple directory structure for customizing map music instead of depending on playlists

#### Disadvantages compared to built-in customization
* Cannot customize situation based music. For example, Glam has no way of telling when it is night-time or when you are in a battle. GW2's memory-mapped file does not provide this information
* There is no support for main menu music. GW2 does not provide position data when you're not in a map

### Usage
[Please see the wiki for detailed usage instructions.](https://github.com/kesac/gw2lam/wiki)

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
