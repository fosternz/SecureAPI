﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureAPI.Constants;

namespace SecureAPI.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class UsersController : ControllerBase
{
    public IConfiguration _config { get; }

    public UsersController(IConfiguration config)
    {
        _config = config;

    }

    // GET: api/<UsersController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<UsersController>/5
    [HttpGet("{id}")]
    //[Authorize]
    [Authorize(Policy = PolicyConstants.MustHaveStaffId)]
    public string Get(int id)
    {
        return _config.GetConnectionString("Default");

    }

    // POST api/<UsersController>
    [HttpPost]
    [Authorize(Policy = PolicyConstants.MustHaveStaffId)]
    [Authorize(Policy = PolicyConstants.MustBeEngineer)]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<UsersController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<UsersController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
