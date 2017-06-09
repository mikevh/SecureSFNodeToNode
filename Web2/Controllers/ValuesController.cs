using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Web2.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var rv = new List<string> {"value21", "value22"};

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) =>
                {
                    var cert = FindCertificateByThumbprint(certificate.GetCertHashString());

                    return cert != null;
                };
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync("https://localhost:8456/api/values");
                    var content = await result.Content.ReadAsStringAsync();

                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(content);
                    rv.AddRange(data);
                }
            }
            catch (Exception ex)
            {
                rv.Add(ex.Message);
            }

            return rv;
        }

        public static X509Certificate2 FindCertificateByThumbprint(string certificateThumbprint)
        {
            if (certificateThumbprint == null)
            {
                throw new ArgumentNullException(nameof(certificateThumbprint));
            }

            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            try
            {
                store.Open(OpenFlags.ReadOnly);

                var col = store.Certificates.Find(X509FindType.FindByThumbprint, certificateThumbprint, false); // Don't validate certs, since the test root isn't installed.
                if (col == null || col.Count == 0)
                {
                    throw new Exception($"Could not find the certificate with thumbprint {certificateThumbprint} in the Local Machine's Personal certificate store.");
                }

                return col[0];
            }
            finally
            {
                store.Close();
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
