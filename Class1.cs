using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PilotGaea.Geometry;
using PilotGaea.TMPEngine;
using PilotGaea.Serialize;

namespace Snap
{
    public class SnapClass : SnapBaseClass
    {
        public override void DeInit()
        {
        }

        public override bool GetSnapGeometry(CGeoDatabase DB, string LayerName, GeoPoint Point, double Distance, int EPSGCode, out List<Geo> Geos)
        {
            bool Ret = false;
            Geos = new List<Geo>();
            CVectorLayer Layer = DB.FindLayer(LayerName) as CVectorLayer_Base;
            if (Layer == null) return Ret;
            CEntityFetcher Fetcher = Layer.SearchByDistance(EPSGCode, Point, Distance);
            for (int i = 0; i < Fetcher.Count; i++)
            {
                GeoPoint p = new GeoPoint();
                GeoPolyline pl = new GeoPolyline();
                GeoPolygonSet pgs = new GeoPolygonSet();
                switch (Fetcher.GetType(i))
                {
                    case GEO_TYPE.POINT:
                        {
                            Fetcher.GetGeo(i, ref p);
                            Geos.Add(p);
                        }
                        break;
                    case GEO_TYPE.POLYLINE:
                        {
                            Fetcher.GetGeo(i, ref pl);
                            Geos.Add(pl);
                        }
                        break;
                    case GEO_TYPE.POLYGONSET:
                        {
                            Fetcher.GetGeo(i, ref pgs);
                            Geos.Add(pgs);
                        }
                        break;
                }
            }
            //Ret = true; //如果傳回true，表示只拿這些圖素做計算
            return Ret;
        }

        public override bool Init()
            {
                return true;
            }
        }
}
