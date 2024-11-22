using Pallas.NET.Models;

namespace Pallas.NET.Utils;

public static class ClientUtils
{
    public static Point MapPallasPoint(PallasDotnetRs.PallasDotnetRs.Point rsPoint)
        => new(rsPoint.slot, Convert.ToHexString(rsPoint.hash.ToArray()));
}