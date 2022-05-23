using StackExchange.Redis;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using ssdb;
using System.Configuration;
public class SSDBConsoleHost: IDisposable,ConnectionManager
{
    

    public Client SSDBConnection { get; set; }
    public IDatabaseAsync DatabaseConnection(string ipinfo, int portinfo)
    {
        
        if (ipinfo == null || portinfo == 0)
        {
             var config = new ConfigurationOptions
             {
                 EndPoints = { { "localhost", 8888 } },
                 CommandMap = CommandMap.Create(new HashSet<string> {
                     "ping", "get", "set", "del", "incr", "incrby", "mget", "mset", "keys", "getset", "setnx",
                     "hget", "hset", "hdel", "hincrby", "hkeys", "hvals", "hmget", "hmset", "hlen",
                     "zscore", "zadd", "zrem", "zrange", "zrangebyscore", "zincrby", "zdecrby", "zcard",
                     "llen", "lpush", "rpush", "lpop", "rpop", "lrange", "lindex" }, true)
             };
             ConnectionMultiplexer ssdb = ConnectionMultiplexer.Connect(config);
             IDatabase db = ssdb.GetDatabase();
             return db;         
        }

        else{
             var config = new ConfigurationOptions
             {
                 EndPoints = { { ipinfo, portinfo} },
                 CommandMap = CommandMap.Create(new HashSet<string> {
                     "ping", "get", "set", "del", "incr", "incrby", "mget", "mset", "keys", "getset", "setnx",
                     "hget", "hset", "hdel", "hincrby", "hkeys", "hvals", "hmget", "hmset", "hlen",
                     "zscore", "zadd", "zrem", "zrange", "zrangebyscore", "zincrby", "zdecrby", "zcard",
                     "llen", "lpush", "rpush", "lpop", "rpop", "lrange", "lindex" }, true)
             };
             ConnectionMultiplexer ssdb = ConnectionMultiplexer.Connect(config); 

             IDatabase db = ssdb.GetDatabase();
             
             return db;
         }               
    }

    public SSDBConsoleHost(string ipinfo, int portinfo)
    {
        if (ipinfo == null || portinfo == 0)
        {
            try
            {
                
                Client client = new Client("127.0.0.1", 8888);
                this.SSDBConnection = client;                
            }
            catch(Exception ex)
            {
                this.SSDBConnection = null;
            }
        }
        else{
            try
            { 
                Client client = new Client(ipinfo.ToString(), portinfo);
                this.SSDBConnection = client;
                
            }
            catch(Exception ex)
            {
                this.SSDBConnection = null;
            }
        }
    }

    public void Dispose()
    {

        this.SSDBConnection.close();
        //ipinfo = "xxx.xxx.xxx.xxx";
        //portinfo = 10;
        //var T = this.SSDBConnection;
        //T = SSDBConnection;
    }
   
