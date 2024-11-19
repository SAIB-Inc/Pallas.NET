﻿using PallasDotnet.Models.Enums;

namespace PallasDotnet.Models;

public record NextResponse(
    NextResponseAction Action,
    Point Tip,
    byte[] BlockCbor
);