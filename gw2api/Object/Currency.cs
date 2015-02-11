using System;
using System.Text;

namespace gw2api.Object
{
    public class Currency
    {
        public static double SalesTax = 0.15;
        public int Raw { get; private set; }

        public Currency(int raw)
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

        public static implicit operator int(Currency obj)
        {
            return obj.Raw;
        }

        public static implicit operator Currency(int raw)
        {
            return new Currency(raw);
        }

        public static Currency operator +(Currency a, Currency b)
        {
            return new Currency(a.Raw + b.Raw);
        }

        public static Currency operator -(Currency a, Currency b)
        {
            return new Currency(a.Raw - b.Raw);
        }

        public static Currency operator *(Currency a, double b)
        {
            return new Currency((int) (a.Raw*b));
        }

        public static Currency operator /(Currency a, double b)
        {
            return new Currency((int) (a.Raw/b));
        }

        public static bool operator <(Currency a, Currency b)
        {
            return a.Raw < b.Raw;
        }

        public static bool operator >(Currency a, Currency b)
        {
            return a.Raw > b.Raw;
        }

        public static Currency ProfitSellingAt(Currency price)
        {
            return price*(1.0 - SalesTax);
        }
    }

    public static class CurrencyExtensions
    {
        public static Currency AsCurrency(this int raw)
        {
            return new Currency(raw);
        }
    }
}