    public List<byte[]> _request(string cmd, string[] args, SSDBInfo l)
    {
        SSDBConsoleHost C = new SSDBConsoleHost(l.ipadd, int.Parse(l.portno));
        using (C)
        {

            try
            {
                return C.SSDBConnection.request(cmd, args);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

public class StringDetail : GetStringDetail
{
    public int KeyCount()
    {
        return 1;
    }    
    public async Task<string[]> AllKeys(SSDBInfo l)
    {
        SSDBConsoleHost C = new SSDBConsoleHost(l.ipadd, int.Parse(l.portno));
        using (C)
        {
            //C.DatabaseConnection().
            
            try
            {
                return await C.SSDBConnection.listAsync("", "", l.tablelength);
            }
            catch
            {
                return null;
            }
        }
    
    }
}

public class HashDetail : GetHashDetail
{
    public int KeyCount()
    {        
        return 0;
    }    
    public async Task<string[]> AllKeys(SSDBInfo l)
    {
        SSDBConsoleHost C = new SSDBConsoleHost(l.ipadd, int.Parse(l.portno));
        //C.DatabaseConnection().
        using (C)
        {
           
            try
            {
                return await C.SSDBConnection.hlistAsync("", "", l.tablelength);
            }
            catch
            {
                return null;
            }
        }
    }
}

public class SortedSetDetail : GetSortedSetDetail
{
    public int KeyCount()
    {
        return 0;
    }
    public async Task<string[]> AllKeys(SSDBInfo l)
    {
        SSDBConsoleHost C = new SSDBConsoleHost(l.ipadd, int.Parse(l.portno));
        //C.DatabaseConnection().
        using (C)
        {
            
            try
            {
                return await C.SSDBConnection.zlistAsync("", "", l.tablelength);
            }
            catch
            {
                return null;
            }
        }
    }
}

public class ListDetail : GetListDetail
{
    public int KeyCount()
    {

        return 0;
    }
    
    public async Task <List<byte[]>> AllValues(SSDBInfo l)
    {
        SSDBConsoleHost C = new SSDBConsoleHost(l.ipadd, int.Parse(l.portno));
        using (C)
        {
           
            string[] args = { "", "", l.tablelength.ToString() };

            try
            {
                return await C.SSDBConnection.requestAsync("qlist", args);
            }
            catch
            {
                return null;
            }
        }
    }
}

public class SSDBInfo
{
    public string ipadd { get; set; }
    public string portno { get; set; }
    public int tablelength { get; set; }
    
}
public class GetRequestArgs
{
    public string cmd { get; set; }
    public string[] parameters { get; set; }
}
public class GetKeyInfo
{
    public int datatype { get; set; }
    public string keyname { get; set; }
    public long[] size { get; set; }
    public string firstitem { get; set; }
    public string lastitem { get; set; }    
}
public class KeyDetails{
    public string [] Field { get; set; }
    public string [] Value { get; set; }
    public string [] Member { get; set; }
    public long [] Score { get; set; }
    public string Keyname { get; set; }
    public string Keyvalue { get; set; }
    public long Size { get; set; }
    public long FieldSize { get; set; }
    public long FieldSizeLeft { get; set; }
    public long FieldSizePast { get; set; }
}
public class KeyInfo
{
    public int StringCount { get; set; }
    public int HashCount { get; set; }    
    public int ListCount { get; set; }
    public int SortedSetCount { get; set; }    
    public List<string> StringList = new List<string>();
    public List<string> HashList = new List<string>();
    public List<string> ListKeys = new List<string>();
    public List<string> SortedSetList = new List<string>();
}
public class ServerInfoObj
{
    
}
public class ServerInfo
{
    public async Task<string[]> Info(SSDBConsoleHost C)
    {
        using (C)
        {
            if (C.SSDBConnection == null) { return null; }
            else
            {
                try
                {                            
                    //C.SSDBConnection.close();
                    return C.SSDBConnection.info();
                }//
                catch
                { return null; }
            }
        }
    }
    public async Task<KeyInfo> All(SSDBInfo l)
    {
        SSDBConsoleHost C = new SSDBConsoleHost(l.ipadd,int.Parse(l.portno));
        using (C)
        {
            StringDetail A = new StringDetail();
            SortedSetDetail B = new SortedSetDetail();
            HashDetail D = new HashDetail();
            

            List<string> G = new List<string>();
            KeyInfo H = new KeyInfo();
            int I = 0;

            string[] xx = await A.AllKeys(l).ConfigureAwait(false);
            foreach (string f in xx)
            {
                H.StringList.Add(f);
                I = I + 1;
            }
            H.StringCount = I;
            I = 0;

            xx = await B.AllKeys(l).ConfigureAwait(false);
            foreach (string f in xx)
            {
                H.SortedSetList.Add(f);
                I = I + 1;
            }
            H.SortedSetCount = I;
            I = 0;


            xx = await D.AllKeys(l).ConfigureAwait(false);
            //C.SSDBConnection.close();
            foreach (string f in xx)
            {
                H.HashList.Add(f);
                I = I + 1;
            }
            H.HashCount = I;
            I = 0;

            return H;
        }
    }
    public async Task<KeyDetails> GetKeyDetails(GetKeyInfo C, SSDBInfo l)
    {
        
            KeyValuePair<string, long>[] E = new KeyValuePair<string, long>[20];
            KeyValuePair<string, byte[]>[] F = new KeyValuePair<string, byte[]>[20];
            KeyDetails J = new KeyDetails();
            if (C.firstitem == null) { C.firstitem = String.Empty; }
            if (C.lastitem == null) { C.lastitem = String.Empty; }

            long g = 0;
            switch (C.datatype)
            {
                case 0:
                    string k = "";
                    try
                    {
                        using (SSDBConsoleHost D = new SSDBConsoleHost(l.ipadd, int.Parse(l.portno)))
                        {
                            k = await D.SSDBConnection.getAsync(C.keyname).ConfigureAwait(false);
                            D.SSDBConnection.close();
                        }
                    }
                    catch
                    {
                    }
                    J.Keyname = null; J.Keyvalue = null;
                    J.Keyname = C.keyname;
                    J.Keyvalue = k;
                    J.Size = C.size[0];
                    break;
                case 1:
                    try
                    {
                        using (SSDBConsoleHost D = new SSDBConsoleHost(l.ipadd, int.Parse(l.portno)))
                        {
                        g = await D.SSDBConnection.hsizeAsync(C.keyname).ConfigureAwait(false);
                        }

                    }
                    catch
                    {
                    }
                    if (g <= C.size[0])
                    {

                        J.Size = g;
                        F = new KeyValuePair<string, byte[]>[g];

                    }
                    else
                    {
                        J.Size = C.size[0];


                    }

                    if (C.firstitem == String.Empty)
                    {
                        try
                        {
                            //F = D.SSDBConnection.hscan(C.keyname, "", "", J.Size);          
                            using (SSDBConsoleHost D = new SSDBConsoleHost(l.ipadd, int.Parse(l.portno)))
                        {
                            F = await D.SSDBConnection.hscanAsync(C.keyname, C.lastitem, C.firstitem, J.Size).ConfigureAwait(false);
                            }
                        }
                        catch { }
                        int i = 0;
                        J.Field = new string[J.Size]; J.Value = new string[J.Size];
                        J.Keyname = C.keyname; J.FieldSize = g;
                        foreach (KeyValuePair<string, byte[]> h in F)
                        {
                            J.Field[i] = h.Key;
                            J.Value[i] = Encoding.UTF8.GetString(h.Value);
                            i = i + 1;
                        }

                    }
                    else
                    {
                        try
                        {
                            //F = D.SSDBConnection.hscan(C.keyname, "", "", J.Size);
                            using (SSDBConsoleHost D = new SSDBConsoleHost(l.ipadd, int.Parse(l.portno)))
                        {
                            F = await D.SSDBConnection.hrscanAsync(C.keyname, C.firstitem, C.lastitem, J.Size).ConfigureAwait(false);
                            }
                        }
                        catch { }
                        int i = F.Length - 1;
                        J.Field = new string[J.Size]; J.Value = new string[J.Size];
                        J.Keyname = C.keyname; J.FieldSize = g;


                        foreach (KeyValuePair<string, byte[]> h in F)
                        {
                            J.Field[i] = h.Key;
                            J.Value[i] = Encoding.UTF8.GetString(h.Value);
                            i = i - 1;
                        }

                    }
                    //D.SSDBConnection.close();

                    break;
                case 2:
                    string[] args = { C.keyname };
                    try
                    {
                        using (SSDBConsoleHost D = new SSDBConsoleHost(l.ipadd, int.Parse(l.portno)))
                        {
                        g = Convert.ToInt32(D.SSDBConnection.requestAsync("qsize", args).ConfigureAwait(false));
                        D.SSDBConnection.close();
                        }
                    }
                    catch { }
                    if (g <= C.size[0])
                    {
                        J.Size = g;

                    }
                    else
                    {
                        J.Size = C.size[0];
                    }

                    if (C.firstitem == String.Empty)
                    {
                        try
                        {


                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {


                        }
                        catch { }
                    }
                    J.Keyname = C.keyname;
                    break;
                case 3:
                    try
                    {
                        using (SSDBConsoleHost D = new SSDBConsoleHost(l.ipadd, int.Parse(l.portno)))
                        {
                        g = await D.SSDBConnection.zsizeAsync(C.keyname).ConfigureAwait(false);
                        D.SSDBConnection.close();
                        }
                    }
                    catch
                    { }
                    if (g <= C.size[0])
                    {
                        J.Size = g;
                    }
                    else
                    {
                        J.Size = C.size[0];
                    }
                    if (C.firstitem == String.Empty)
                    {
                        try
                        {
                            using (SSDBConsoleHost D = new SSDBConsoleHost(l.ipadd, int.Parse(l.portno)))
                        {
                            E = await D.SSDBConnection.zscanAsync(C.keyname, C.lastitem, Int64.MinValue, Int64.MaxValue, J.Size);
                            D.SSDBConnection.close();
                            }
                        }
                        catch { }
                        int j = 0;
                        J.Keyname = C.keyname; J.FieldSize = g;
                        J.Member = new string[J.Size]; J.Score = new long[J.Size];
                        foreach (KeyValuePair<string, long> h in E)
                        {
                            J.Member[j] = h.Key;
                            J.Score[j] = h.Value;
                            j = j + 1;
                        }
                    }
                    else
                    {
                        try
                        {
                            using (SSDBConsoleHost D = new SSDBConsoleHost(l.ipadd, int.Parse(l.portno)))
                        {
                            E = await D.SSDBConnection.zrscanAsync(C.keyname, C.firstitem, Int64.MaxValue, Int64.MinValue, J.Size).ConfigureAwait(false);
                            D.SSDBConnection.close();
                            }
                        }
                        catch { }
                        int j = E.Length - 1;
                        J.Keyname = C.keyname; J.FieldSize = g;
                        J.Member = new string[J.Size]; J.Score = new long[J.Size];
                        foreach (KeyValuePair<string, long> h in E)
                        {
                            J.Member[j] = h.Key;
                            J.Score[j] = h.Value;
                            j = j - 1;
                        }
                    }
                    break;
           

            }

            return J;
        
    }
}