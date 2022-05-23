using StackExchange.Redis;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using ssdb;
public class SSDBConsoleHost:ConnectionManager
{
    public string ipinfo { get; set; }
    public int portinfo { get; set; } 

    public IDatabaseAsync DatabaseConnection()
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

    public Client U()
    {
        if (ipinfo == null || portinfo == 0)
        {
            try
            {

                Client client = new Client("127.0.0.1", 8888);
                return client;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        else{
            try
            { 
                Client client = new Client(ipinfo.ToString(), portinfo);
                return client;  
            }
            catch(Exception ex)
            {

                return null;
            }
        }
    }

    public List<byte[]> _request(string cmd, string[] args)
    {
        SSDBConsoleHost C = new SSDBConsoleHost();        
         try{
             return C.U().request(cmd, args);
        }
        catch(Exception ex)
        {
            return null;
        }
    }
}

public class StringDetail : GetStringDetail
{
    public int KeyCount()
    {
        return 1;
    }    
    public string[] AllKeys(SSDBInfo l)
    {
        SSDBConsoleHost C = new SSDBConsoleHost();
        //C.DatabaseConnection().
        C.ipinfo = l.ipadd;
        C.portinfo = int.Parse(l.portno);
        try{
            return C.U().list("", "", l.tablelength);
        }
        catch
        {
            return null;
        }
        ;
    }
}

public class HashDetail : GetHashDetail
{
    public int KeyCount()
    {        
        return 0;
    }    
    public string[] AllKeys(SSDBInfo l)
    {
        SSDBConsoleHost C = new SSDBConsoleHost();
        //C.DatabaseConnection().
        C.ipinfo = l.ipadd;
        C.portinfo = int.Parse(l.portno);
        try
        {
            return C.U().hlist("", "", l.tablelength);
        }
        catch
        {
            return null;
        }
    }
}

public class SortedSetDetail : GetSortedSetDetail
{
    public int KeyCount()
    {
        return 0;
    }
    public string[] AllKeys(SSDBInfo l)
    {
        SSDBConsoleHost C = new SSDBConsoleHost();
        //C.DatabaseConnection().
        C.ipinfo = l.ipadd;
        C.portinfo = int.Parse(l.portno);
        try
        {
            return C.U().zlist("", "", l.tablelength);
        }
        catch
        {
            return null;
        }
    }
}

public class ListDetail : GetListDetail
{
    public int KeyCount()
    {

        return 0;
    }
    
    public List<byte[]> AllValues(SSDBInfo l)
    {
        SSDBConsoleHost C = new SSDBConsoleHost();
        C.ipinfo = l.ipadd;
        C.portinfo = int.Parse(l.portno);
        string []args = {"", "", l.tablelength.ToString()};        
        
        try
        {
             return C._request("qlist", args); 
        }
        catch
        {
        return null;
        }
    }
}

public class SSDBInfo
{
    public string ipadd { get; set; }
    public string portno { get; set; }
    public int tablelength { get; set; }
    
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
    public string[] Info(SSDBConsoleHost C)
    {
        
        if (C.U() == null) { return null; }
        else
        {
            try { return C.U().info(); }
            catch
            { return null; }
        }
    }
    public KeyInfo All(SSDBInfo l)
    {
        SSDBConsoleHost C = new SSDBConsoleHost();
        StringDetail A = new StringDetail();
        SortedSetDetail B = new SortedSetDetail();
        HashDetail D = new HashDetail();
        C.ipinfo = l.ipadd;
        C.portinfo = int.Parse(l.portno);

        List<string> G = new List<string>();
        KeyInfo H = new KeyInfo();
        int I = 0;
        
        foreach (string f in  A.AllKeys(l))
        {            
            H.StringList.Add(f);
            I = I + 1;
        }
        H.StringCount = I;
        I = 0;
        

        foreach (string f in B.AllKeys(l))
        {            
            H.SortedSetList.Add(f);
            I = I + 1;
        }
        H.SortedSetCount = I;
        I = 0;       

        foreach (string f in D.AllKeys(l))
        {            
            H.HashList.Add(f);
            I = I + 1;
        }
        H.HashCount = I;
        I = 0;

        return H;    
    }
    public KeyDetails GetKeyDetails(GetKeyInfo C)
    {
        SSDBConsoleHost D = new SSDBConsoleHost();
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
                    D.U().get(C.keyname, out k);
                }
                catch { 
                }
                J.Keyname = null; J.Keyvalue = null;
                J.Keyname =  C.keyname;
                J.Keyvalue = k;
                J.Size = C.size[0];
                break;
            case 1:
                try{
                g = D.U().hsize(C.keyname);
                }
                catch{
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
                        //F = D.U().hscan(C.keyname, "", "", J.Size);                        
                        F = D.U().hscan(C.keyname, C.lastitem, C.firstitem, J.Size);
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
                        //F = D.U().hscan(C.keyname, "", "", J.Size);
                        F = D.U().hrscan(C.keyname, C.firstitem, C.lastitem, J.Size);
                    }
                    catch { }
                    int i = F.Length -1;
                    J.Field = new string[J.Size]; J.Value = new string[J.Size];
                    J.Keyname = C.keyname; J.FieldSize = g; 
                    

                    foreach (KeyValuePair<string, byte[]> h in F)
                    {
                        J.Field[i] = h.Key;
                        J.Value[i] = Encoding.UTF8.GetString(h.Value);
                        i = i - 1;
                    }
                
                }
                
               
                break;
            case 2:
                string[]args = { C.keyname };
                try{
                g = Convert.ToInt32(D._request("qsize", args));
                }
                catch{}
                if (g<= C.size[0])
                {
                    J.Size = g;

                }
                else {
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
                try{
                g = D.U().zsize(C.keyname);
                }
                catch
                {}
                if (g<= C.size[0])
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

                        E = D.U().zscan(C.keyname, C.lastitem, Int64.MinValue, Int64.MaxValue, J.Size);
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

                        E = D.U().zrscan(C.keyname, C.firstitem, Int64.MaxValue, Int64.MinValue, J.Size);
                    }
                    catch { }
                    int j = E.Length -1;
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