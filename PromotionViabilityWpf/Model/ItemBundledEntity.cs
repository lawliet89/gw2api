using System;
using System.ComponentModel;
using System.Globalization;
using System.Reactive.Linq;
using gw2api.Object;
using GW2NET.Commerce;
using GW2NET.Items;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactiveUI;

namespace PromotionViabilityWpf.Model
{
    [JsonConverter(typeof(ItemEntityConverter))]
    [TypeConverter(typeof(ItemStringConverter))]
    public class ItemBundledEntity : ReactiveObject,
        IBundledEntity<int, Item>, 
        IBundledEntity<int, AggregateListing>,
        IBundleableRenderableEntity<Item>
    {
        public ItemBundledEntity(int id)
        {
            Identifier = id;

            this.WhenAnyValue(x => x.Listings)
                .Where(l => l != null)
                .Select(l => (Coin)l.BuyOffers.UnitPrice)
                .ToProperty(this, x => x.MaxOfferUnitPrice, out maxOfferUnitPrice, 0);
            this.WhenAnyValue(x => x.Listings)
                .Where(l => l != null)
                .Select(l => (Coin)l.SellOffers.UnitPrice)
                .ToProperty(this, x => x.MinSaleUnitPrice, out minSaleUnitPrice, 0);
        }

        public ItemBundledEntity(string stringId) : this(Convert.ToInt32(stringId))
        {
        }

        private Item item;

        public Item Item
        {
            get { return item; }
            private set { this.RaiseAndSetIfChanged(ref item, value); }
        }

        private AggregateListing listings;

        public AggregateListing Listings
        {
            get { return listings; }
            private set { this.RaiseAndSetIfChanged(ref listings, value); }
        }

        private byte[] iconPng;

        public byte[] IconPng
        {
            get { return iconPng; }
            private set { this.RaiseAndSetIfChanged(ref iconPng, value); }
        }

        private readonly ObservableAsPropertyHelper<Coin> maxOfferUnitPrice; 
        public Coin MaxOfferUnitPrice
        {
            get { return maxOfferUnitPrice.Value; }
        }

        private readonly ObservableAsPropertyHelper<Coin> minSaleUnitPrice; 
        public Coin MinSaleUnitPrice
        {
            get { return minSaleUnitPrice.Value; }
        }

        #region IBundledEntity Overrides
        public Item Object
        {
            get { return Item; }
            set { Item = value; }
        }

        AggregateListing IBundledEntity<int, AggregateListing>.Object
        {
            get { return Listings; }
            set { Listings = value; }
        }

        public int Identifier { get; private set; }

        public Item Renderable
        {
            get { return Item; }
        }

        public byte[] Icon
        {
            set { IconPng = value; }
        }
        #endregion
    }

    public class ItemEntityConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var item = value as ItemBundledEntity;
            if (item == null)
                throw new InvalidOperationException("Tried to convert object not of type ItemBundledEntity");
            JToken.FromObject(item.Identifier).WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            var id = Convert.ToInt32(token.ToString());
            return new ItemBundledEntity(id);
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof (ItemBundledEntity));
        }
    }

    public class ItemStringConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof (string) || sourceType == typeof (ItemBundledEntity))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string) || destinationType == typeof(ItemBundledEntity))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var stringid = value as string;
            if (stringid != null)
            {
                return new ItemBundledEntity(stringid);
            }
            var item = value as ItemBundledEntity;
            if (item != null)
            {
                return item.Identifier.ToString();
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var stringId = value as string;
            if (stringId != null && destinationType == typeof (ItemBundledEntity))
            {
                return new ItemBundledEntity(stringId);
            }
            var item = value as ItemBundledEntity;
            if (item != null && destinationType == typeof (string))
            {
                return item.Identifier.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}