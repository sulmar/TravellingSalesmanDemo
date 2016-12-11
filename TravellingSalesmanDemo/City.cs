﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace TravellingSalesmanDemo
{
    [Serializable]
    public class City
    {
        public City(string name, double latitude, double longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }

        public string Name { set; get; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double GetDistanceFromPosition(double latitude, double longitude)
        {
            var R = 6371; // radius of the earth in km
            var dLat = DegreesToRadians(latitude - Latitude);
            var dLon = DegreesToRadians(longitude - Longitude);
            var a =
                Sin(dLat / 2) * Sin(dLat / 2) +
                Cos(DegreesToRadians(Latitude)) * Cos(DegreesToRadians(latitude)) *
                Sin(dLon / 2) * Sin(dLon / 2)
                ;
            var c = 2 * Atan2(Sqrt(a), Sqrt(1 - a));
            var d = R * c; // distance in km
            return d;
        }

        private static double DegreesToRadians(double deg)
        {
            return deg * (PI / 180);
        }

        public byte[] ToBinaryString()
        {
            var result = new byte[6];

            return result;
        }

        public override bool Equals(object obj)
        {
            var item = obj as City;
            return Equals(item);
        }

        protected bool Equals(City other)
        {
            return string.Equals(Name, other.Name) && Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Latitude.GetHashCode();
                hashCode = (hashCode * 397) ^ Longitude.GetHashCode();
                return hashCode;
            }
        }
    }

}
