namespace CrunchyCom.Business.Services;

using CrunchyCom.Data.Models;
public interface IPostService
{
    IEnumerable<Post> GetAllPosts();
    Post? GetPostById(string id);
    void CreatePost(Post post);
    void UpdatePost(Post post);
    void DeletePost(string id);
}