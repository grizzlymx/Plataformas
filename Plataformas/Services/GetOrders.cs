using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GetLabel_LP.Services
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class Address
    {
        [JsonPropertyName("city")]
        public object city { get; set; }

        [JsonPropertyName("country_iso_code")]
        public string country_iso_code { get; set; }

        [JsonPropertyName("state")]
        public object state { get; set; }

        [JsonPropertyName("street_1")]
        public object street_1 { get; set; }

        [JsonPropertyName("street_2")]
        public object street_2 { get; set; }

        [JsonPropertyName("zip_code")]
        public object zip_code { get; set; }
    }

    public class BillingAddress
    {
        [JsonPropertyName("city")]
        public string city { get; set; }

        [JsonPropertyName("company")]
        public object company { get; set; }

        [JsonPropertyName("company_2")]
        public object company_2 { get; set; }

        [JsonPropertyName("country")]
        public string country { get; set; }

        [JsonPropertyName("country_iso_code")]
        public string country_iso_code { get; set; }

        [JsonPropertyName("firstname")]
        public string firstname { get; set; }

        [JsonPropertyName("lastname")]
        public string lastname { get; set; }

        [JsonPropertyName("phone")]
        public string phone { get; set; }

        [JsonPropertyName("state")]
        public string state { get; set; }

        [JsonPropertyName("street_1")]
        public string street_1 { get; set; }

        [JsonPropertyName("street_2")]
        public string street_2 { get; set; }

        [JsonPropertyName("zip_code")]
        public string zip_code { get; set; }
    }

    public class Center
    {
        [JsonPropertyName("code")]
        public string code { get; set; }
    }

    public class Channel
    {
        [JsonPropertyName("code")]
        public string code { get; set; }

        [JsonPropertyName("label")]
        public string label { get; set; }
    }

    public class CommissionTaxis
    {
        [JsonPropertyName("amount")]
        public double amount { get; set; }

        [JsonPropertyName("code")]
        public string code { get; set; }

        [JsonPropertyName("rate")]
        public double rate { get; set; }
    }

    public class Customer
    {
        [JsonPropertyName("billing_address")]
        public BillingAddress billing_address { get; set; }

        [JsonPropertyName("civility")]
        public object civility { get; set; }

        [JsonPropertyName("customer_id")]
        public string customer_id { get; set; }

        [JsonPropertyName("firstname")]
        public string firstname { get; set; }

        [JsonPropertyName("lastname")]
        public string lastname { get; set; }

        [JsonPropertyName("locale")]
        public string locale { get; set; }

        [JsonPropertyName("shipping_address")]
        public ShippingAddress shipping_address { get; set; }
    }

    public class Fulfillment
    {
        [JsonPropertyName("center")]
        public Center center { get; set; }
    }

    public class Order
    {
        [JsonPropertyName("acceptance_decision_date")]
        public DateTime acceptance_decision_date { get; set; }

        [JsonPropertyName("can_cancel")]
        public bool can_cancel { get; set; }

        [JsonPropertyName("can_shop_ship")]
        public bool can_shop_ship { get; set; }

        [JsonPropertyName("channel")]
        public Channel channel { get; set; }

        [JsonPropertyName("commercial_id")]
        public string commercial_id { get; set; }

        [JsonPropertyName("created_date")]
        public DateTime created_date { get; set; }

        [JsonPropertyName("currency_iso_code")]
        public string currency_iso_code { get; set; }

        [JsonPropertyName("customer")]
        public Customer customer { get; set; }

        [JsonPropertyName("customer_debited_date")]
        public DateTime customer_debited_date { get; set; }

        [JsonPropertyName("customer_directly_pays_seller")]
        public bool customer_directly_pays_seller { get; set; }

        [JsonPropertyName("delivery_date")]
        public object delivery_date { get; set; }

        [JsonPropertyName("fulfillment")]
        public Fulfillment fulfillment { get; set; }

        [JsonPropertyName("fully_refunded")]
        public bool fully_refunded { get; set; }

        [JsonPropertyName("has_customer_message")]
        public bool has_customer_message { get; set; }

        [JsonPropertyName("has_incident")]
        public bool has_incident { get; set; }

        [JsonPropertyName("has_invoice")]
        public bool has_invoice { get; set; }

        [JsonPropertyName("last_updated_date")]
        public DateTime last_updated_date { get; set; }

        [JsonPropertyName("leadtime_to_ship")]
        public int leadtime_to_ship { get; set; }

        [JsonPropertyName("order_additional_fields")]
        public List<OrderAdditionalField> order_additional_fields { get; set; }

        [JsonPropertyName("order_id")]
        public string order_id { get; set; }

        [JsonPropertyName("order_lines")]
        public List<OrderLine> order_lines { get; set; }

        [JsonPropertyName("order_state")]
        public string order_state { get; set; }

        [JsonPropertyName("order_state_reason_code")]
        public object order_state_reason_code { get; set; }

        [JsonPropertyName("order_state_reason_label")]
        public object order_state_reason_label { get; set; }

        [JsonPropertyName("order_tax_mode")]
        public string order_tax_mode { get; set; }

        [JsonPropertyName("order_taxes")]
        public object order_taxes { get; set; }

        [JsonPropertyName("paymentType")]
        public string paymentType { get; set; }

        [JsonPropertyName("payment_type")]
        public string payment_type { get; set; }

        [JsonPropertyName("payment_workflow")]
        public string payment_workflow { get; set; }

        [JsonPropertyName("price")]
        public double price { get; set; }

        [JsonPropertyName("promotions")]
        public Promotions promotions { get; set; }

        [JsonPropertyName("quote_id")]
        public object quote_id { get; set; }

        [JsonPropertyName("shipping_carrier_code")]
        public object shipping_carrier_code { get; set; }

        [JsonPropertyName("shipping_carrier_standard_code")]
        public object shipping_carrier_standard_code { get; set; }

        [JsonPropertyName("shipping_company")]
        public object shipping_company { get; set; }

        [JsonPropertyName("shipping_deadline")]
        public DateTime shipping_deadline { get; set; }

        [JsonPropertyName("shipping_price")]
        public double shipping_price { get; set; }

        [JsonPropertyName("shipping_pudo_id")]
        public object shipping_pudo_id { get; set; }

        [JsonPropertyName("shipping_tracking")]
        public object shipping_tracking { get; set; }

        [JsonPropertyName("shipping_tracking_url")]
        public object shipping_tracking_url { get; set; }

        [JsonPropertyName("shipping_type_code")]
        public string shipping_type_code { get; set; }

        [JsonPropertyName("shipping_type_label")]
        public string shipping_type_label { get; set; }

        [JsonPropertyName("shipping_type_standard_code")]
        public string shipping_type_standard_code { get; set; }

        [JsonPropertyName("shipping_zone_code")]
        public string shipping_zone_code { get; set; }

        [JsonPropertyName("shipping_zone_label")]
        public string shipping_zone_label { get; set; }

        [JsonPropertyName("total_commission")]
        public double total_commission { get; set; }

        [JsonPropertyName("total_price")]
        public double total_price { get; set; }
    }

    public class OrderAdditionalField
    {
        [JsonPropertyName("code")]
        public string code { get; set; }

        [JsonPropertyName("type")]
        public string type { get; set; }

        [JsonPropertyName("value")]
        public string value { get; set; }
    }

    public class OrderLine
    {
        [JsonPropertyName("can_refund")]
        public bool can_refund { get; set; }

        [JsonPropertyName("cancelations")]
        public List<object> cancelations { get; set; }

        [JsonPropertyName("category_code")]
        public string category_code { get; set; }

        [JsonPropertyName("category_label")]
        public string category_label { get; set; }

        [JsonPropertyName("commission_fee")]
        public double commission_fee { get; set; }

        [JsonPropertyName("commission_rate_vat")]
        public double commission_rate_vat { get; set; }

        [JsonPropertyName("commission_taxes")]
        public List<CommissionTaxis> commission_taxes { get; set; }

        [JsonPropertyName("commission_vat")]
        public double commission_vat { get; set; }

        [JsonPropertyName("created_date")]
        public DateTime created_date { get; set; }

        [JsonPropertyName("debited_date")]
        public DateTime debited_date { get; set; }

        [JsonPropertyName("description")]
        public object description { get; set; }

        [JsonPropertyName("fees")]
        public List<object> fees { get; set; }

        [JsonPropertyName("last_updated_date")]
        public DateTime last_updated_date { get; set; }

        [JsonPropertyName("offer_id")]
        public int offer_id { get; set; }

        [JsonPropertyName("offer_sku")]
        public string offer_sku { get; set; }

        [JsonPropertyName("offer_state_code")]
        public string offer_state_code { get; set; }

        [JsonPropertyName("order_line_additional_fields")]
        public List<OrderLineAdditionalField> order_line_additional_fields { get; set; }

        [JsonPropertyName("order_line_id")]
        public string order_line_id { get; set; }

        [JsonPropertyName("order_line_index")]
        public int order_line_index { get; set; }

        [JsonPropertyName("order_line_state")]
        public string order_line_state { get; set; }

        [JsonPropertyName("order_line_state_reason_code")]
        public object order_line_state_reason_code { get; set; }

        [JsonPropertyName("order_line_state_reason_label")]
        public object order_line_state_reason_label { get; set; }

        [JsonPropertyName("price")]
        public double price { get; set; }

        [JsonPropertyName("price_additional_info")]
        public object price_additional_info { get; set; }

        [JsonPropertyName("price_unit")]
        public double price_unit { get; set; }

        [JsonPropertyName("product_medias")]
        public List<ProductMedia> product_medias { get; set; }

        [JsonPropertyName("product_sku")]
        public string product_sku { get; set; }

        [JsonPropertyName("product_title")]
        public string product_title { get; set; }

        [JsonPropertyName("promotions")]
        public List<object> promotions { get; set; }

        [JsonPropertyName("quantity")]
        public int quantity { get; set; }

        [JsonPropertyName("received_date")]
        public object received_date { get; set; }

        [JsonPropertyName("refunds")]
        public List<object> refunds { get; set; }

        [JsonPropertyName("shipped_date")]
        public object shipped_date { get; set; }

        [JsonPropertyName("shipping_from")]
        public ShippingFrom shipping_from { get; set; }

        [JsonPropertyName("shipping_price")]
        public double shipping_price { get; set; }

        [JsonPropertyName("shipping_price_additional_unit")]
        public object shipping_price_additional_unit { get; set; }

        [JsonPropertyName("shipping_price_unit")]
        public object shipping_price_unit { get; set; }

        [JsonPropertyName("shipping_taxes")]
        public List<object> shipping_taxes { get; set; }

        [JsonPropertyName("taxes")]
        public List<object> taxes { get; set; }

        [JsonPropertyName("total_commission")]
        public double total_commission { get; set; }

        [JsonPropertyName("total_price")]
        public double total_price { get; set; }
    }

    public class OrderLineAdditionalField
    {
        [JsonPropertyName("code")]
        public string code { get; set; }

        [JsonPropertyName("type")]
        public string type { get; set; }

        [JsonPropertyName("value")]
        public string value { get; set; }
    }

    public class ProductMedia
    {
        [JsonPropertyName("media_url")]
        public string media_url { get; set; }

        [JsonPropertyName("mime_type")]
        public string mime_type { get; set; }

        [JsonPropertyName("type")]
        public string type { get; set; }
    }

    public class Promotions
    {
        [JsonPropertyName("applied_promotions")]
        public List<object> applied_promotions { get; set; }

        [JsonPropertyName("total_deduced_amount")]
        public int total_deduced_amount { get; set; }
    }

    public class GetOrders
    {
        [JsonPropertyName("orders")]
        public List<Order> orders { get; set; }

        [JsonPropertyName("total_count")]
        public int total_count { get; set; }
    }

    public class ShippingAddress
    {
        [JsonPropertyName("additional_info")]
        public object additional_info { get; set; }

        [JsonPropertyName("city")]
        public string city { get; set; }

        [JsonPropertyName("company")]
        public object company { get; set; }

        [JsonPropertyName("company_2")]
        public object company_2 { get; set; }

        [JsonPropertyName("country")]
        public string country { get; set; }

        [JsonPropertyName("country_iso_code")]
        public string country_iso_code { get; set; }

        [JsonPropertyName("firstname")]
        public string firstname { get; set; }

        [JsonPropertyName("lastname")]
        public string lastname { get; set; }

        [JsonPropertyName("phone")]
        public string phone { get; set; }

        [JsonPropertyName("state")]
        public string state { get; set; }

        [JsonPropertyName("street_1")]
        public string street_1 { get; set; }

        [JsonPropertyName("street_2")]
        public string street_2 { get; set; }

        [JsonPropertyName("zip_code")]
        public string zip_code { get; set; }
    }

    public class ShippingFrom
    {
        [JsonPropertyName("address")]
        public Address address { get; set; }
    }


}
