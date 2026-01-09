namespace PortfolioAdminApi.Models.Projects
{
    public class ProjectResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Techstack { get; set; }
        public string ProjectLink { get; set; }
        public string GithubLink { get; set; }
    }
}
