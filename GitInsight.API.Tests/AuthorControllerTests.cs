using Microsoft.Win32;
namespace bdsaproject.Tests;

public class AuthorControllerTests {

    private readonly AuthorController controller;

    public AuthorControllerTests() {
        controller = new AuthorController();
    }

    // [Theory]
    // [InlineData("whistle-dev", "bdsaproject", "unknown")]
    // public void GetReturnsError(string username, string reponame, string author) {
    //     controller.Get(username, reponame, author).Result.Should().BeAssignableTo<NotFoundResult>();
    // }

    [Theory]
    [InlineData("whistle-dev", "bdsaproject", "yovitus")]
    public void GetReturnsResult(string username, string reponame, string author)  {

        // Arrange
        // var authorResult = "the result";

        // Act
        var response = controller.Get(username, reponame, author);
        
        // Assert
        // response.Should().BeEquivalentTo(authorResult);

        response.Should().BeOfType<OkObjectResult>();
    }

}