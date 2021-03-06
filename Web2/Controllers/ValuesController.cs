﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync("https://localhost:9061/api/values");
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
