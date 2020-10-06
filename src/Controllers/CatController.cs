﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CatApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CatApi.Controllers
{
    public class CatController : Controller
    {
        private static List<Cat> cats;

        static CatController()
        {
            using (FileStream fs = new FileStream("cats.json", FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            {
                var file = sr.ReadToEnd();
                cats = JsonSerializer.Deserialize<List<Cat>>(file);
            }
        }

        // GET api/values
        [HttpGet("api/cats")]
        public IEnumerable<Cat> Get([FromQuery] bool rand = false)
        {
            if (!rand)
                return cats.OrderByDescending(x => x.Loves).ThenByDescending(x => x.Hates);
            else
                return cats.OrderBy(x => Guid.NewGuid());
        }

        // GET api/values/5
        [HttpGet("api/cat/{id}")]
        public IActionResult Get(string id)
        {
            var result = cats.FirstOrDefault(x => x.Id == id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        // PUT api/values/5
        [Authorize]
        [HttpPut("api/love/{id}")]
        public IActionResult Love(string id)
        {
            var result = cats.FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                result.Loves++;
                return Ok();
            }
        }

        [Authorize]
        [HttpPut("api/hate/{id}")]
        public IActionResult Hate(string id)
        {
            var result = cats.FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                result.Hates++;
                return Ok();
            }
        }
    }
}