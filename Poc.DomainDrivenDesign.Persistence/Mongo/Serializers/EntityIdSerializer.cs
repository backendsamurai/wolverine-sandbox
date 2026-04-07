using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Poc.DomainDrivenDesign.Domain.Common;

namespace Poc.DomainDrivenDesign.Persistence.Mongo.Serializers;

public sealed class EntityIdSerializer<TId> : SerializerBase<TId> where TId : IEntityId
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TId value)
    {
        context.Writer.WriteGuid(value.Value);
    }

    public override TId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var id = context.Reader.ReadGuid();

        return (TId)Activator.CreateInstance(typeof(TId), id)!;
    }
}
