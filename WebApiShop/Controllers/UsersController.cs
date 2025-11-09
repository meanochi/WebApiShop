using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        string filePath = "..\\file1.txt";

        // GET: api/<UsersController>
        [HttpGet]
        public string Get()
        {
            return "value";
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult<UserClass> Get(int id)
        {
            using (StreamReader reader = System.IO.File.OpenText(filePath))
            {
                string? currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    UserClass user = JsonSerializer.Deserialize<UserClass>(currentUserInFile);
                    if (user.UserID == id)
                        return Ok(user);
                }
            }
            return NoContent();

        }
        
        // POST api/<UsersController>
        [HttpPost]
        public ActionResult <UserClass> POST([FromBody] UserClass newUser)
        {
            int numberOfUsers = System.IO.File.ReadLines(filePath).Count();
            newUser.UserID = numberOfUsers + 1;
            string userJson = JsonSerializer.Serialize(newUser);
            System.IO.File.AppendAllText(filePath, userJson + Environment.NewLine);
            return CreatedAtAction(nameof(Get), new {newUser.UserID }, newUser);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id,[FromBody] UserClass userToUpdate)
        {
            string textToReplace = string.Empty;
            using (StreamReader reader = System.IO.File.OpenText(filePath))
            {
                string currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {

                    UserClass user = JsonSerializer.Deserialize<UserClass>(currentUserInFile);
                    if (user.UserID == id)
                        textToReplace = currentUserInFile;
                }
            }

            if (textToReplace != string.Empty)
            {
                string text = System.IO.File.ReadAllText(filePath);
                text = text.Replace(textToReplace, JsonSerializer.Serialize(userToUpdate));
                System.IO.File.WriteAllText(filePath, text);
            }

        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
