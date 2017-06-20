using Xunit;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SocialMedia.Objects
{
  [Collection("SocialMedia")]

  public class UserTest : IDisposable
  {
    public UserTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=social_media_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void User_GetAll_DatabaseEmptyOnload()
    {
      List<User> testList = User.GetAll();

      List<User> controlList = new List<User>{};

      Assert.Equal(controlList, testList);
    }

    [Fact]
    public void User_Save_SavesUserToDatabase()
    {
      User controlUser = new User("Joshua", "Fairchild", "jfairchild", "password", "mail@mail.com", new DateTime(2017, 06, 19));
      controlUser.Save();

      User testUser = User.GetAll()[0];

      Assert.Equal(controlUser, testUser);
    }

    [Fact]
    public void User_Equals_ObjectsEqualsIdenticalObject()
    {
      User user1 = new User("Joshua", "Fairchild", "jfairchild", "password", "mail@mail.com", new DateTime(2017, 06, 19));
      User user2 = new User("Joshua", "Fairchild", "jfairchild", "password", "mail@mail.com", new DateTime(2017, 06, 19));

      Assert.Equal(user1, user2);
    }

    [Fact]
    public void User_Find_FindsUserById()
    {
      User controlUser = new User("Joshua", "Fairchild", "jfairchild", "password", "mail@mail.com", new DateTime(2017, 06, 19));
      controlUser.Save();

      User testUser = User.Find(controlUser.Id);

      Assert.Equal(controlUser, testUser);
    }

    [Fact]
    public void User_Delete_DeletesIndividualUser()
    {
      User user1 = new User("Joshua", "Fairchild", "jfairchild", "password", "mail@mail.com", new DateTime(2017, 06, 19));
      user1.Save();
      User user2 = new User("Jennifer", "Fairchild", "jenfairchild", "password", "mail@mail.com", new DateTime(2017, 06, 19));
      user2.Save();

      user2.Delete();

      List<User> testList = User.GetAll();
      List<User> controlList = new List<User>{user1};

      Assert.Equal(controlList, testList);
    }

    [Fact]
    public void User_UsernameTaken_ReturnsFalse()
    {
      User user1 = new User("Joshua", "Fairchild", "jfairchild", "password", "mail@mail.com", new DateTime(2017, 06, 19));
      user1.Save();

      User user2 = new User("Joshua", "Fairchild", "fairchild1", "password", "mail@mail.com", new DateTime(2017, 06, 20));

      user2.UsernameTaken();
      Assert.Equal(false, user2.UsernameTaken());
    }

    [Fact]
    public void User_UsernameTaken_ReturnsTrue()
    {
      User user1 = new User("Joshua", "Fairchild", "jfairchild", "password", "mail@mail.com", new DateTime(2017, 06, 19));
      user1.Save();

      User user2 = new User("Joshua", "Fairchild", "jfairchild", "password", "mail@mail.com", new DateTime(2017, 06, 20));

      user2.UsernameTaken();
      Assert.Equal(true, user2.UsernameTaken());
    }

    [Fact]
    public void User_GetPosts_ReturnsUsersPosts()
    {
      User newUser = new User("Joshua", "Fairchild", "jfairchild", "password", "mail@mail.com", new DateTime(2017, 06, 19));
      newUser.Save();

      Post post1 = new Post("Hello world", newUser.Id, new DateTime(2017, 06, 19));
      post1.Save();
      Post post2 = new Post("Goodbye world", newUser.Id, new DateTime(2017, 06, 19));
      post2.Save();

      List<Post> testList = newUser.GetPosts();
      List<Post> controlList = new List<Post>{post1, post2};

      Assert.Equal(testList, controlList);
    }

    public void Dispose()
    {
      User.DeleteAll();
    }
  }
}