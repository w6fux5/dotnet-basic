using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using testapi.Data;
using testapi.Model;

namespace testapi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
    DataContextEF _entityFramework;
    IUserRepository _userRepository;
    IMapper _mapper;

    public UserEFController(IConfiguration config, IUserRepository userRepository)
    {
        _entityFramework = new DataContextEF(config);

        _userRepository = userRepository;

        _mapper = new Mapper(
            new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserToAddDto, User>();
            })
        );
    }

    [HttpGet]
    public IEnumerable<User> GetUsers()
    {
        return _userRepository.GetUsers();
    }

    [HttpGet("{userId}")]
    public User GetSingleUser(int userId)
    {
        return _userRepository.GetSingleUser(userId);
    }

    [HttpPut]
    public IActionResult EditUser(User user)
    {
        User? userDb = _userRepository.GetSingleUser(user.UserId);

        if (userDb != null)
        {
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            userDb.Active = user.Active;

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed to update user");
        }

        throw new Exception("Failed to update user");
    }

    [HttpPost]
    public IActionResult AddUser(UserToAddDto user)
    {
        User userDb = _mapper.Map<User>(user);

        _userRepository.AddEntity<User>(userDb);

        if (_userRepository.SaveChanges())
        {
            return Ok();
        }

        throw new Exception("Failed to create user");
    }

    [HttpDelete("{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _userRepository.GetSingleUser(userId);

        if (userDb != null)
        {
            _userRepository.RemoveEntity<User>(userDb);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed to delete user");
        }

        throw new Exception("Failed to Get user");
    }
}
