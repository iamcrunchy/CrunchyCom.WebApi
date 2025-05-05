namespace CrunchyCom.Business.Services;

using CrunchyCom.Data.Models;
public interface IPostService
{
    IEnumerable<Post> GetAllPosts();
    Post? GetPostById(int id);
    void CreatePost(Post post);
    void UpdatePost(Post post);
    void DeletePost(int id);
}