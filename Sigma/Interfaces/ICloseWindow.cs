﻿// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using System;

namespace Sigma.Interfaces // Interface used to Close other windows
{
    internal interface ICloseWindow
    {
        Action CloseAction { get; set; }
    }
}