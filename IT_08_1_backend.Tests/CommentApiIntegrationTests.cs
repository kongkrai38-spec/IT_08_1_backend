using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using IT_08_1_backend.Models;

namespace IT_08_1_backend.Tests
{
    public class CommentApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CommentApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetComments_ReturnsSuccessStatusCode()
        {
            // Act
            var response = await _client.GetAsync("/api/comment");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetComments_ReturnsJsonArray()
        {
            // Act
            var response = await _client.GetAsync("/api/comment");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
            content.Should().StartWith("[");
        }

        [Fact]
        public async Task PostComment_ReturnsCreatedComment()
        {
            // Arrange
            var comment = new Comment
            {
                UserName = "Integration Test User",
                Message = "Integration Test Message"
            };

            var json = JsonSerializer.Serialize(comment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/comment", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var returnedComment = await response.Content.ReadFromJsonAsync<Comment>();
            returnedComment.Should().NotBeNull();
            returnedComment!.UserName.Should().Be("Integration Test User");
            returnedComment.Message.Should().Be("Integration Test Message");
            returnedComment.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(2));
        }

        [Fact]
        public async Task PostComment_WithEmptyBody_ReturnsBadRequest()
        {
            // Arrange
            var content = new StringContent("", Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/comment", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteAllComments_ReturnsNoContent()
        {
            // Act
            var response = await _client.DeleteAsync("/api/comment");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task PostAndGetComments_ReturnsPostedComment()
        {
            // Arrange - Clear all comments first
            await _client.DeleteAsync("/api/comment");

            var comment = new Comment
            {
                UserName = "Test Flow User",
                Message = "Test Flow Message"
            };

            var json = JsonSerializer.Serialize(comment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act - Post a comment
            await _client.PostAsync("/api/comment", content);

            // Act - Get all comments
            var getResponse = await _client.GetAsync("/api/comment");
            var comments = await getResponse.Content.ReadFromJsonAsync<List<Comment>>();

            // Assert
            comments.Should().NotBeNull();
            comments.Should().HaveCountGreaterOrEqualTo(1);
            comments!.Should().Contain(c => c.UserName == "Test Flow User");
        }
    }
}