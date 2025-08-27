using Unity.Collections;
using Unity.Netcode;

public struct PlayerStats : INetworkSerializable
{
    public int Kills;
    public int Deaths;
    public FixedString128Bytes displayName;
    public ulong clientID;



    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {

        serializer.SerializeValue(ref Kills);
        serializer.SerializeValue(ref Deaths);
        serializer.SerializeValue(ref displayName);
        serializer.SerializeValue(ref clientID);
    }
}
