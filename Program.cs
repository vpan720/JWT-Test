using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Security;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


namespace JWT_Test
{
    class Program
    {

        
        static void Main(string[] args)
        {
            var secret = "dcppaymentgateway";
            var secbyte = Encoding.UTF8.GetBytes(secret);
            var enc = Convert.ToBase64String(secbyte);
            var seclen = enc.Length;
            var encsecret = enc.Substring(0, seclen - 1);

            var header = "{\"alg\":\"HS256\",\"typ\":\"JWT\"}";

            var jsonheader = JObject.Parse(header);
            var headerstring = jsonheader.ToString();
            //Console.WriteLine(jsonheader+"\n\n"+headerstring+"\n\n");
            var decodedHeader = Encoding.UTF8.GetBytes(headerstring);
            var encodedHeader = Convert.ToBase64String(decodedHeader);

            var pid = "5f0c5e88ab44e23e7c495878";
            var oid = "5f0c5e874b1e3d3b1ea63db9";

            DateTimeOffset testiat = DateTimeOffset.UtcNow;
            long iat = testiat.ToUnixTimeSeconds();
            DateTimeOffset testexp = DateTimeOffset.UtcNow.AddHours(3);
            long exp = testexp.ToUnixTimeSeconds();
            Console.WriteLine(exp + "\n\n" + iat + "\n\n");

            var payload = "{\"paymentDetails\":{\"paymentId\":\"\",\"orderId\":\"\",\"status\":\"SUCCESSFUL\"},\"exp\":1594657891,\"iat\":1516239022}";
            var jsonpayload = JObject.Parse(payload);
            jsonpayload["iat"] = iat;
            jsonpayload["exp"] = exp;
            var orderDetails = jsonpayload["paymentDetails"];
            orderDetails["paymentId"] = pid;
            orderDetails["orderId"] = oid;
            var jsonstring = jsonpayload.ToString();
            Console.WriteLine(jsonpayload + "\n\n" + jsonstring + "\n\n");
            var decodedPayload = Encoding.UTF8.GetBytes(jsonstring);
            var encodedPayload = Convert.ToBase64String(decodedPayload);
            var datalen = encodedPayload.Length;
            var encpayload = encodedPayload.Substring(0, datalen - 1);

            var token = encodedHeader + "." + encpayload;
            var signature = "";
            var encoding = new ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(token);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                signature = Convert.ToBase64String(hashmessage);
            }

            var signedToken = token + "." + signature;

            Console.WriteLine(signedToken);

            Console.ReadLine();
            Console.ReadKey();




            //var jwttoken = "eyJhbGciOiJIUzI1NiJ9.eyJwYXltZW50SWQiOiI1ZjBjNWU4OGFiNDRlMjNlN2M0OTU4NzgiLCJjdXN0b21lckRldGFpbHMiOnsiZmlyc3ROYW1lIjoiY3NKb2huMSIsImxhc3ROYW1lIjoiY3NEb2UxIiwidXNlcklkIjoiNWVmNGI1OGZkYjUyZjk1ZGY4NWYwMjg0In0sIm9yZGVyRGV0YWlscyI6eyJvcmRlcklkIjoiNWYwYzVlODc0YjFlM2QzYjFlYTYzZGI5IiwiY3VycmVuY3kiOm51bGwsImFtb3VudCI6IjU0LjAwIn0sIm1lcmNoYW50RGV0YWlscyI6eyJuYW1lIjoiRENQIn0sImlhdCI6MTU5NDY0NjE1MiwiZXhwIjoxNTk0NjQ2NzUyfQ.dEe0EASnRobQuPnGcvM163DVQcEoVPlaiq1oKmghQ5Q";

            //var header = jwttoken.Substring(0, jwttoken.IndexOf("."));
            //Console.WriteLine(header);
            //byte[] base64EncodedBytes = Convert.FromBase64String(header);
            //var jsonHeader = Encoding.UTF8.GetString(base64EncodedBytes);
            //Console.WriteLine(jsonHeader + "\n");

            //var data = jwttoken.Substring(jwttoken.IndexOf(".") + 1);
            //var Maindata = data.Substring(0, data.IndexOf("."));
            //Console.WriteLine(data + "\n\n" + Maindata);

            //if (Maindata.Length % 4 != 0)
            //    Maindata += new String('=', 4 - Maindata.Length % 4);

            //byte[] base64EncodedBytes2 = Convert.FromBase64String(Maindata);
            //var jsonData = Encoding.UTF8.GetString(base64EncodedBytes2);
            //Console.WriteLine("\n\n" + jsonData + "\n\n");

            //var details = JObject.Parse(jsonData);
            //var paymentId = details["paymentId"];
            //var orderDetails = details["orderDetails"];
            //var orderId = orderDetails["orderId"];
            //Console.WriteLine(string.Concat("paymentID ", paymentId, "\norderID " + orderId));

            //Console.ReadLine();

            //Console.ReadKey();
        }
    }
}
