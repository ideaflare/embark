namespace Embark.DataChannel
{
    /// <summary>
    /// Data Transfer Object used to send ID and Object Serialized as text
    /// </summary>
    /// <remarks> Used instead of Tuple or KeyValuePair,
    /// because their key/item properties are get only,
    /// and any unknown object/text serializer can be passed to Embark that might not serialize/deserialize non-POCO classes correctly</remarks>
    public class DataEnvelope
    {
        /// <summary>
        /// An Int64 ID Unique to the document
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// JSON (XML, YAML or other) Text repesenting the serialized POCO/DTO/(or primitive data)
        /// </summary>
        public string Text { get; set; }
    }
}
