/*
 * This class is a modified copy of the Gw2PositionReader from GWAPINET (https://gwapinet.codeplex.com) used
 * under the terms of its MIT license:
 * 
 * Copyright (c) 2013 Chris Neal, Philip Barcelon
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial
 * portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
 * TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Glam
{
    public class MumbleLink : IDisposable
    {
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct LinkedMem
        {
            public uint uiVersion;//4
            public uint uiTick;//4
            public fixed float fAvatarPosition[3];//12
            public fixed float fAvatarFront[3];//12
            public fixed float fAvatarTop[3];//12
            public fixed byte name[512];//512
            public fixed float fCameraPosition[3];//12
            public fixed float fCameraFront[3];//12
            public fixed float fCameraTop[3];//12
            public fixed byte identity[512];//512
            public uint context_len;//4
            //public fixed byte context[512];//512
            public fixed byte serverAddress[28]; // context[0], contains sockaddr_in or sockaddr_in6
            public uint mapId; // context[28]
            public uint mapType; // context[32]
            public uint worldId; // context[36]
            public uint instance; // context[40]
            public uint build; // context[44]
            public fixed byte contextUnused[464];
            public fixed byte description[4096];//4096
        }

        private MemoryMappedFile MappedFile;
        private MemoryMappedViewAccessor Accessor;
        private LinkedMem Data;

        public uint Tick
        {
            get
            {
                if (this.MappedFile == null) this.OpenMumbleLink();
                this.Accessor.Read(0, out this.Data);

                unsafe
                {
                    fixed (LinkedMem* _data = &this.Data)
                    {
                        return _data->uiTick;
                    }
                }
            }
        }

        public uint MapId
        {
            get
            {
                if (this.MappedFile == null) this.OpenMumbleLink();
                this.Accessor.Read(0, out this.Data);

                unsafe
                {
                    fixed (LinkedMem* _data = &this.Data)
                    {
                        return _data->mapId;
                    }
                }
            }
        }

        public MumbleLink()
        {
            this.Data = new LinkedMem();
        }

        private void OpenMumbleLink()
        {
            this.MappedFile = MemoryMappedFile.CreateOrOpen("MumbleLink", Marshal.SizeOf(this.Data));
            this.Accessor = this.MappedFile.CreateViewAccessor(0, Marshal.SizeOf(this.Data));
        }

        public void Dispose()
        {

            this.MappedFile.Dispose();
        }
    }
}
