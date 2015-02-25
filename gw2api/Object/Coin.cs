using System;
using System.Text;

namespace gw2api.Object
{
    public class Coin : IEquatable<Coin>
    {
        public static double SalesTax = 0.15;
        public int Raw { get; private set; }

        public Coin(int raw)
        {
            Raw = raw;
        }

        public int Gold
        {
            get
            {
                if (Math.Abs(Raw) < 10000) return 0;
                return Raw/10000;
            }
        }

        public int Silver
        {
            get
            {
                if (Math.Abs(Raw) < 100) return 0;
                return Raw/100 - Gold*100;
            }
        }

        public int Copper
        {
            get
            {
                if (Math.Abs(Raw) < 100) return Raw;
                return Raw - Silver*100 - Gold*10000;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Raw < 0)
            {
                sb.Append("-");
            }
            if (Math.Abs(Gold) > 0)
            {
                sb.Append(Math.Abs(Gold)).Append("g ");
            }
            if (Math.Abs(Silver) > 0)
            {
                sb.Append(Math.Abs(Silver)).Append("s ");
            }
            sb.Append(Math.Abs(Copper)).Append("c");
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            var coin = obj as Coin;
            return coin != null && Equals(coin);
        }

        public bool Equals(Coin other)
        {
            return Raw == other.Raw;
        }

        public override int GetHashCode()
        {
            return Raw;
        }

        public static implicit operator int(Coin obj)
        {
            return obj.Raw;
        }

        public static implicit operator Coin(int raw)
        {
            return new Coin(raw);
        }

        public static Coin operator +(Coin a, Coin b)
        {
            return new Coin(a.Raw + b.Raw);
        }

        public static Coin operator -(Coin a, Coin b)
        {
            return new Coin(a.Raw - b.Raw);
        }

        public static Coin operator *(Coin a, double b)
        {
            return new Coin((int) (a.Raw*b));
        }

        public static Coin operator /(Coin a, double b)
        {
            return new Coin((int) (a.Raw/b));
        }

        public static bool operator <(Coin a, Coin b)
        {
            return a.Raw < b.Raw;
        }

        public static bool operator >(Coin a, Coin b)
        {
            return a.Raw > b.Raw;
        }

        public static Coin ProfitSellingAt(Coin price)
        {
            return price*(1.0 - SalesTax);
        }
    }

    public static class CurrencyExtensions
    {
        public static Coin AsCurrency(this int raw)
        {
            return new Coin(raw);
        }
    }
}