// See https://aka.ms/new-console-template for more information

using SnmpDotNet;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestSnmp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static string deviceIp = "";
        public static ushort port = 161;
        public static string community = "";
        public static async Task Main(string[] args)
        {
            var value = "\b\u0002\u0001\u0000";
            var y = string.Join(".", Encoding.UTF8.GetBytes(value));

            //Console.WriteLine(Regex.Match("", @"(\d+\/\d+((\/|.)\d+)*)").Groups[1].Value);

            await DoTest(TestGetV1);
            //await DoTest(TestGetV2);
            await DoTest(TestGetSubtreeV1);
            await DoTest(TestGetSubtreeV2);
            //await DoTest(TestCounter32);
            //await DoTest(TestCounter64);
            //await DoTest(TestGauge32);
            //await DoTest(TestInteger32);
            ////await DoTest(TestIpAddress);
            ////await DoTest(TestNull);
            //await DoTest(TestOctetString);
            //await DoTest(TestOid);
            //await DoTest(TestTimeTicks);
            ////await DoTest(TestUnsigned32);
        }

        public static async Task TestGetV1()
        {
            Console.WriteLine("Test Get v1");
            var result = await Snmp.GetOneAsync(SnmpVersion.V1, deviceIp, 161, community, TimeSpan.FromSeconds(5), 1, "1.3.6.1.2.1.1.2.0");
            if (result.Value.Tag != SnmpTag.Oid || result.Value.ToOid().Value != "1.3.6.1.4.1.9.1.1017") throw new Exception();
        }
        public static async Task TestGetV2()
        {
            Console.WriteLine("Test Get v2");
            var result = await Snmp.GetOneAsync(SnmpVersion.V2c, deviceIp, 161, community, TimeSpan.FromSeconds(5), 2, "1.3.6.1.2.1.1.2.0");
            if (result.Value.Tag != SnmpTag.Oid || result.Value.ToOid().Value != "1.3.6.1.4.1.9.1.1017") throw new Exception();
        }

        public static async Task TestGetSubtreeV1()
        {
            Console.WriteLine("Test GetSubtree v1");
            var result = await Snmp.GetSubtreeAsync(SnmpVersion.V1, deviceIp, 161, community, TimeSpan.FromSeconds(5), 2, 10, "1.3.6.1.2.1.1");
            if (result.Count != 7) throw new Exception();
        }

        public static async Task TestGetSubtreeV2()
        {
            Console.WriteLine("Test GetSubtree v2");
            var result = await Snmp.GetSubtreeAsync(SnmpVersion.V2c, deviceIp, 161, community, TimeSpan.FromSeconds(5), 2, 10, "1.3.6.1.2.1.1");
            if (result.Count != 7) throw new Exception();
        }
        public static async Task TestOctetString()
        {
            Console.WriteLine("Test octet string");
            var result = await Snmp.GetOneAsync(SnmpVersion.V2c, deviceIp, 161, community, TimeSpan.FromSeconds(5), 2, "1.3.6.1.2.1.1.5.0");

            if (result.Value.Tag != SnmpTag.OctetString || result.Value.ToOctetString().Value != "ASR_PYN03SCU") throw new Exception();
        }
        public static async Task TestTimeTicks()
        {
            Console.WriteLine("Test timeticks");
            var result = await Snmp.GetOneAsync(SnmpVersion.V2c, deviceIp, 161, community, TimeSpan.FromSeconds(5), 2, "1.3.6.1.2.1.1.3.0");

            if (result.Value.Tag != SnmpTag.TimeTicks || result.Value.ToTimeTicks().Value <= 0) throw new Exception();
        }
        public static async Task TestInteger32()
        {
            Console.WriteLine("Test Integer32");
            var result = await Snmp.GetOneAsync(SnmpVersion.V2c, deviceIp, 161, community, TimeSpan.FromSeconds(5), 2, "1.3.6.1.2.1.1.7.0");

            if (result.Value.Tag != SnmpTag.Integer32 || result.Value.ToInteger32().Value != 78) throw new Exception();
        }
        public static async Task TestGauge32()
        {
            Console.WriteLine("Test Gauge32");
            var result = await Snmp.GetOneAsync(SnmpVersion.V2c, deviceIp, 161, community, TimeSpan.FromSeconds(5), 2, "1.3.6.1.2.1.31.1.1.1.15.578");

            if (result.Value.Tag != SnmpTag.Gauge32 || result.Value.ToGauge32().Value != 10000) throw new Exception();
        }
        public static async Task TestCounter64()
        {
            Console.WriteLine("Test Counter64");
            var result = await Snmp.GetOneAsync(SnmpVersion.V2c, deviceIp, 161, community, TimeSpan.FromSeconds(5), 2, "1.3.6.1.2.1.31.1.1.1.6.578");

            if (result.Value.Tag != SnmpTag.Counter64 || result.Value.ToCounter64().Value <= 0) throw new Exception();
        }
        public static async Task TestCounter32()
        {
            Console.WriteLine("Test Counter32");
            var result = await Snmp.GetOneAsync(SnmpVersion.V2c, deviceIp, 161, community, TimeSpan.FromSeconds(5), 2, "1.3.6.1.2.1.2.2.1.10.578");

            if (result.Value.Tag != SnmpTag.Counter32 || result.Value.ToCounter32().Value <= 0) throw new Exception();
        }
        public static async Task TestOid()
        {
            Console.WriteLine("Test Oid");
            var result = await Snmp.GetOneAsync(SnmpVersion.V1, deviceIp, 161, community, TimeSpan.FromSeconds(5), 2, "1.3.6.1.2.1.1.2.0");
            if (result.Value.Tag != SnmpTag.Oid || result.Value.ToOid().Value != "1.3.6.1.4.1.9.1.1017") throw new Exception();
        }
        public static async Task DoTest(Func<Task> action)
        {
            try
            {
                await action();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Passed");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}