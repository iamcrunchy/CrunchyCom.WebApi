using CrunchyCom.Data.Models;
using CrunchyCom.Data.Repositories;

namespace CrunchyCom.Business.Services;

public class PostService : IPostService
{
    private readonly IRepository<Post> _postRepository;

    public PostService(IRepository<Post> postRepository)
    {
        _postRepository = postRepository;
    }

    public IEnumerable<Post> GetAllPosts() => _postRepository.GetAll();

    public Post? GetPostById(int id) => _postRepository.GetById(id);

    public void CreatePost(Post post) => _postRepository.Add(post);

    public void UpdatePost(Post post) => _postRepository.Update(post);

    public void DeletePost(int id) => _postRepository.Delete(id);
}