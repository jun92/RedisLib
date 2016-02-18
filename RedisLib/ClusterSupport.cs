using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace Syncnet
{ 
namespace RedisLib
{
    public class RedisClusterSupport : RedisObject
    {
        private const String config_filename = @"C:\RedisLib\RedisLib-cluster.configuration.xml";
        private RedisClusterInfo rci;
        public RedisClusterSupport(RedisAsyncConnManager conn) : base(conn)
        {

        }
        public REDIS_RESPONSE_TYPE clusterslots()
        {
            RESPMaker m = new RESPMaker();

            m.Add("CLUSTER");
            m.Add("SLOTS");

            return Process(m);
        }
        public REDIS_RESPONSE_TYPE clusternodes()
        {
            RESPMaker m = new RESPMaker();
            m.Add("CLUSTER");
            m.Add("NODES");
            return Process(m);
        }

        public void ConstructClusterConfigInfo()
        {
            if (System.IO.File.Exists(config_filename)) LoadClusterConfigInfo();
            else CreateClusterConfigInfo();            
        }
        public void LoadClusterConfigInfo()
        {            
            rci = new RedisClusterInfo();
            XmlSerializer xs = new XmlSerializer(typeof(RedisClusterInfo));
            StreamReader sr = new StreamReader(config_filename);
            rci = (RedisClusterInfo)xs.Deserialize(sr);            
        }
        public void CreateClusterConfigInfo()
        {
            List<dynamic> narray = new List<dynamic>();
            if (clusterslots() == REDIS_RESPONSE_TYPE.ERROR)
            {
                return;
            }
            getNestedArray(ref narray);
            rci = new RedisClusterInfo();
            for (int i = 0; i < narray.Count; i++)
            {
                RedisClusterNode rcn = new RedisClusterNode();
                rcn.lowHashSlot = narray[i][0];
                rcn.highHashSlot = narray[i][1];
                for (int j = 2; j < narray[i].Count; j++)
                {
                    RedisNodeConnInfo rnc = new RedisNodeConnInfo();
                    rnc.server_ip = narray[i][j][0];
                    rnc.server_port = narray[i][j][1];
                    rcn.ServerConnInfo.Add(rnc);
                }
                rci.Add(rcn);
            }
            System.IO.Directory.CreateDirectory(@"C:\RedisLib");
            XmlSerializer xs = new XmlSerializer(typeof(RedisClusterInfo));
            StreamWriter wr = new StreamWriter(config_filename);
            xs.Serialize(wr, rci);

        }
        public int getHashslot(String key)
        {
            RedisHashslot h = new RedisHashslot();
            return h.getHashslot(key);            
        }

        public void ReconnAccordingToKey(String key)
        {

        }
        public bool FindProperServer(int hashslot, ref List<String> ip, ref List<int> port)
        {
            foreach( RedisClusterNode node in rci.info  )
            {
                if( node.lowHashSlot <= hashslot && hashslot <= node.highHashSlot )
                {
                    foreach( RedisNodeConnInfo info in node.ServerConnInfo)
                    {
                        ip.Add(info.server_ip);
                        port.Add(info.server_port);
                    }
                    return true;
                }
            }
            return false; 
        }
    }
}
}
