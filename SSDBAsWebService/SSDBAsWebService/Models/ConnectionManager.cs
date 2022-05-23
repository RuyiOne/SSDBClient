using StackExchange.Redis;
using ssdb;
public interface ConnectionManager
{

    IDatabaseAsync DatabaseConnection(string ipinfo, int portinfo);
    //Client U();
}