using System;


namespace Luxena.Travel.Domain
{
	public static class GeoUtility
	{
		// formula from http://www.movable-type.co.uk/scripts/latlong-vincenty.html
		public static double GetDistance(double lat1, double lon1, double lat2, double lon2)
		{
			var l = ToRad(lon2 - lon1);
			var u1 = Math.Atan((1 - F)*Math.Tan(ToRad(lat1)));
			var u2 = Math.Atan((1 - F)*Math.Tan(ToRad(lat2)));
			var sinU1 = Math.Sin(u1);
			var cosU1 = Math.Cos(u1);
			var sinU2 = Math.Sin(u2);
			var cosU2 = Math.Cos(u2);

			double sinSigma;
			double cosSigma;
			double sigma;
			double cosSqAlpha;
			double cos2SigmaM;

			var lambda = l;
			double lambdaP;
			var iterLimit = 100;
			do
			{
				var sinLambda = Math.Sin(lambda);
				var cosLambda = Math.Cos(lambda);

				sinSigma = Math.Sqrt((cosU2*sinLambda)*(cosU2*sinLambda) + (cosU1*sinU2 - sinU1*cosU2*cosLambda)*(cosU1*sinU2 - sinU1*cosU2*cosLambda));

				if (sinSigma == 0)
					return 0; // co-incident points

				cosSigma = sinU1*sinU2 + cosU1*cosU2*cosLambda;

				sigma = Math.Atan2(sinSigma, cosSigma);

				var sinAlpha = cosU1*cosU2*sinLambda/sinSigma;

				cosSqAlpha = 1 - sinAlpha*sinAlpha;

				cos2SigmaM = cosSigma - 2*sinU1*sinU2/cosSqAlpha;

				if (double.IsNaN(cos2SigmaM))
					cos2SigmaM = 0; // equatorial line: cosSqAlpha=0 (§6)

				var c = F/16*cosSqAlpha*(4 + F*(4 - 3*cosSqAlpha));

				lambdaP = lambda;

				lambda = l + (1 - c)*F*sinAlpha*(sigma + c*sinSigma*(cos2SigmaM + c*cosSigma*(-1 + 2*cos2SigmaM*cos2SigmaM)));
			} while (Math.Abs(lambda - lambdaP) > 1e-12 && --iterLimit > 0);

			if (iterLimit == 0)
				return double.NaN; // formula failed to converge

			var uSq = cosSqAlpha*(A*A - B*B)/(B*B);
			var a = 1 + uSq/16384*(4096 + uSq*(-768 + uSq*(320 - 175*uSq)));
			var b = uSq/1024*(256 + uSq*(-128 + uSq*(74 - 47*uSq)));
			var deltaSigma = b*sinSigma*(cos2SigmaM + b/4*(cosSigma*(-1 + 2*cos2SigmaM*cos2SigmaM) - b/6*cos2SigmaM*(-3 + 4*sinSigma*sinSigma)*(-3 + 4*cos2SigmaM*cos2SigmaM)));

			return B*a*(sigma - deltaSigma);
		}


		private static double ToRad(double angle)
		{
			return angle*Math.PI/180;
		}


		private const double A = 6378137.0;
		private const double B = 6356752.314245;
		private const double F = 1/298.257223563; // WGS-84 ellipsoid params
	}
}