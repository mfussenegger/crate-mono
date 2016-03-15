using System;
using GeoAPI.Geometries;
using NetTopologySuite.IO;

namespace Crate.Client
{
    public static class CrateGeoExtensions
    {
        public static IGeometry GetGeometry(this CrateDataReader reader, int i)
        {
            return new GeoJsonReader().Read<IGeometry>(reader.sqlResponse.rows[reader.currentRow][i].ToString());
        }
    }
}
