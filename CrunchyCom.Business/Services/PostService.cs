using CrunchyCom.Data.Models;
using CrunchyCom.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace CrunchyCom.Business.Services;

public class PostService : IPostService
{
    private readonly ILogger<PostService> _logger;
    private readonly IRepository<Post> _postRepository;

    public PostService(IRepository<Post> postRepository, ILogger<PostService> logger)
    {
        _postRepository = postRepository;
        _logger = logger;
    }

    /// Retrieves all posts from the data repository.
    /// Logs the process of retrieving posts and handles any exceptions
    /// that may occur during the operation.
    /// <returns>
    ///     A collection of all posts available in the data repository as an IEnumerable of Post objects.
    ///     If no posts are available, an empty collection is returned.
    /// </returns>
    public IEnumerable<Post> GetAllPosts() //=> _postRepository.GetAll();
    {
        _logger.LogInformation("Retrieving all posts");

        try
        {
            var posts = _postRepository.GetAll();
            _logger.LogInformation($"Retrieved {posts.Count()} posts");
            return posts;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving ALL posts");
            throw;
        }
    }

    /// Retrieves a specific post by its unique identifier.
    /// Logs the process of retrieving the specified post and handles any exceptions
    /// that may occur during the operation.
    /// <param name="id">
    ///     The unique identifier of the post to retrieve.
    /// </param>
    /// <returns>
    ///     The post object corresponding to the specified identifier, or null if no post
    ///     with the given identifier exists in the repository.
    /// </returns>
    public Post? GetPostById(string id) //=> _postRepository.GetById(id);
    {
        _logger.LogInformation("Retrieving all posts");

        try
        {
            var post = _postRepository.GetById(id);
            _logger.LogInformation($"Retrieved post with ID: {id}");
            return post;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving ALL posts");
            throw;
        }
    }

    /// Creates a new post in the data repository.
    /// Logs the creation process and handles any exceptions that may occur during the operation.
    /// <param name="post">
    ///     The post object containing details such as title, description, body, author, tags, and timestamps.
    /// </param>
    public void CreatePost(Post post)
    {
        _logger.LogInformation("Creating a new post");

        try
        {
            _postRepository.Add(post);
            _logger.LogInformation($"Post created with ID: {post.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while creating post: {ex.Message}");
            throw;
        }
    }

    /// Updates an existing post in the data repository.
    /// Logs any errors that occur during the update process.
    /// <param name="post">
    ///     The post object containing updated information. The post must already exist in the repository.
    /// </param>
    /// <exception cref="Exception">
    ///     Thrown if an error occurs while updating the post in the repository.
    /// </exception>
    public void UpdatePost(Post post)
    {
        try
        {
            _postRepository.Update(post);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while updating post: {ex.Message}");
            throw;
        }
    }

    /// Deletes a post identified by its unique identifier from the data repository.
    /// Handles any exceptions that may occur during the deletion process and logs relevant information.
    /// <param name="id">
    ///     The unique identifier of the post to delete.
    /// </param>
    public void DeletePost(string id)
    {
        try
        {
            _postRepository.Delete(id);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while deleting post: {ex.Message}");
            throw;
        }
    }
}