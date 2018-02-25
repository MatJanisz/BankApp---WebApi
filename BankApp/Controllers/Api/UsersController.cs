using BankApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BankApp.Controllers.Api
{
    public class UsersController : ApiController
    {
        private ApplicationDbContext _context;

        public UsersController()
        {
            _context = new ApplicationDbContext();
        }

        //domyślny routing dla api to nazwakontrolera/id
        //w przeciwieństwie do mvc gdzie: nazwakontrolera/akcja/parametry


        //GET
        //Api/Users
        [HttpGet]
        public IEnumerable<UserAccount> GetUserAccounts()
        {
            return _context.UserAccount.ToList();
        }

        //GET 
        //Api/Users/number
        [HttpGet]
        public IHttpActionResult GetUserAccount(int id)
        {
            var user = _context.UserAccount.SingleOrDefault(c => c.Id == id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        //POST Api/Users
        //konwencja z IHttpActionResult zwraca typ 201, czyli Creted, w przeciw do poprz wersji
        [HttpPost]
        public IHttpActionResult CreateUserAccount(UserAccount userAccount)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _context.UserAccount.Add(userAccount);
            _context.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + userAccount.Id), userAccount);
        }

        //PUT /Api/Users/1
        [HttpPut]
        public IHttpActionResult UpdateUserAccount(int id, UserAccount userAccount)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var userInDb = _context.UserAccount.SingleOrDefault(c => c.Id == id);

            if (userInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            userInDb.Name = userAccount.Name;
            userInDb.Surname = userAccount.Surname;
            userInDb.DateOfBirth = userAccount.DateOfBirth;

            _context.SaveChanges();

            return Ok();
        }

        //DELETE Api/User/1
        [HttpDelete]
        public IHttpActionResult DeleteUserAccount(int id)
        {
            var userInDb = _context.UserAccount.SingleOrDefault(c => c.Id == id);

            if (userInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.UserAccount.Remove(userInDb);
            _context.SaveChanges();

            return Ok();

        }
    }
}
